import Entity from './entity'
// import AbpBase from "../../lib/abpbase";
export default class selectListItem extends Entity<number>{
    customerId:number;
    warehouseId:number;
    areaId:number;
    input:string;
   
}