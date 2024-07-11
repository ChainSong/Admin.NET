import Entity from './entity'

export default class customerConfig extends Entity<number>{
    customerId: number=0; 
    customerCode: string="";
    customerName: string="";
    customerLogo: string="";
    creator: string="";
    creationTime: string="";

}