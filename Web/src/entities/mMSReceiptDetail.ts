import Entity from './entity'

// import AbpBase from "../../lib/abpbase";
export default class mMSReceiptDetail extends Entity<number>{
    receiptId: number;
    receiptNumber: string;
    purchaseOrderNumber: string;
    externReceiptNumber: string;
    supplierId: number;
    supplierName: string;
    warehouseId: number;
    warehouseName: string;
    lineNumber: string;
    sKU: string;
    uPC: string;
    goodsType: string;
    goodsName: string;
    boxCode: string;
    trayCode: string;
    batchCode: string;
    lotCode: string;
    poCode: string;
    weight: number;
    volume: number;
    expectedQty: number;
    receivedQty: number;
    receiptQty: number;
    unitCode
    onwer
    productionDate
    expirationDate: string;
    remark: string;
    creator: string;
    creationTime: string;
    updator: string;
    updateTime: string;
    str1: string;
    str2: string;
    str3: string;
    str4: string;
    str5: string;
    str6: string;
    str7: string;
    str8: string;
    str9: string;
    str10: string;
    str11: string;
    str12: string;
    str13: string;
    str14: string;
    str15: string;
    str16: string;
    str17: string;
    str18: string;
    str19: string;
    str20: string;
    dateTime1: string;
    dateTime2: string;
    dateTime3: string;
    dateTime4: string;
    dateTime5: string;
    int1: number;
    int2: number;
    int3: number;
    int4: number;
    int5: number;
}