import Entity from './entity' 
// import AbpBase from "../../lib/abpbase";
export default class warehouseDetail extends Entity<number>{

    warehouseId: number;
    warehouseName: string;
    customerId: number;
    customerName: string;
    contact: string;
    tel: string;
    phone: string;
    email: string;
    createTime: string;
    creator: string;
}