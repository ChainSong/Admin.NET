<template>
    <!-- <el-select v-model="value" style="width: 100%;" ref='mySelected' filterable clearable remote
            v-bind:disabled="isDisabledData == 0" font-family="Helvetica Neue" placeholder="请选择" size="small"
            :remote-method="getDropDownListRemoteData">
            <el-option v-for="item in options" :key="item.value" @change="getChildrenVal" :label="item.label" :value="item">
            </el-option>
        </el-select> -->
    <el-select v-model="modelValue" style="width: 90%;" :key="modelValue" clearable filterable remote
        :disabled="props.isDisabled == 0" handleChange :placeholder="props.placeholder" allowClear show-search
        @change="valueChange" :remote-method="getDropDownListRemoteData">
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
    whereData?: any;
    placeholder?: string;
    apiUrl: string;
    isDisabled: number;
    selectData: any;
    columnData: any;
    // idValue:any;
}
const props = withDefaults(defineProps<Props>(), {
    //占位符
    placeholder: '请输入',

    value: '',
    defaultvValue: '',
    //请求参数
    whereData: '',
    //请求URL(已经弃用，可以从columnData获取)
    apiUrl: '',
    isDisabled: 0,
    //该字段的描述信息
    columnData: [],
    selectData: [],
    // keyValue:'',
});
const modelValue = ref<any>();
const isDisabled = ref<number>();
// const idValue = ref<any>();
const list = ref<any>();
watch(() => props.value, (newVal, oldVal) => {
    // console.log("newVal")
    console.log(newVal)
    console.log(oldVal)
    modelValue.value = newVal;
});



const emits = defineEmits(['select:model']);
// ,update:modelText
const valueChange = (data) => {
    console.log(data);
    if (list.value.filter(a => a.value == data).length > 0) {
        let result = list.value.filter(a => a.value == data)[0];
        emits('select:model', result);
    } else {
        emits('select:model', { "value": "", "text": "" });
    }
};

const getDropDownListRemoteData = async (data) => {
    // console.log(data);
    // console.log(props.columnData.associated);
    let result = await getSelect(props.columnData.associated, { "whereData": props.whereData, "inputData": data })
    list.value = result.data.result;
    // console.log( list.value);
}

onMounted(async () => {
    list.value = [];
    modelValue.value = props.defaultvValue;
    console.log("props.whereData");
    console.log(props.whereData);
    console.log(props);
    const result = await getSelect(props.columnData.associated, props.whereData)
    list.value = result.data.result;
    isDisabled.value = props.isDisabled;
});
</script>