//热敏打印

var SSPrint = uni.requireNativePlugin("wyj-ssprint_SSPrintUniModule");
var handle = 0;
const modal = uni.requireNativePlugin('modal');

var CacheSSPrint = {
	ip_address: "", //打印机ip
	port_number: "", //打印机端口
	encoding: "", //打印机编码
	character_set: "", //字符集
	code_table: "", //码表文件
	character_library: "", //字库文件
	keyValue: "", //热敏打印机key
	type: 1 //打印机类型
};

//打印相关的参数
let SSPrintPlugin  = {
	ip_address: "", //打印机ip
	port_number: "", //打印机端口
	encoding: "", //打印机编码
	character_set: "", //字符集
	code_table: "", //码表文件
	character_library: "", //字库文件
	keyValue: "", //热敏打印机key
	type: 1 //打印机类型
};

//打开热敏打印机端口
function open_printer_port() {
	//端口未打开
	if (!SSPrint.Port_IsOpened(handle)) {
		open_port_if_not_opened();
	}
}
//关闭热敏打印机端口
function close_printer_port() {
	if (SSPrint.Port_IsOpened(handle)) {
		SSPrint.Port_Close(handle);
	}
}

//判断热敏打印机是否已打开  如果没有打开则打开端口
function open_port_if_not_opened() {
	//如果有端口未关闭，则直接使用之前未关闭的端口。
	if (!SSPrint.Port_IsOpened(handle)) {
		var handleList = SSPrint.Port_GetPortHandleList();
		for (let i = 0; i < handleList.length; i++) {
			if (SSPrint.Port_IsOpened(handleList[i])) {
				handle = handleList[i];
				SSPrint.Port_Close(handle);
			}
		}
	}

	//如果端口未打开，则打开端口。
	if (!SSPrint.Port_IsOpened(handle)) {
		/*modal.toast({
			message: "正在打开端口...",
			duration: 1.5
		});
		*/
		handle = SSPrint.Port_OpenTcp(SSPrintPlugin.ip_address, SSPrintPlugin.port_number, null, 0, 10000);
		if (SSPrint.Port_IsOpened(handle)) {
			var portInfo = SSPrint.Port_GetPortInfoMap(handle);
			/*modal.toast({
				message: portInfo,
				duration: 1.5
			});*/
		} else {
			uni.showToast({
				title: "打印机端口超时！",
				icon: 'error'
			});
		}
	}
}


function isVariableEmpty(variable) {
	return variable === null || variable === undefined || variable === '';
}
//获取打印参数并连接打印机
function printingParameters(print) {
	try {
		if (!isVariableEmpty(print.ip_address)&&!isVariableEmpty(print.port_number)&&!isVariableEmpty(print.character_set)&&!isVariableEmpty(print.code_table)&&!isVariableEmpty(print.character_library)) {
			if(!isVariableEmpty(CacheSSPrint.ip_address)){
				if(CacheSSPrint.ip_address!=print.ip_address&&CacheSSPrint.port_number!=print.port_number){
					close_printer_port();
				}
			}
			
			CacheSSPrint.ip_address= print.ip_address;
			CacheSSPrint.port_number = print.port_number;
			CacheSSPrint.encoding = print.encoding;
			CacheSSPrint.character_set = print.character_set;
			CacheSSPrint.code_table = print.code_table;
			CacheSSPrint.character_library = print.character_library;
			CacheSSPrint.keyValue = print.keyValue;
			CacheSSPrint.type = print.type;
			
			SSPrintPlugin.ip_address= print.ip_address;
			SSPrintPlugin.port_number = print.port_number;
			SSPrintPlugin.encoding = print.encoding;
			SSPrintPlugin.character_set = print.character_set;
			SSPrintPlugin.code_table = print.code_table;
			SSPrintPlugin.character_library = print.character_library;
			SSPrintPlugin.keyValue = print.keyValue;
			SSPrintPlugin.type = print.type;
			console.log("2");
			open_port_if_not_opened(); //判断热敏打印机是否已打开  如果没有打开则打开端口
			console.log("3");
			if (!SSPrint.Port_IsOpened(handle)) {
				uni.showModal({
				    title: '错误提示',
				    content: '访问热敏打印机异常1！',
					showCancel: false
				});
				return false;
			} else {
				return true;
			}
		} else {
			uni.showModal({
			    title: '错误提示',
			    content: '访问热敏打印机异常2！',
				showCancel: false
			});
			return false;
		}
	} catch {
		uni.showModal({
		    title: '错误提示',
		    content: '访问热敏打印机异常3！',
			showCancel: false
		});
		return false;
	}
}

