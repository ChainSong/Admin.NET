<template>
    <div>

        <el-card>
            <el-form :inline="true" :model="formInline" class="demo-form-inline">
                <el-form-item label="客户名称">
                    <el-input v-model="formInline.customerName" placeholder="请输入客户名称" clearable />
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="Search(formInline)">查询</el-button>
                </el-form-item>
            </el-form>
        </el-card>

        <el-card>
            <el-table :data="tableData" style="width: 100%" height="250" border>
                <el-table-column fixed prop="customerName" label="Operational Tracker &nbsp; 经销商配件库存健康度2.5X Month"
                    width="350" align="center" />
                <el-table-column prop="type" label="" align="center" />
                <el-table-column prop="ytd" label="YTD" align="center" />
                <el-table-column prop="jul" label="Jul" align="center" />
                <el-table-column prop="aug" label="Aug" align="center" />
                <el-table-column prop="sep" label="Sep" align="center" />
                <el-table-column prop="oct" label="Oct" align="center" />
                <el-table-column prop="nov" label="Nov" align="center" />
                <el-table-column prop="dec" label="Dec" align="center" />
            </el-table>
        </el-card>

    </div>

</template>

<script lang="ts" setup>
import { onMounted, reactive, ref } from 'vue'
import { QueryOperationalTrackerList } from '/@/api/main/hachRepport'
const formInline = reactive({
    customerName: '',
})

const tableData = ref([])
const getTableData = async (params: any) => {
    var res = await QueryOperationalTrackerList(params)
    console.log("res", res)
}

const Search = async (params: any) => {
    var res = await QueryOperationalTrackerList(params)
    console.log("res", res)
}

onMounted(async () => {
    await getTableData(formInline)
})
</script>