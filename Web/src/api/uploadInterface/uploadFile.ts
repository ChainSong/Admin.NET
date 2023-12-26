import { Local, Session } from '/@/utils/storage';
// 主体路径
let baseURL = import.meta.env.VITE_API_URL;
//给上传组件赋值url
let uploadURL = baseURL;
//+ '/api/mMSReceiptReceivingReceiving/UploadExcelFile';
//给上传组件赋值token
// 获取本地的 token
const accessTokenKey = 'access-token';
const accessToken = Local.get(accessTokenKey);
let httpheaders = { Authorization: "Bearer " + accessToken }

  // 导出   实例
  export default {httpheaders,uploadURL};