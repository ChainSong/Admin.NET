<template>
  <div class="wMSCustomer-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-row :gutter="[16, 15]">
          <template v-for="i in  state.tableColumnHeaders">
            <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6" v-if="i.isSearchCondition" :key="i">
              <!-- <template v-if="v.type == 'TextBox'">
                <el-form-item class="mb-0" :label="v.displayName">
                  <el-input v-model="state.header[v.dbColumnName]" :placeholder="v.displayName" />
                </el-form-item>
              </template> -->
              <template v-if="i.type == 'TextBox'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-input v-model="state.header[i.dbColumnName]" :placeholder="i.displayName" />
                </el-form-item>
              </template>
              <template v-if="i.type == 'DropDownListInt'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-select v-model="state.header[i.columnName]" v-if="i.isSearchCondition" size="small"
                    placeholder="请选择">
                    <el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt" style="width: 100%"
                      :label="item.name" :value="item.codeInt">
                    </el-option>
                  </el-select>
                  <!-- <el-input v-model="state.header[i.dbColumnName]" :placeholder="i.displayName" /> -->
                </el-form-item>

              </template>
              <template v-if="i.type == 'DropDownListStr'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-select v-model="state.header[i.columnName]" v-if="i.isSearchCondition" size="small"
                    placeholder="请选择">
                    <el-option v-for="item in i.tableColumnsDetails" :key="item.codeStr" style="width: 100%"
                      :label="item.name" :value="item.codeStr">
                    </el-option>
                  </el-select>
                </el-form-item>
              </template>
              <template v-if="i.type == 'DatePicker'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-date-picker v-model="state.header[i.columnName]" type="daterange" size="small"
                    v-if="i.isSearchCondition" range-separator="~" start-placeholder="开始日期" end-placeholder="结束日期"
                    style="width: 100%">
                  </el-date-picker>
                </el-form-item>
              </template>
              <template v-if="i.type == 'DateTimePicker'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-date-picker v-model="state.header[i.columnName]" v-if="i.isSearchCondition" size="small"
                    type="datetimerange" range-separator="~" start-placeholder="开始日期" end-placeholder="结束日期"
                    style="width: 100%">
                  </el-date-picker>
                </el-form-item>
              </template>
            </el-col>
          </template>
        </el-row>

        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSCustomer:page'"> 查询 </el-button>
            <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
          </el-button-group>

        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAdd" v-auth="'wMSCustomer:add'"> 新增
          </el-button>

        </el-form-item>

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <!-- <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id" border="">
        <el-table-column type="index" label="序号" width="55" align="center" />
        <el-table-column prop="projectId" label="ProjectId" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="customerCode" label="客户代码" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="customerName" label="客户名称" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="description" label="Description" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="customerType" label="CustomerType" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="customerStatus" label="CustomerStatus" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="creditLine" label="CreditLine" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="province" label="Province" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="city" label="City" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="address" label="Address" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="remark" label="Remark" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="email" label="Email" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="phone" label="Phone" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="lawPerson" label="LawPerson" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="postCode" label="PostCode" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="bank" label="Bank" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="account" label="Account" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="taxId" label="TaxId" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="invoiceTitle" label="InvoiceTitle" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="fax" label="Fax" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="webSite" label="WebSite" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="creator" label="Creator" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="creationTime" label="CreationTime" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="updator" label="Updator" fixed="" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip=""
          v-if="auth('wMSCustomer:edit') || auth('wMSCustomer:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSCustomer(scope.row)"
              v-auth="'wMSCustomer:edit'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSCustomer(scope.row)"
              v-auth="'wMSCustomer:delete'"> 删除 </el-button>
          </template>
        </el-table-column>
      </el-table> -->

      <el-table :data="state.headers" show-overflow-tooltip tooltip-effect="light" row-key="id" style="width: 100%">
        <template v-for="v in state.tableColumnHeaders">
          <template v-if="v.isShowInList">
            <el-table-column v-if="v.type == 'DropDownListInt'" v-bind:key="v.columnName" :fixed="false"
              :prop="v.columnName" :label="v.displayName" width="150" max-height="50">
              <template #default="scope">
                <template v-for="item in v.tableColumnsDetails">
                  <el-tag  v-if="item.codeInt ==  state.headers[scope.$index][v.columnName]" v-bind:key="item.codeStr" show-icon
                    :type="item.color">
                    {{ item.name }}
                  </el-tag>
                </template>
              </template>
            </el-table-column>
            <el-table-column v-else-if="v.type == 'DropDownListStr'" v-bind:key="v.columnName" :fixed="false"
              :prop="v.columnName" :label="v.displayName" width="150" max-height="50">
              <template #default="scope">
                <template v-for="item in v.tableColumnsDetails">
                  <el-tag  v-if="item.codeStr ==  state.headers[scope.$index][v.columnName]" v-bind:key="item.codeStr" show-icon
                    :type="item.color">
                    {{ item.name }}
                  </el-tag>
                </template>
              </template>
            </el-table-column>
            <el-table-column v-else v-bind:key="v.id" :fixed="false" :prop="v.columnName" :label="v.displayName"
              width="150" max-height="50">
            </el-table-column>
          </template>
        </template>
        <el-table-column fixed="right" label="操作" width="200">

          <template #default="scope">
            <el-button @click="openQuery(scope.row)" class="el-icon-s-comment" type="text" size="small">查看
            </el-button>
            <el-button @click="openEdit(scope.row)" class="el-icon-edit"  
                type="text" size="small">编辑</el-button> 
              <!--   <el-popconfirm confirm-button-text="确定"  cancel-button-text="取消"
                icon="el-icon-info" icon-color="red" @confirm="handleDelete(scope.row)" title="确定删除吗？">
                <el-button   type="text" class="el-icon-delete" style="color:#F56C6C;margin-left: 10px;"
                  size="small">删除</el-button>
              </el-popconfirm> -->

          </template>
        </el-table-column>
      </el-table>

      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background="" @size-change="handleSizeChange"
        @current-change="handleCurrentChange" layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editTitle" @reloadTable="handleQuery" />
      <addDialog ref="addDialogRef" :title="addTitle" @reloadTable="handleQuery" />
      <queryDialog ref="queryDialogRef" :title="queryTitle" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSCustomer">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/wMSCustomer/component/editDialog.vue'
