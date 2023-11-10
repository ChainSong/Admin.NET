import Entity from './entity'
import TableColumnsDetails from './tableColumnsDetails'
// import AbpBase from "../../lib/abpbase";
export default class pickTask extends Entity<number>{
    
customerId: number;
customerName: string;
warehouseId: number;
warehouseName: string;
pickTaskNumber: string;
pickStatus: Number;
pickType: string;
startTime: string;
endTime: string;
printNum: number;
printTime: string;
printPersonnel: string;
pickPlanPersonnel: string;
detailQty: number;
detailKindsQty: number;
pickContainer: string;
creator: string;
creationTime: string;
updator: string;
updateTime: string;
str1: string;
str2: string;
str3: string;
str4: string;
str5: string;
str6: string;
str7: string;
str8: string;
str9: string;
str10: string;
dateTime1: string;
dateTime2: string;
dateTime3: string;
dateTime4: string;
dateTime5: string;
int1: number;
int2: number;
int3: number;
int4: number;
int5: number;
pickTaskDetails: Array<pickTaskDetail>;
}