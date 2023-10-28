import Entity from './entity'
import receiptDetail from './receiptDetail'
import receiptReceiving from './receiptReceiving'

export default class receipt extends Entity<number>{
    aSNId: Number;
    aSNNumber: string;
    receiptNumber: string;
    externReceiptNumber: string;
    customerId: Number;
    customerName: string;
    warehouseId: Number;
    warehouseName: string;
    receiptTime: string;
    receiptStatus: Number;
    receiptType: string;
    contact: string;
    contactInfo: string;
    completeTime: string;
    remark: string;
    creator: string;
    creationTime: string;
    updator: string;
    updateTime: string;
    str1			  :string;
    str2			  :string;
    str3			  :string;
    str4			  :string;
    str5			  :string;
    str6			  :string;
    str7			  :string;
    str8			  :string;
    str9			  :string;
    str10			  :string;
    str11			  :string;
    str12			  :string;
    str13			  :string;
    str14			  :string;
    str15			  :string;
    str16			  :string;
    str17			  :string;
    str18			  :string;
    str19			  :string;
    str20			  :string;
    dateTime1		  :string;
    dateTime2		  :string;
    dateTime3		  :string;
    dateTime4		  :string;
    dateTime5		  :string;
    int1			  :number;
    int2			  :number;
    int3			  :number;
    int4			  :number;
    int5			  :number;
    receiptDetails:Array<receiptDetail>;
    receiptReceivings:Array<receiptReceiving>;
}