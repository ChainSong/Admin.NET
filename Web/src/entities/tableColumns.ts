import Entity from './entity'
import TableColumnsDetails from './tableColumnsDetails'
// import AbpBase from "../../lib/abpbase";
export default class tableColumns extends Entity<number>{
    projectId: number;
    roleName: string;
    customerId: number;
    tableName: string;
    tableNameCH: string;
    displayName: string;
    dbColumnName: string;
    columnName: string;
    isKey: number;
    isSearchCondition: number;
    isHide: number;
    isReadOnly: number;
    isShowInList: number;
    isImportColumn: number;
    isDropDownList: number;
    isCreate: number;
    isUpdate: number;
    searchConditionOrder: number;
    associated: string;
    validation: string;
    group: string;
    type: string;
    characteristic: string;
    order: string;
    forView: string;
    creationTime: string;
    tableFormat: string;
    precision: number;
    step: number;
    max: number;
    min: number;
    default:string;
    Link:string;
    relationColumn:string;
    relationDBColumn:string;
    tableColumnsDetails: Array<TableColumnsDetails>;
}