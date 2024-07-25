<template>
    <div class="wMSPreOrder-container">
      <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
        <el-form :model="queryParams" ref="queryForm" :inline="true">
          <el-row :gutter="[16, 15]">
            <template v-for="i in  state.tableColumnHeaders">
              <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6" v-if="i.isSearchCondition" :key="i">
                <template v-if="i.type == 'TextBox'">
                  <el-form-item class="mb-0" :label="i.displayName">
                    <el-input v-model="state.header[i.dbColumnName]" :placeholder="i.displayName" />
                  </el-form-item>
                </template>
                <template v-if="i.type == 'DropDownListInt'">
                  <el-form-item class="mb-0" :label="i.displayName">
                    <el-select v-model="state.header[i.columnName]" clearable filterable v-if="i.isSearchCondition"
                      size="small" placeholder="请选择">
                      <el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt" style="width: 100%"
                        :label="item.name" :value="item.codeInt">
                      </el-option>
                    </el-select>

                  </el-form-item>

                </template>
                <template v-if="i.type == 'DropDownListStrRemote'">
                  <el-form-item class="mb-0" :label="i.displayName">
                    <select-Remote :whereData="state.header" :isDisabled="i.isSearchCondition" :columnData="i"
                      :defaultvValue="state.header[i.columnName]"
                      @select:model="data => { state.header[i.columnName] = data.text; state.header[i.relationColumn] = data.value; console.log(state.header) }"></select-Remote>
                  </el-form-item>
                </template>


                <template v-if="i.type == 'DropDownListStr'">
                  <el-form-item class="mb-0" :label="i.displayName">
                    <el-select v-model="state.header[i.columnName]" clearable filterable v-if="i.isSearchCondition"
                      size="small" placeholder="请选择">
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
              <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSPreOrder:page'"> 查询
              </el-button>
              <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
            </el-button-group>

          </el-form-item>
          <el-form-item>
            <el-button-group>
              <el-button type="primary" icon="ele-Download" @click="exportPreOrderFun" v-auth="'wMSPreOrder:export'"> 导出
              </el-button>
              <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
            </el-button-group>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" icon="ele-Plus" @click="openAdd" v-auth="'wMSPreOrder:add'"> 新增
            </el-button>
          </el-form-item>

          <el-form-item>
            <el-button type="primary" icon="ele-Fold" @click="PreOrderForOrderFun" v-auth="'wMSPreOrder:preOrderForOrder'"> 转出库单
            </el-button>
          </el-form-item>
        </el-form>
      </el-card>
      <el-card class="full-table" shadow="hover" style="margin-top: 8px">

        <el-table :data="state.headers" ref="multipleTableRef" show-overflow-tooltip tooltip-effect="light" row-key="id"
          style="width: 100%">
          <el-table-column type="selection" width="55">
          </el-table-column>
          <template v-for="v in state.tableColumnHeaders">
            <template v-if="v.isShowInList">
              <el-table-column v-if="v.type == 'DropDownListInt'" v-bind:key="v.columnName" :fixed="false"
                :prop="v.columnName" :label="v.displayName" width="150" max-height="50">
                <template #default="scope">
                  <template v-for="item in v.tableColumnsDetails">
                    <el-tag v-if="item.codeInt == state.headers[scope.$index][v.columnName]" v-bind:key="item.codeStr"
                      show-icon :type="item.color">
                      {{ item.name }}
                    </el-tag>
                  </template>
                </template>
              </el-table-column>
              <el-table-column v-else-if="v.type == 'DropDownListStr'" v-bind:key="v.columnName" :fixed="false"
                :prop="v.columnName" :label="v.displayName" width="150" max-height="50">
                <template #default="scope">
                  <template v-for="item in v.tableColumnsDetails">
                    <el-tag v-if="item.codeStr == state.headers[scope.$index][v.columnName]" v-bind:key="item.codeStr"
                      show-icon :type="item.color">
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
          <el-table-column fixed="right" label="操作" width="280">

            <template #default="scope">
              <el-button @click="openQuery(scope.row)" class="el-icon-s-comment" type="text" size="small">查看
              </el-button>
              <el-button @click="openEdit(scope.row)" class="el-icon-edit" type="text" v-auth="'wMSPreOrder:edit'" size="small">编辑</el-button>
              <el-button @click="openCancel(scope.row)" class="el-icon-edit" type="text" v-auth="'wMSPreOrder:edit'" size="small">取消</el-button>
              <el-button @click="openEditAddres(scope.row)" class="el-icon-edit" v-auth="'wMSPreOrder:editAddress'" type="text" size="small">编辑地址</el-button>
              <!--   <el-popconfirm confirm-button-text="确定"  cancel-button-text="取消"
                icon="el-icon-info" icon-color="red" @confirm="handleDelete(scope.row)" title="确定删除吗？">
                <el-button   type="text" class="el-icon-delete" style="color:#F56C6C;margin-left: 10px;"
                  size="small">删除</el-button>
              </el-popconfirm> -->
            </template>
          </el-table-column>
        </el-table>

        <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
          :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
          @size-change="handleSizeChange" @current-change="handleCurrentChange"
          layout="total, sizes, prev, pager, next, jumper" />
        <editDialog ref="editDialogRef" :title="editTitle" @reloadTable="handleQuery" />
        <addDialog ref="addDialogRef" :title="addTitle" @reloadTable="handleQuery" />
        <queryDialog ref="queryDialogRef" :title="queryTitle" @reloadTable="handleQuery" />
        <editAddresDialog ref="editAddresDialogRef" :title="addresTitle" @reloadTable="handleQuery" />
      </el-card>
      
    <el-dialog v-model="resultPopupShow" :title="resultTitle" :append-to-body="true">
      <el-alert v-for="i in state.orderStatus" v-bind="i" :key="i" :title="i.externOrder + i.msg" :type="i.statusMsg">
      </el-alert>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup="" name="wMSPreOrder">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editAddresDialog from '/@/views/main/wMSPreOrder/component/editAddresDialog.vue'
