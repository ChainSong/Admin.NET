import Entity from './entity'
export default class warehouseUserMapping extends Entity<number>{
    userId:number;
    userName:string;
    warehouseId:number;
    warehouseName:string;
    status:number;
    creator:string;
    updator:string;
    updateTime:string;
    creationTime:string;
    
}