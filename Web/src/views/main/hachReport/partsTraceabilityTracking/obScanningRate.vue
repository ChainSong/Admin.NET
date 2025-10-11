<template>
    <div>
        <el-card class="card-form">
            <el-form :inline="true" :model="formInline" class="demo-form-inline">
                <el-form-item label="客户名称">
                    <el-select v-model="formInline.customerId" clearable filterable size="small" placeholder="请选择"
                        @click="clickCustomerSelect" v-loading="selectLoading">
                        <el-option v-for="item in customerSelectList" :key="item.id" style="width: 100%"
                            :label="item.label" :value="item.value">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="Search(formInline)">查询</el-button>
                    <el-button color="skyblue" @click="Export(formInline)">导出</el-button>

                </el-form-item>
            </el-form>
        </el-card>

        <el-card class="card-table">
            <el-table :data="tableData" style="width: 100%" v-loading="tbLoading" border>
                <el-table-column fixed prop="customerName" label="Operational Tracker 经销商配件 Sell Thru" width="350"
                    align="center" />
                <el-table-column prop="type" label="" align="center" />
                <!-- 动态生成月份列 -->
                <template v-for="month in monthColumns" :key="month.monthName">
                    <el-table-column :label="month.monthName" align="center">
                        <el-table-column v-for="week in month.weeks" :key="'week' + week.weekNum"
                            :prop="'week' + week.weekNum" :label="'Week' + week.weekNum" align="center" width="80">
                            <template #default="scope">
                                <span
                                    v-if="scope.row.customerName && scope.row.customerName.includes('Sell Thru')">
                                    <!-- 如果是Sell Thru行，显示为百分比 -->
                                    {{ formatToPercentage(scope.row['week' + week.weekNum]) }}
                                </span>
                                <span v-else>
                                    {{ scope.row['week' + week.weekNum] }}
                                </span>
                            </template>
                        </el-table-column>
                    </el-table-column>
                </template>
            </el-table>
            <el-pagination style="text-align: right" background @size-change="onHandleSizeChange"
                @current-change="onHandleCurrentChange" :page-sizes="[10, 20, 30]" :current-page="formInline.page"
                :page-size="formInline.pageSize" layout="total, sizes, prev, pager, next, jumper"
                :total="tableDataTotal">
            </el-pagination>
        </el-card>
    </div>
</template>

<script lang="ts" name="operationalTrackerSellThru" setup>
import { onMounted, reactive, ref } from 'vue'
import { QueryOperationalOBScanningRateList, GetCustomerSelectList, ExportOperationalOBScanningRateList } from '/@/api/main/hachRepport'
const formInline = reactive({
    customerId: null,
    page: 1,
    pageSize: 10,
})
const customerSelectList = ref([])
const selectLoading = ref(false)
//点击客户下拉
const clickCustomerSelect = async () => {
    var res = await GetCustomerSelectList()
    customerSelectList.value = res.data.result ?? []
}
const tbLoading = ref(false)
const tableData = ref([])
const tableDataTotal = ref(0)

// 定义月份和周数的映射关系
const monthColumns = ref([
    { monthName: 'Jan', weeks: [{ weekNum: 1 }, { weekNum: 2 }, { weekNum: 3 }, { weekNum: 4 }] },
    { monthName: 'Feb', weeks: [{ weekNum: 5 }, { weekNum: 6 }, { weekNum: 7 }, { weekNum: 8 }] },
    { monthName: 'Mar', weeks: [{ weekNum: 9 }, { weekNum: 10 }, { weekNum: 11 }, { weekNum: 12 }] },
    { monthName: 'Apr', weeks: [{ weekNum: 13 }, { weekNum: 14 }, { weekNum: 15 }, { weekNum: 16 }] },
    { monthName: 'May', weeks: [{ weekNum: 17 }, { weekNum: 18 }, { weekNum: 19 }, { weekNum: 20 }] },
    { monthName: 'Jun', weeks: [{ weekNum: 21 }, { weekNum: 22 }, { weekNum: 23 }, { weekNum: 24 }] },
    { monthName: 'July', weeks: [{ weekNum: 25 }, { weekNum: 26 }, { weekNum: 27 }, { weekNum: 28 }] },
    { monthName: 'Aug', weeks: [{ weekNum: 29 }, { weekNum: 30 }, { weekNum: 31 }, { weekNum: 32 }] },
    { monthName: 'Sep', weeks: [{ weekNum: 33 }, { weekNum: 34 }, { weekNum: 35 }, { weekNum: 36 }] },
    { monthName: 'Oct', weeks: [{ weekNum: 37 }, { weekNum: 38 }, { weekNum: 39 }, { weekNum: 40 }] },
    { monthName: 'Nov', weeks: [{ weekNum: 41 }, { weekNum: 42 }, { weekNum: 43 }, { weekNum: 44 }] },
    { monthName: 'Dec', weeks: [{ weekNum: 45 }, { weekNum: 46 }, { weekNum: 47 }, { weekNum: 48 }] },
])


const getTableData = async (params: any) => {
    tbLoading.value = true
    try {
        var res = await QueryOperationalOBScanningRateList(params)
        tableData.value = res.data.result.data
        tableDataTotal.value = res.data.result.total ?? 0
    } catch (error) {
        console.error(error)
    } finally {
        tbLoading.value = false
    }

}

const Search = async (params: any) => {
    await getTableData(params)
}

const formatToPercentage = (value: any) => {
    if (value === null || value === undefined || value === '') {
        return '0.00%';
    }

    // 将数值转换为百分比格式
    const numValue = parseFloat(value);
    if (isNaN(numValue)) {
        return '0.00%';
    }

    // 格式化为两位小数的百分比
    return (numValue * 100).toFixed(2) + '%';
}

import { downloadByData, getFileName } from '/@/utils/download';

const Export = async (params: any) => {
    let res = await ExportOperationalOBScanningRateList(params);
    var fileName = getFileName(res.headers);
    downloadByData(res.data as any, fileName);
}
// 分页点击
const onHandleSizeChange = async (val: number) => {
    formInline.pageSize = val
    await getTableData(formInline)

};
// 分页点击
const onHandleCurrentChange = async (val: number) => {
    formInline.page = val
    await getTableData(formInline)
};
onMounted(async () => {
    await getTableData(formInline)
})
</script>
<style lang="css" scoped>
.div {
    height: 100%
}

.card-form {
    height: 10%
}

.card-table {
    height: 100%
}
</style>