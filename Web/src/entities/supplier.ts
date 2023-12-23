import Entity from './entity' 

export default class supplier extends Entity<number>{
    supplierId: number;
    projectId: number;
    supplierCode:  string="";
    supplierName:  string="";
    description:  string="";
    customerType:  string="";
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
}