import addDialog from '/@/views/main/wMSCustomer/component/addDialog.vue'
import queryDialog from '/@/views/main/wMSCustomer/component/queryDialog.vue'
import { pageWMSCustomer, deleteWMSCustomer } from '/@/api/main/wMSCustomer';
import { getByTableNameList } from "/@/api/main/tableColumns";

import Header from "/@/entities/customer";
import Details from "/@/entities/customerDetail";
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";


const state = ref({
  vm: {
    id: "",

    // form: {
    //     customerDetails: []
    // } as any,
  },
  visible: false,
  loading: false,
  header: new Header(),
  headers: new Array<Header>(),
  details: new Array<Details>(),
  // header: new Array<Details>(),

  tableColumnHeader: new TableColumns(),
  tableColumnHeaders: new Array<TableColumns>(),
  tableColumnDetail: new TableColumns(),
  tableColumnDetails: new Array<TableColumns>()

  // tableColumn: new TableColumns(),
  // tableColumns: new Array<TableColumns>(),
  // tableColumnsDetails: new Array<TableColumnsDetails>(),
  //   tableColumnsDetail = ref();
});

const editDialogRef = ref();
const addDialogRef = ref();
const queryDialogRef = ref();
const loading = ref(false);
// const tableData = ref<any>
// ([]);
const queryParams = ref<any>
  ({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const editTitle = ref("");
const addTitle = ref("");
const queryTitle = ref("");

// 页面加载时
onMounted(async () => {
  gettableColumn();
});

const gettableColumn = async () => {

  let res = await getByTableNameList("Customer");
  state.value.tableColumnHeaders = res.data.result;
  // console.log(" state.value.tableColumnHeaders")
  // console.log(state.value.tableColumnHeaders)
  // console.log(state.value.header)
  // let resDetail = await getByTableNameList("CustomerDetail");
  // state.value.tableColumnHeaders = res.data.result;

};

// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSCustomer(Object.assign(state.value.header, tableParams.value));
  state.value.headers = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAdd= () => {
  addTitle.value = '添加';
  addDialogRef.value.openDialog({});
};

// 打开编辑页面
const openEdit = (row: any) => {
  editTitle.value = '编辑';
  editDialogRef.value.openDialog(row);
};
// 打开查询页面
const openQuery = (row: any) => {
  queryTitle.value = '查看';
  queryDialogRef.value.openDialog(row);
};

// 删除
const del = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteWMSCustomer(row);
      handleQuery();
      ElMessage.success("删除成功");
    })
    .catch(() => { });
};

// 改变页面容量
const handleSizeChange = (val: number) => {
  tableParams.value.pageSize = val;
  handleQuery();
};

// 改变页码序号
const handleCurrentChange = (val: number) => {
  tableParams.value.page = val;
  handleQuery();
};


handleQuery();
</script>


