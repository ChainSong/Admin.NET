<template>
  <div class="wMSInventoryUsable-container">
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
              <template v-else-if="i.type == 'DropDownListInt'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-select v-model="state.header[i.columnName]" v-if="i.isSearchCondition" size="small"
                    placeholder="请选择">
                    <el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt" style="width: 100%"
                      :label="item.name" :value="item.codeInt">
                    </el-option>
                  </el-select>
                </el-form-item>

              </template>
              <template v-else-if="i.type == 'DropDownListStr'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-select v-model="state.header[i.columnName]" v-if="i.isSearchCondition" size="small"
                    placeholder="请选择">
                    <el-option v-for="item in i.tableColumnsDetails" :key="item.codeStr" style="width: 100%"
                      :label="item.name" :value="item.codeStr">
                    </el-option>
                  </el-select>
                </el-form-item>
              </template>

              <template v-else-if="i.type == 'DropDownListStrRemote'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <select-Remote :whereData="state.header" :isDisabled="i.isSearchCondition" :columnData="i"
                    @select:model="(data)=>{state.header[i.columnName]=data.text;state.header[i.relationColumn]=data.value;console.log(state.header)}"></select-Remote>
                </el-form-item>
              </template>

              <template v-else-if="i.type == 'DatePicker'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-date-picker v-model="state.header[i.columnName]" type="daterange" size="small"
                    v-if="i.isSearchCondition" range-separator="~" start-placeholder="开始日期" end-placeholder="结束日期"
                    style="width: 100%">
                  </el-date-picker>
                </el-form-item>
              </template>
              <template v-else-if="i.type == 'DateTimePicker'">
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
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSInventoryUsable:page'"> 查询 </el-button>
          </el-button-group>
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="invrntoryExport" v-auth="'wMSInventoryUsable:page'"> 下载 </el-button>
          </el-button-group>
        </el-form-item>
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAdd" v-auth="'wMSInventoryUsable:add'"> 新增
          </el-button>
        </el-form-item> -->
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="state.headers" show-overflow-tooltip tooltip-effect="light" row-key="id" style="width: 100%">
        <template v-for="v in state.tableColumnHeaders">
          <template v-if="v.isShowInList">
            <el-table-column v-if="v.type == 'DropDownListInt'" v-bind:key="v.columnName" :fixed="false"
              :prop="v.columnName" :label="v.displayName" width="150" max-height="50">
              <template #default="scope">
                <template v-for="item in v.tableColumnsDetails">
                  <el-tag v-if="item.codeInt == state.headers[scope.$index][v.columnName]" :key="item.codeInt"
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
                  <el-tag v-if="item.codeStr == state.headers[scope.$index][v.columnName]" :key="item.codeStr"
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
        <!-- <el-table-column fixed="right" label="操作" width="200">
          <template #default="scope">
            <el-button @click="openQuery(scope.row)" class="el-icon-s-comment" type="text" size="small">查看
            </el-button>
            <el-button @click="openEdit(scope.row)" class="el-icon-edit" type="text" size="small">编辑</el-button>
          </template>
        </el-table-column> -->
      </el-table>

      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background="" @size-change="handleSizeChange"
        @current-change="handleCurrentChange" layout="total, sizes, prev, pager, next, jumper" />
      <!-- <editDialog ref="editDialogRef" :title="editTitle" @reloadTable="handleQuery" />
      <addDialog ref="addDialogRef" :title="addTitle" @reloadTable="handleQuery" />
      <queryDialog ref="queryDialogRef" :title="queryTitle" @reloadTable="handleQuery" /> -->
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSInventoryUsable">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
// import editDialog from '/@/views/main/wMSInventoryUsable/component/editDialog.vue'
// import addDialog from '/@/views/main/wMSInventoryUsable/component/addDialog.vue'
// import queryDialog from '/@/views/main/wMSInventoryUsable/component/queryDialog.vue'
import selectRemote from '/@/views/tools/select-remote.vue';

// import { pageWMSInventoryUsable, deleteWMSInventoryUsable } from '/@/api/main/wMSInventoryUsable';
import { invrntoryDataPage,invrntoryDataExport } from '/@/api/main/wMSInventoryReport';
import { getByTableNameList } from "/@/api/main/tableColumns";

import Header from "/@/entities/InventoryUsable";
// import Details from "/@/entities/InventoryUsableDetail";
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";
import { downloadByData, getFileName,downloadByInterface } from '/@/utils/download';
import { json } from "stream/consumers";
import { stringify } from "querystring";

const state = ref({
  vm: {
    id: "",

    // form: {
    //     InventoryUsableDetails: []
    // } as any,
  },
  visible: false,
  loading: false,
  header: new Header(),
  headers: new Array<Header>(),
  // details: new Array<Details>(),
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

// const editDialogRef = ref();
// const addDialogRef = ref();
// const queryDialogRef = ref();
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
// const editTitle = ref("");
// const addTitle = ref("");
// const queryTitle = ref("");

// 页面加载时
onMounted(async () => {
  gettableColumn();
});

const gettableColumn = async () => {
  let res = await getByTableNameList("WMS_Inventory_Usable_Report");
  state.value.tableColumnHeaders = res.data.result;
};

// 查询操作
const handleQuery = async () => {
  loading.value = true; 
  console.log("dasdsadasd");
  console.log(JSON.stringify(Object.assign(state.value.header, tableParams.value)));
  var res = await invrntoryDataPage(Object.assign(state.value.header, tableParams.value));
  state.value.headers = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};
 

// 改变页面容量
const handleSizeChange = (val: number) => {
  tableParams.value.pageSize = val;
  handleQuery();
};

//  下载
const invrntoryExport =async (val: number) => {
    // let res = await invrntoryDataExport(state.value.header);
  // var fileName = getFileName(res.headers);
   downloadByInterface(invrntoryDataExport,state.value.header)
  // let res = await invrntoryDataExport(state.value.header);
  // var fileName = getFileName(res.headers);
  // downloadByData(res.data as any, fileName);
};




// 改变页码序号
const handleCurrentChange = (val: number) => {
  tableParams.value.page = val;
  handleQuery();
};


handleQuery();
</script>