//打印lpn
export function drawAndExportLpn(print, data) {
	try {
		if (printingParameters(print)) {
			let leftDistance = 0;
			var printStr = "^XA ";
			//printStr += "^FO0,0 ^GB576,240,3^FS";   //70mm x100mm的热敏纸 实际尺寸约为  551，787
			//printStr += "^FO20,10 ^GB538,770,3^FS";  
			if (data == null || data == "" || data.length >= 20) {
				leftDistance = 115;
			} else {
				leftDistance = 115 + (20 - data.length) * 11;
			}
			printStr += "^FO 210," + leftDistance + ", ^BCR,150,Y,N,N ^FD" + data + "^FS ";
			printStr += "^XZ";
			console.log(printStr);
			let flag = SSPrint.Port_WriteStringUsingUTF8Encoding(handle, printStr);
			if(flag>0){
				return true;
			}else{
				uni.showToast({
					title: '打印失败',
					icon: 'none'
				});
				return false;
			}
		} else {
			uni.showToast({
				title: '打印失败',
				icon: 'none'
			});
			return false;
		}
	} catch {
		uni.showModal({
		    title: '错误提示',
		    content: '访问热敏打印机异常4！',
			showCancel: false
		});
		return false;
	}
}

//打印发运单
export function drawAndExportShipOrder(print, data) {
	console.log(print);
	console.log(data);
	try {
		if (printingParameters(print)) {
			let leftDistance = 0;
			var printStr = "^XA ";
			printStr += " ^FWR "; //文字顺时针翻转90度
			printStr += " ^SEE:" + print.code_table + "^FS"; //码表文件
			printStr += " ^CWJ," + print.character_library + "^FS"; //字库文件
			printStr += " ^FO20,10 ^GB537,770,3 ^FS" ; //画方框
			printStr += " ^FO482,10 ^GB1,770,2 ^FS "; //画第一个横线
			printStr += " ^FO341,10 ^GB1,770,2 ^FS "; //画第二个横线
			printStr += " ^FO288,10 ^GB1,770,2 ^FS "; //画第三个横线
			printStr += " ^FO235,10 ^GB1,770,2 ^FS"; //画第四个横线
			printStr += " ^FO94,10 ^GB1,770,2 ^FS "; //画第五个横线
			
			printStr += " ^FO20,97 ^GB537,2,2 ^FS"; //画第一个竖线
			printStr += " ^FO482,420 ^GB74,2,2 ^FS "; //画第一行第二个竖线
			printStr += " ^FO482,517 ^GB74,2,2 ^FS "; //画第一行第三个竖线
			printStr += " ^FO20,200 ^GB74,2,2 ^FS"; //画第七行第二个竖线
			printStr += " ^FO20,294 ^GB74,2,2 ^FS "; //画第七行第三个竖线
			printStr += " ^FO20,562 ^GB74,2,2 ^FS"; //画第七行第四个竖线
			printStr += " ^FO20,624 ^GB74,2,2 ^FS"; // 画第七行第五个竖线
			printStr += " ^FO525,22  ^A0,20,20 ^FD BFCES ^FS";
			printStr += " ^FO495,11  ^AJ,20,20 ^" + print.character_set + "^FD 物料编码 ^FS";
			//物料编码部分左距离
			if (data.materialCode == null || data.materialCode == "" || data.materialCode.length >= 15) {
				leftDistance = 100;
			} else {
				leftDistance = 100 + (15 - data.materialCode.length) * 10;
			}
			printStr += " ^FO495," + leftDistance + " ^A0,40,40 ^FD " + data.materialCode + " ^FS"; //物料号
			printStr += " ^FO525,442  ^A0,20,20 ^FD BFCES ^FS";
			printStr += " ^FO495,426 ^AJ,20,20 ^" + print.character_set + "^FD 物料名称 ^FS";
			//物料名部分左距离
			if (data.materialName == null || data.materialName == "" || data.materialName.length >= 10) {
				leftDistance = 518;
			} else {
				leftDistance = 518 + (10 - data.materialName.length) * 12;
			}
			printStr += " ^FO508," + leftDistance + " ^AJ,24,24 ^" + print.character_set + "^FD " + data.materialName +" ^FS"; //物料名称

            printStr += " ^FO400,11  ^AJ,20,20 ^CI26^FD 物料条码 ^FS";
			//物料编码条码部分左距离
			if (data.materialCode == null || data.materialCode == "" || data.materialCode.length >= 15) {
				leftDistance = 218;
			} else {
				leftDistance = 218 + (15 - data.materialCode.length) * 10;
			}
			printStr += " ^FO390," + leftDistance + " ^BCR,50,Y,N,N ^FD"+ data.materialCode +"^FS"; //物料号条码
			printStr += " ^FO315,30  ^AJ,20,20 ^" + print.character_set + "^FD 客户 ^FS";
			printStr += " ^FO292,11  ^AJ,20,20 ^" + print.character_set + "^FD 物料编码 ^FS";

			//客户物料编码部分左距离
			if (data.customerMaterialCode == null || data.customerMaterialCode == "" || data.customerMaterialCode
				.length >= 15) {
				leftDistance = 284;
			} else {
				leftDistance = 284 + (15 - data.customerMaterialCode.length) * 10;
			}

			printStr += " ^FO291," + leftDistance + " ^A0,40,40 ^FD " + data.customerMaterialCode + " ^FS"; //客户物料编码

/*
			//客户物料编码条码部分左距离
			if (data.customerMaterialCode == null || data.customerMaterialCode == "" || data.customerMaterialCode
				.length >= 15) {
				leftDistance = 238;
			} else {
				leftDistance = 238 + (15 - data.customerMaterialCode.length) * 10;
			}
			printStr += " ^FO275," + leftDistance + " ^BCR,50,Y,N,N ^FD " + data.customerMaterialCode +
			"  ^FS"; //客户物料编码条码
			
			*/
			printStr += "^FO251,16  ^AJ,20,20 ^" + print.character_set + "^FD LPN号 ^FS";

			//lpn部分左距离
			if (data.lpn == null || data.lpn == "" || data.lpn.length >= 15) {
				leftDistance = 284;
			} else {
				leftDistance = 284 + (15 - data.lpn.length) * 10;
			}
			printStr += " ^FO239," + leftDistance + " ^A0,40,40 ^FD " + data.lpn + " ^FS"; //lpn
            printStr += " ^FO160,12  ^AJ,20,20 ^CI26^FD LPN条码 ^FS "; //lpn
			//lpn条码部分左距离
			if (data.lpn == null || data.lpn == "" || data.lpn.length >= 15) {
				leftDistance = 218;
			} else {
				leftDistance = 218 + (15 - data.lpn.length) * 10;
			}
			printStr += " ^FO150," + leftDistance + " ^BCR,50,Y,N,N ^FD" + data.lpn +"^FS"; //lpn 条码
			printStr += " ^FO45,26  ^AJ,20,20 ^" + print.character_set + "^FD 数量 ^FS";

			//数量左距离
			if (data.qty == null || data.qty == "" || data.qty.length >= 7) {
				leftDistance = 100;
			} else {
				leftDistance = 100 + (7 - data.qty.length) * 6;
			}
			printStr += " ^FO45,"+leftDistance+" ^A0,22,22 ^FD " + data.qty + " ^FS"; //  数量
			//printStr += " ^FO36,206  ^AJ,20,20 ^" + print.character_set + "^FD 数量条码 ^FS";
			printStr += " ^FO45,220  ^AJ,20,20 ^" + print.character_set + "^FD 来源 ^FS";

			//数量条码左距离
			//if (data.qty == null || data.qty == "" || data.qty.length >= 7) {
			//	leftDistance = 310;
			//} else {
			//	leftDistance = 310 + (7 - data.qty.length) * 10;
			//}
			//printStr += "^FO32," + leftDistance + " ^BCR,50,Y,N,N ^FD " + data.qty + " ^FS";
			printStr += " ^FO45,570  ^AJ,20,20 ^" + print.character_set + "^FD 时间 ^FS";
			printStr += " ^FO45,631  ^" + print.character_set + "^AJ,20,20 ^FD " + data.time + " ^FS";
			printStr += "^XZ";
			//console.log(printStr);
			let flag = SSPrint.Port_WriteStringUsingGBKEncoding(handle,printStr);
			if(flag>0){
				return true;
			}else{
				uni.showToast({
					title: '打印失败',
					icon: 'none'
				});
				return false;
			}
		} else {
			uni.showToast({
				title: '打印失败',
				icon: 'none'
			});
			return false;
		}
	} catch {
		uni.showModal({
		    title: '错误提示',
		    content: '访问热敏打印机异常5！',
			showCancel: false
		});
		return false;
	}
}