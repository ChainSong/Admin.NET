import Entity from './entity'

export default class customerDetail extends Entity<number>{
    customerId: number=0;
    projectId: number=0;
    customerCode: string="";
    customerName: string="";
    contact: string="";
    tel: string="";
    creator: string="";
    creationTime: string="";

}