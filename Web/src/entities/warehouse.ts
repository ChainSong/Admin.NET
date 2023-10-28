import Entity from './entity'
import TableColumnsDetails from './tableColumnsDetails'
// import AbpBase from "../../lib/abpbase";
export default class warehouse extends Entity<number>{
projectId:number;
warehouseName:string;
warehouseStatus:number;
warehouseType:string;
description:string;
company:string;
address:string;
province:string;
city:string;
contractor:string;
contractorAddress:string;
mobile:string;
phone:string;
fax:string;
email:string;
remark:string;
creator:string;
updator:string;
updateTime:string;
creationTime:string;
}