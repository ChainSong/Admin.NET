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
            <el-table :data="tableData" style="width: 100%" height="250" v-loading="tbLoading" border>
                <el-table-column fixed prop="customerName" label="Operational Tracker 经销商出库扫描率"
                    width="350" align="center" />
                <el-table-column prop="type" label="" align="center" />
                <el-table-column prop="ytd" label="YTD" align="center">
                    <template #default="scope">
                        <span>{{ scope.row.ytd }}</span>
                    </template>
                </el-table-column>
                <el-table-column prop="jan" label="Jan" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.jan">{{ scope.row.jan }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="feb" label="Feb" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.feb">{{ scope.row.feb }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="mar" label="Mar" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.Mar">{{ scope.row.Mar }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="apr" label="Apr" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.Apr">{{ scope.row.Apr }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="may" label="May" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.may">{{ scope.row.may }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="jun" label="Jun" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.jun">{{ scope.row.jun }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="jul" label="Jul" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.jul">{{ scope.row.jul }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="aug" label="Aug" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.aug">{{ scope.row.aug }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="sep" label="Sep" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.sep">{{ scope.row.sep }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="oct" label="Oct" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.oct">{{ scope.row.oct }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="nov" label="Nov" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.nov">{{ scope.row.nov }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
                <el-table-column prop="dec" label="Dec" align="center">
                    <template #default="scope">
                        <span v-if="scope.row.dec">{{ scope.row.dec }}</span>
                        <span style="color: red;" v-else>未维护</span>
                    </template>
                </el-table-column>
            </el-table>

            <el-pagination style="text-align: right" background @size-change="onHandleSizeChange"
                @current-change="onHandleCurrentChange" :page-sizes="[10, 20, 30]" :current-page="formInline.page"
                :page-size="formInline.pageSize" layout="total, sizes, prev, pager, next, jumper"
                :total="tableDataTotal">
            </el-pagination>
        </el-card>

    </div>

</template>

<script lang="ts" name="obScanningRate" setup>
import { onMounted, reactive, ref } from 'vue'
import { QueryOperationalTrackerList, GetCustomerSelectList, ExportOperationalTrackerList } from '/@/api/main/hachRepport'
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

const getTableData = async (params: any) => {
    tbLoading.value = true
    try {
        var res = await QueryOperationalTrackerList(params)
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

import { downloadByData, getFileName } from '/@/utils/download';

const Export = async (params: any) => {
    let res = await ExportOperationalTrackerList(params);
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