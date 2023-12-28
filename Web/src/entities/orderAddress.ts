import Entity from './entity' 
 

export default class orderAddress extends Entity<number>{
    preOrderId:number;
    preOrderNumber:string;
    externReceiptNumber:string;
    name:string;
    companyName:string;
    addressTag:string;
    phone:string;
    zipCode:string;
    province:string;
    city:string;
    country:string;
    address:string;
    expressCompany:string;
    expressNumber:string;

}