import editDialog from '/@/views/main/wMSPreOrder/component/editDialog.vue'
import addDialog from '/@/views/main/wMSPreOrder/component/addDialog.vue'
import queryDialog from '/@/views/main/wMSPreOrder/component/queryDialog.vue'
import { pageWMSPreOrder, deleteWMSPreOrder, preOrderForOrder,exportPreOrder,cancelWMSPreOrder } from '/@/api/main/wMSPreOrder';
import { getByTableNameList } from "/@/api/main/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue';
import Header from "../../../entities/preOrder";
import Details from "../../../entities/preOrderDetail";
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";
import orderStatus from "/@/entities/orderStatus";
import { downloadByData, getFileName } from '/@/utils/download';

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
  tableColumnDetails: new Array<TableColumns>(),
  //自定义提示
  orderStatus: new Array<orderStatus>(),
  // tableColumn: new TableColumns(),
  // tableColumns: new Array<TableColumns>(),
  // tableColumnsDetails: new Array<TableColumnsDetails>(),
  //   tableColumnsDetail = ref();
});

const editAddresDialogRef = ref();
const editDialogRef = ref();
const addDialogRef = ref();
const queryDialogRef = ref();
const loading = ref(false);
const multipleTableRef = ref();
const resultTitle = ref();

// const select_order_number = ref('') //表格select选中的条数
// const multipleSelection = ref([])
//自定义提示
const resultPopupShow = ref(false);
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
const addresTitle = ref("");

// 页面加载时
onMounted(async () => {
  gettableColumn();
});

const gettableColumn = async () => {
  let res = await getByTableNameList("WMS_PreOrder");
  state.value.tableColumnHeaders = res.data.result;
};

// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSPreOrder(Object.assign(state.value.header, tableParams.value));
  state.value.headers = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAdd = () => {
  addTitle.value = '添加';
  addDialogRef.value.openDialog({});
};

// 打开编辑地址页面
const openEditAddres = (row: any) => {
  addresTitle.value = '查看';
  editAddresDialogRef.value.openDialog(row);
};
 

// 打开编辑页面
const openEdit = (row: any) => {

  if (row.preOrderStatus != 1) {
    ElMessage.warning("订单状态不允许编辑");
    return;
  }
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
      await deleteWMSPreOrder(row);
      handleQuery();
      ElMessage.success("删除成功");
    })
    .catch(() => { });
};



// 取消
const openCancel = (row: any) => {
  ElMessageBox.confirm(`确定要取消吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await cancelWMSPreOrder(row);
      handleQuery();
      ElMessage.success("取消成功");
    })
    .catch(() => { });
};

// 转入库单
const PreOrderForOrderFun = () => {
  resultTitle.value="转出库单结果";
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    if (a.preOrderStatus == 1) {
      ids.push(a.id);
    }
  });
  if (ids.length == 0) {
    ElMessage.warning("请勾选新增订单");
    return;
  }
  ElMessageBox.confirm(`确定要转出库单吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      let result = await preOrderForOrder(ids);
      handleQuery();
      if (result.data.result.data.length > 0) {
        state.value.orderStatus = result.data.result.data;
        resultPopupShow.value = true;
      } else {
        ElMessage.info(result.data.result.msg);
      }
    })
    .catch(() => { });
};


//导出预出库单
const exportPreOrderFun = async () => {
  //1 获取选中的订单ID
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  // 2,验证数据有没有勾选
  if (ids.length < 1) {
    ElMessage.error("请勾选订单");
    return;
  }
  let res = await exportPreOrder(ids);
  var fileName = getFileName(res.headers);
  downloadByData(res.data as any, fileName);
}


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


