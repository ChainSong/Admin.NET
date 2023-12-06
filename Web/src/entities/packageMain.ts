import Entity from './entity'
import packageDetail from './packageDetail'

export default class packageMain extends Entity<number>{

    orderId: number;
    packageNumber: string;
    orderNumber: string;
    externOrderNumber: string;
    customerId: number;
    customerName: string;
    warehouseId: number;
    warehouseName: string;
    packageType: string;
    length: number;
    width: number;
    height: number;
    netWeight: number;
    grossWeight: number;
    expressCompany: string;
    expressNumber: string;
    isComposited: number;
    isHandovered: number;
    handoveror: string;
    handoverTime: string;
    packageStatus: number;
    packageTime: string;
    detailCount: number;
    creator: string;
    creationTime: string;
    updator: string;
    updateTime: string;
    remark: string;
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
    details: Array<packageDetail>;
}