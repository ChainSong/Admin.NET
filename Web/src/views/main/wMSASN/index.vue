﻿<template>
  <div class="wMSASN-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">

        <el-row :gutter="[16, 15]">
          <template v-for="o in state.presetQuery.presetQueryList ">
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSASN:page'"> 查询 </el-button>
          </template>
        </el-row>
        <el-row :gutter="[16, 15]">
          <template v-for="i in state.tableColumnHeaders">
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
          <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSASN:page'"> 查询 </el-button>
          <el-button type="primary" icon="ele-Search" @click="saveQuery" v-auth="'wMSASN:page'"> 保存查询条件 </el-button>

          <el-button type="primary" icon="ele-Plus" @click="openAdd" v-auth="'wMSASN:add'"> 新增
          </el-button>

          <el-button type="primary" icon="ele-Download" @click="exportASNFun" v-auth="'wMSASN:export'"> 导出
          </el-button>

          <el-button type="primary" icon="ele-Fold" @click="asnForReceiptFun" v-auth="'wMSASN:add'"> 转入库单(全部)
          </el-button>

          <el-button type="primary" icon="ele-Fold" @click="asnCountQuantity" v-auth="'wMSASN:add'"> 点数</el-button>
        </el-form-item>

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">

      <el-table :data="state.headers" ref="multipleTableRef" show-overflow-tooltip tooltip-effect="light" row-key="id">

        <el-table-column type="selection" width="55">
        </el-table-column>
        <template v-for="v in state.tableColumnHeaders">
          <template v-if="v.isShowInList">
            <el-table-column v-if="v.type == 'DropDownListInt'" v-bind:key="v.columnName" :fixed="false"
              :prop="v.columnName" :label="v.displayName" max-height="50">
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
              :prop="v.columnName" :label="v.displayName" max-height="50">
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
        <el-table-column fixed="right" label="操作" width="340">
          <template #header>
            <el-select placeholder="请选择">

              <el-option v-for="item in state.tableColumnHeaders.filter(a => a.isCreate == 1)" :key="item.value">
                <el-checkbox @change="checked => showColumnOption(checked, item)" :true-label="1" :false-label="0"
                  :label="item.displayName" :key="item.columnName" v-model="item.isShowInList">{{ item.displayName
                  }}</el-checkbox>
              </el-option>
            </el-select>
          </template>
          <template #default="scope">
            <el-button @click="openQuery(scope.row)" class="el-icon-s-comment" type="text" size="small">查看
            </el-button>
            <el-button @click="openCancel(scope.row)" class="el-icon-s-comment" type="text" size="small">取消
            </el-button>
            <el-button @click="openEdit(scope.row)" class="el-icon-edit" type="text" size="small">编辑</el-button>
            <el-button @click="openAsnforReceipt(scope.row)" class="el-icon-edit" type="text"
              size="small">转入库单(部分)</el-button>
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
      <asnforReceiptDialog ref="openAsnforReceiptRef" :title="asnforReceiptTitle" @reloadTable="handleQuery" />
    </el-card>
    <el-dialog v-model="resultPopupShow" title="转入库单结果" :append-to-body="true">
      <el-alert v-for="i in state.orderStatus" v-bind="i" :key="i" :title="i.externOrder + i.msg" :type="i.statusMsg">
      </el-alert>
    </el-dialog>
    <el-dialog title="提示" v-model="state.presetQuery.saveQueryVisible" width="30%">
      <span><el-input v-model="state.presetQuery.queryName" placeholder="请输入内容"></el-input></span>
      <span class="dialog-footer">
        <el-button @click="state.presetQuery.saveQueryVisible = false">取 消</el-button>
        <el-button type="primary" @click="saveQueryFun">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup="" name="wMSASN">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/wMSASN/component/editDialog.vue'
import addDialog from '/@/views/main/wMSASN/component/addDialog.vue'
import queryDialog from '/@/views/main/wMSASN/component/queryDialog.vue'
import asnforReceiptDialog from '/@/views/main/wMSASN/component/asnforReceiptDialog.vue'
import { pageWMSASN, deleteWMSASN, asnForReceipt, exportASN, cancelWMSASN } from '/@/api/main/wMSASN';
import { addWMSASNCountQuantity } from '/@/api/main/wMSASNCountQuantity';
import { getByTableNameList } from "/@/api/main/tableColumns";
import { addSysPresetQuery, queryByUser } from "/@/api/main/sysPresetQuery";
import selectRemote from '/@/views/tools/select-remote.vue'
import Header from "/@/entities/asn";
import Details from "/@/entities/asnDetail";
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";
import orderStatus from "/@/entities/orderStatus";
import { downloadByData, getFileName } from '/@/utils/download';
import { stringify } from "querystring";

