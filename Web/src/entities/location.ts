import Entity from './entity'
import TableColumnsDetails from './tableColumnsDetails'
// import AbpBase from "../../lib/abpbase";
export default class warehouse extends Entity<number>{
    warehouseId:number;
    warehouseName:string;
    areaId:number;
    areaName:string;
    location:string;
    locationStatus:number;
    locationType:string;
    classification:string;
    category:string;
    aBCClassification:string;
    isMultiLot:string;
    isMultiSKU:string;
    locationLevel:string;
    goodsPutOrder:string;
    goodsPickOrder:string;
    sku:string;
    creator:string;
    creationTime:string;
    updator:string;
    updateTime:string;
}