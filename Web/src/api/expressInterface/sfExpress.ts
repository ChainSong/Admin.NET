
import { ref, onMounted, nextTick } from "vue";
import request from '/@/utils/request';
import { getExpressConfig, allExpress } from '/@/api/main/wMSExpressConfig';

const expressConfig = ref({});
// --------------------顺丰快递打印-----------------------
// 寮曞叆SDK鍚庡垵濮嬪寲瀹炰緥锛屼粎鎵ц涓€娆�
const sdkCallback = result => { };
let sdkParams = {
  env: expressConfig.value.env, // 鐢熶骇锛歱ro锛涙矙绠憋細sbox銆備笉浼犻粯璁ょ敓浜э紝杞敓浜ч渶瑕佷慨鏀硅繖閲�
  partnerID: expressConfig.value.partnerId,
  callback: sdkCallback,
  notips: true
};
let printSdk = new SCPPrint(sdkParams);



// 鑾峰彇鎵撳嵃鏈哄垪琛�
const getPrintersCallback = result => {
    if (result.code === 1) {
      const printers = result.printers;
  
      const selectElement = document.getElementById("printers");
  
      // 娓叉煋鎵撳嵃鏈洪€夋嫨妗嗕笅鎷夊€�
      for (let i = 0; i < printers.length; i++) {
        const item = printers[i];
        var option = document.createElement("option");
        option.innerHTML = item.name;
        option.value = item.index;
        selectElement.appendChild(option);
      }
  
      // 璁剧疆榛樿鎵撳嵃鏈�
      var printer = 0;
      selectElement.value = printer;
      // console.log("printerprinter")
      // console.log("printerprinter")
      // console.log("printerprinter")
      // console.log(printer)
      printSdk.setPrinter(printer);
    }
  };
  printSdk.getPrinters(getPrintersCallback);
  
  // 閫夋嫨鎵撳嵃鏈�
  const selectPrinter = (e) => {
    // 璁剧疆鎵撳嵃鏈�
    printSdk.setPrinter(e.target.value);
  }
  export const getExpress= async(express:any)=>{
    // let res = await printExpressData(row);
    // alert(res.data.result.data.expressNumber);
    // console.log(expressConfig.value);
   
    let resToken = await getExpressConfig(express);
    // console.log("resToken")
    if (resToken.data.result.code == 1) {
      // console.log(resToken)
      expressConfig.value = resToken.data.result.data;
    }
  sdkParams.env = expressConfig.value.env;// 鐢熶骇锛歱ro锛涙矙绠憋細sbox銆備笉浼犻粯璁ょ敓浜э紝杞敓浜ч渶瑕佷慨鏀硅繖閲�
    sdkParams.partnerID = expressConfig.value.partnerId;
    sdkParams.callback = sdkCallback;
    sdkParams.notips = true;
    printSdk = new SCPPrint(sdkParams);

  }
  // 鎵撳嵃
  export const print = async (express:any) => {
    //获取token；
   await  getExpress(express);
    // console.log(result);
    const data = {
      requestID: expressConfig.value.partnerId,
      accessToken: expressConfig.value.token,
      templateCode: expressConfig.value.templateCode,
      templateVersion: "",
      documents: [
        {
          masterWaybillNo: express.expressNumber
        }
      ],
      extJson: {},
      customTemplateCode: ""
    };
    const callback = function (result) { };
    const options = {
      lodopFn: "PRINT" // 榛樿鎵撳嵃锛岄瑙堜紶PREVIEW
    };
    console.log(printSdk);
    console.log(printSdk.print(data, callback, options));
  };
  
  // 导出   实例
export default {print,getExpress};