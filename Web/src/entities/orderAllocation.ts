import Entity from './entity'


export default class orderAllocation extends Entity<number>{


    inventoryId: number;
    orderId: number;
    orderDetailId: number;
    customerId: number;
    customerName: string;
    warehouseId: number;
    warehouseName: string;
    area: string;
    location: string;
    sKU: string;
    uPC: string;
    goodsType: string;
    inventoryStatus: number;
    superId: number;
    relatedId: number;
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