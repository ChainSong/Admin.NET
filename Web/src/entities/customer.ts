import Entity from './entity'
import CustomerDetail from './customerDetail'

export default class customer extends Entity<number>{
    customerId: number;
    projectId: number;
    customerCode: number;
    customerName: string;
    description: string;
    customerType: string;
    customerStatus: number;
    creditLine: string="";
    province: string="";
    city: string="";
    address: string="";
    remark: string="";
    email: string="";
    phone: string="";
    lawperson: string="";
    postcode: string="";
    bank: string="";
    account: string="";
    taxid: string="";
    invoiceTitle: string="";
    fax: string="";
    website: string="";
    creator: string="";
    createTime: string="";
    endcreateTime: string="";
    creationTime: string="";
    endCreationTime: string="";
    details: Array<CustomerDetail>;
}