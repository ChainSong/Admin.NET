import Entity from './entity'
import TableColumnsDetails from './tableColumnsDetails'
// import AbpBase from "../../lib/abpbase";
export default class pickTaskDetail extends Entity<number>{
    pickTaskId: number;
    inventoryId: number;
    orderId: number;
    orderDetailId: number;
    customerId: number;
    customerName: string;
    warehouseId: number;
    warehouseName: string;
    externOrderNumber: string;
    orderNumber: string;
    pickTaskNumber: string;
    pickStatus: number;
    pikcTime: string;
    pickBoxNumber
    pickQty: number;
    area: string;
    location: string;
    sKU: string;
    uPC: string;
    goodsType: string;
    goodsName: string;
    unitCode: string;
    onwer: string;
    boxCode: string;
    trayCode: string;
    batchCode: string;
    lotCode: String;
    poCode: String;
    weight: Number;
    volume: Number;
    qty: number;
    productionDate: string;
    expirationDate: string;
    remark: string;
    creator: string;
    creationTime: string;
    str1: string;
    str2: string;
    str3: string;
    str4: string;
    str5: string;
    dateTime1: string;
    dateTime2: string;
    int1: number;
    int2: number;
}