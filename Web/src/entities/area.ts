import Entity from './entity'
import TableColumnsDetails from './tableColumnsDetails'
// import AbpBase from "../../lib/abpbase";
export default class area extends Entity<number>{
    warehouseId:number;
    warehouseName:string;
    areaName:string;
    areaStatus:number;
    areaType:string;
    remark:string;
    creator:string;
    creationTime:string;
    updator:string;
    updateTime:string;
}