import Entity from './entity'

export default class tableColumnsDetails extends Entity<number>{
  
    codeInt: number;
    codeStr: string;
    name: string="";
    type: string="";
    color: string="";
    associated: string="";
    status: number=0;
    creator: string="";
    creationTime: string="";
    
}