const state = ref({
  vm: {
    id: "",

    // form: {
    //     customerDetails: []
    // } as any,
  },
  visible: false,
  presetQuery: {
    saveQueryVisible: false,
    queryName: "",
    businessName: "ASN查询",
    queryForm: "",
  },
  presetQueryList: new Array<any>(),
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
});

const editDialogRef = ref();
const openAsnforReceiptRef = ref();
const addDialogRef = ref();
const queryDialogRef = ref();
const loading = ref(false);
const multipleTableRef = ref();
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
const asnforReceiptTitle = ref("");

// 页面加载时
onMounted(async () => {
  gettableColumn();
});

const showColumnOption = async (value: any, item: any) => {
  if (value == 1) {
    item.isShowInList = 1;
  } else {
    item.isShowInList = 0;
  }
};
const gettableColumn = async () => {
  let res = await getByTableNameList("WMS_ASN");
  state.value.tableColumnHeaders = res.data.result;

};
// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSASN(Object.assign(state.value.header, tableParams.value));
  state.value.headers = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAdd = () => {
  addTitle.value = '添加';
  addDialogRef.value.openDialog({});
};


// 打开转入库单页面
const openAsnforReceipt = (row: any) => {

  if (row.asnStatus == 99 || row.asnStatus == 10 || row.asnStatus == -1) {
    ElMessage.warning("订单状态不允许转入库单");
    return;
  }
  asnforReceiptTitle.value = '转入库单';
  openAsnforReceiptRef.value.openDialog(row);
};



// 打开编辑页面
const openEdit = (row: any) => {
  if (row.asnStatus != 1) {
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

  if (row.asnStatus != 1) {
    ElMessage.warning("订单状态不允许删除");
    return;
  }
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteWMSASN(row);
      handleQuery();
      ElMessage.success("删除成功");
    })
    .catch(() => { });
};



// 取消
const openCancel = (row: any) => {

  if (row.asnStatus != 1) {
    ElMessage.warning("订单状态不允许取消");
    return;
  }

  ElMessageBox.confirm(`确定要取消吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await cancelWMSASN(row);
      handleQuery();
      ElMessage.success("取消成功");
    })
    .catch(() => { });
};


// 点数
const asnCountQuantity = () => {
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });

  ElMessageBox.confirm(`确定要生成点数单吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      let result = await addWMSASNCountQuantity(ids);
      // console.log("result");
      // console.log(result);
      if (result.data.result.code == 1) {
        handleQuery();
        ElMessage.success("点数单生成成功");
      } else {
        resultPopupShow.value = true;
        state.value.orderStatus = result.data.result.data;
      }
    })
    .catch(() => { });
};

// 转入库单
const asnForReceiptFun = () => {
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });

  ElMessageBox.confirm(`确定要转入库单吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      let result = await asnForReceipt(ids);
      // console.log("result");
      // console.log(result);
      if (result.data.result.code == 1) {
        handleQuery();
        ElMessage.success("转入库单成功");
      } else {
        resultPopupShow.value = true;
        state.value.orderStatus = result.data.result.data;
      }
    })
    .catch(() => { });
};


//导出预出库单
const exportASNFun = async () => {
  //1 获取选中的订单ID
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  // // 2,验证数据有没有勾选
  // if (ids.length < 1) {
  //   ElMessage.error("请勾选订单");
  //   return;
  // }
  if (ids.length >= 1) {
    let res = await exportASN({ "ids": ids });
    var fileName = getFileName(res.headers);
    downloadByData(res.data as any, fileName);
  } else {
    let res = await exportASN(state.value.header);
    var fileName = getFileName(res.headers);
    downloadByData(res.data as any, fileName);
  }
}

//保存查询条件弹框 
const saveQuery = (val: number) => {
  state.value.presetQuery.saveQueryVisible = true;
};

//保存查询条件弹框 
const saveQueryFun = (val: number) => {
  state.value.presetQuery.saveQueryVisible = false;
  ElMessageBox.confirm(`确定要保存当前查询条件吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      state.value.presetQuery.queryForm = JSON.stringify(state.value.header);
      let result = await addSysPresetQuery(state.value.presetQuery);
      queryPresetQuery();
      ElMessage.success("保存成功");
    })
    .catch(() => { });


};

//保存查询条件弹框 
const queryPresetQuery = async () => {
  var res = await queryByUser(state.value.presetQuery);
  state.value.presetQuery.presetQueryList = res.data.result.data;
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
  queryPresetQuery();
};


handleQuery();
</script>
