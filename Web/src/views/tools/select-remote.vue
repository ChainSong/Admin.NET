<template>
    <!-- <el-select v-model="value" style="width: 100%;" ref='mySelected' filterable clearable remote
            v-bind:disabled="isDisabledData == 0" font-family="Helvetica Neue" placeholder="请选择" size="small"
            :remote-method="getDropDownListRemoteData">
            <el-option v-for="item in options" :key="item.value" @change="getChildrenVal" :label="item.label" :value="item">
            </el-option>
        </el-select> -->
    <el-select v-model="modelValue" style="width: 90%;" clearable remote filterable v-bind:disabled="props.isDisabled == 0"
        handleChange :placeholder="props.placeholder" allowClear show-search @change="valueChange"
        :remote-method="getDropDownListRemoteData">
        <el-option v-for="item in list" :key="item" :value="item.value" :label="item.text">{{ item.text }}</el-option>
    </el-select>
</template>
<script setup lang="ts">
import { onMounted, ref, watch } from 'vue';
// import Service from "@/services/apps/toolsUtil/ToolsUtilService";
import { getSelect } from '/@/api/main/toolsUtilService';
interface Props {
    value?: any;
    defaultvValue?: any;
    objData?: any;
    placeholder?: string;
    apiUrl: string;
    isDisabled: number;
    selectData: any;
    columnData: any;
}
const props = withDefaults(defineProps<Props>(), {
    //占位符
    placeholder: '请输入',

    value: '',
    defaultvValue: '',
    //请求参数
    objData: '',
    //请求URL(已经弃用，可以从columnData获取)
    apiUrl: '',
    isDisabled: 1,
    //该字段的描述信息
    columnData: [],
    selectData: [],
});
const modelValue = ref<any>();
const list = ref<any>();
watch(() => props.value, (newVal) => {
    modelValue.value = newVal;
});



const emits = defineEmits(['select:model']);
// ,update:modelText
const valueChange = (data) => {
    console.log(data);
    if (list.value.filter(a => a.value == data).length > 0) {
        let result = list.value.filter(a => a.value == data)[0];
        emits('select:model', result);
    }else{
        emits('select:model', {"value":"","text":""});
    }
};

const getDropDownListRemoteData = async (data) => {
   
    let result = await getSelect(props.columnData.associated, {"objData":props.objData,"inputData":data})
    list.value = result.data.result;
}

onMounted(async () => {
    list.value = [];
    modelValue.value = props.defaultvValue;
    console.log("props.objData");
    console.log(props.objData);
    const result = await getSelect(props.columnData.associated, props.objData)
    list.value = result.data.result;
});
</script>