import Entity from './entity'
export default class supplierUserMapping extends Entity<number>{
    userId:number;
    userName:string;
    supplierId:number;
    supplierName:string;
    status:number;
    creator:string;
    updator:string;
    updateTime:string;
    creationTime:string;
    
}