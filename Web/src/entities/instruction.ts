import Entity from './entity'
import TableColumnsDetails from './tableColumnsDetails'
// import AbpBase from "../../lib/abpbase";
export default class instruction extends Entity<number>{
    customerId: number;
    customerName: string;
    warehouseId: number;
    warehouseName: string;
    tableName: string;
    instructionType: string;
    businessType: string;
    operationId: number;
    instructionStatus: number;
    instructionTaskNo: string;
    instructionPriority: number;
    message: string;
    remark: string;
    creator: string;
    creationTime: string;
}