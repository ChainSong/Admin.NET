var encode = require("./encoding.js")
// var app = getApp();
var jpPrinter = {
	createNew: function() {
		var jpPrinter = {};
		var data = [];

		//var bar = ["UPC-A", "UPC-E", "EAN13", "EAN8", "CODE39", "ITF", "CODABAR", "CODE93", "CODE128"];

		jpPrinter.name = "账单模式";

		jpPrinter.init = function() { //初始化打印机
			data.push(27)
			data.push(64)
		};

		/**
		 * @description 打印文本内容
		 * @param {int} align （对齐）: 0: 左对齐, 1: 居中, 2: 右对齐
		 * @param {int} bold（加粗）: 0: 不加粗, 1: 加粗
		 * @param {int} wsize（字体宽度）: 倍数计算, 范围[0, 7]
		 * @param {int} hsize（字体高度）: 倍数计算, 范围[0, 7]
		 * @param {int} reverse（反显）: 0: 不反显（黑字白底）, 1: 反显（黑底白字）
		 * @param {int} underline（下划线）: 0: 没下划线, 1: 有下划线
		 * @param {String} content（文本内容）
		 */
		jpPrinter.setText = function(align, bold, wsize, hsize, reverse, underline, content) {
			this.init()
			this.setSelectJustification(align)
			this.setSelectBold(bold)
			this.setSelectTextSize(wsize, hsize)
			this.setSelectReverse(reverse)
			this.setSelectUnderline(underline)
			this.setTextContent(content)
			this.setPrint()
			this.init()
		}

		//设置文本内容
		jpPrinter.setTextContent = function(content) { 
			var code = new encode.TextEncoder(
				'gb18030', {
					NONSTANDARD_allowLegacyEncoding: true
				}).encode(content)
			for (var i = 0; i < code.length; ++i) {
				data.push(code[i])
			}
		}

		//设置条码宽度
		jpPrinter.setBarcodeWidth = function(width) { 
			data.push(29)
			data.push(119)
			if (width > 6) {
				width = 6;
			}
			if (width < 2) {
				width = 1;
			}
			data.push(width)
		}

		/**
		 * @description 打印条码
		 * @param {int} align align （对齐）: 0: 左对齐, 1: 居中, 2: 右对齐
		 * @param {int} type（类型）: 1: UPC-A, 2: JAN13（EAN13）, 3: JAN8（EAN8）, 4: CODE39, 5: ITF, 6: CODABAR, 7:CODE128
		 * @param {int} width（宽度）: 点位计算,范围[2, 6]
		 * @param {int} height（高度）: 点位计算,范围[1, 255]
		 * @param {int} position（HRI字符位置）: 0: 不打印, 1: 条码上方, 2: 条码下方, 3: 条码上、下方都打印
		 * @param {String} content（条码内容）: 注意不同的类型有不同的内容限制，具体看佳博ESC编程手册 
		 */
		jpPrinter.setBar = function(align, type, width, height, position, content) {
			this.init()
			this.setSelectJustification(align)
			this.setBarcodeWidth(width)
			this.setBarcodeHeight(height)
			this.setBarcodePosition(position)
			this.setBarcodeContent(type, content)
			this.setPrint()
			this.init()
		}

		//设置条码高度
		jpPrinter.setBarcodeHeight = function(height) {
			data.push(29)
			data.push(104)
			data.push(height)
		}

		//HRI 字符的打印位置
		jpPrinter.setBarcodePosition = function(n) {
			/*
			0, 48 不打印
			1, 49 条码上方
			2, 50 条码下方 
			3, 51 条码上、下方都打印
			*/
			data.push(29)
			data.push(72)
			data.push(n)
		}

		//设置条码内容
		jpPrinter.setBarcodeContent = function(t, content) {
			// 1: UPC-A, 2: JAN13（EAN13）, 3: JAN8（EAN8）, 4: CODE39, 5: ITF, 6: CODABAR, 7:CODE128
			var code = new encode.TextEncoder(
				'gb18030', {
					NONSTANDARD_allowLegacyEncoding: true
				}).encode(content)
			var ty = 73;
			data.push(29)
			data.push(107)
			switch (t) {
				case 1:
					ty = 65;
					break;
				case 2:
					ty = 67;
					break;
				case 3:
					ty = 68;
					break;
				case 4:
					ty = 69;
					break;
				case 5:
					ty = 70;
					break;
				case 6:
					ty = 71;
					break;
				case 7:
					ty = 73;
					break;
			}
			data.push(ty)
			data.push(parseInt(code.length))
			for (var i = 0; i < code.length; ++i) {
				data.push(code[i])
			}
		}

		//设置二维码大小
		jpPrinter.setSelectSizeOfModuleForQRCode = function(n) {
			data.push(29)
			data.push(40)
			data.push(107)
			data.push(3)
			data.push(0)
			data.push(49)
			data.push(67)
			if (n > 15) {
				n = 15
			}
			if (n < 1) {
				n = 1
			}
			data.push(n)
		}

		//设置纠错等级
		jpPrinter.setSelectErrorCorrectionLevelForQRCode = function(t) {
			/*
			n      功能      纠错能力
			48 选择纠错等级 L 7
			49 选择纠错等级 M 15
			50 选择纠错等级 Q 25
			51 选择纠错等级 H 30
			*/
			data.push(29)
			data.push(40)
			data.push(107)
			data.push(3)
			data.push(0)
			data.push(49)
			data.push(69)
			var ty = 49
			switch (t) {
				case "L":
					ty = 48
					break
				case "M":
					ty = 49
					break
				case "Q":
					ty = 50
					break
				case "H":
					ty = 51
					break
			}
			data.push(ty)
		}

		//设置二维码内容
		jpPrinter.setStoreQRCodeData = function(content) {
			var code = new encode.TextEncoder(
				'gb18030', {
					NONSTANDARD_allowLegacyEncoding: true
				}).encode(content)
			data.push(29)
			data.push(40)
			data.push(107)
			data.push(parseInt((code.length + 3) % 256))
			data.push(parseInt((code.length + 3) / 256))
			data.push(49)
			data.push(80)
			data.push(48)

			for (var i = 0; i < code.length; ++i) {
				data.push(code[i])
			}
		}

		/**
		 * @description 打印二维码
		 * @param {int} align（对齐）: 0: 左对齐, 1: 居中, 2: 右对齐
		 * @param {int} size（尺寸）: 点位计算,范围[1, 16]
		 * @param {String} error（纠错等级）: L: 纠错等级L, M: 纠错等级M, Q: 纠错等级Q, H: 纠错等级H
		 * @param {String} content（二维码内容）: 注意不要超过最大字符数，超过无法打印出 
		 */
		jpPrinter.setQR = function(align, size, error, content) {
			this.init()
			this.setSelectJustification(align)
			this.setSelectSizeOfModuleForQRCode(size)
			this.setSelectErrorCorrectionLevelForQRCode(error)
			this.setStoreQRCodeData(content)
			this.setPrintQRCode()
			this.init()
		}

		//打印二维码
		jpPrinter.setPrintQRCode = function() {
			data.push(29)
			data.push(40)
			data.push(107)
			data.push(3)
			data.push(0)
			data.push(49)
			data.push(81)
			data.push(48)
		}

		//移动打印位置到下一个水平定位点的位置
		jpPrinter.setHorTab = function() {
			data.push(9)
		}

		//设置绝对打印位置
		jpPrinter.setAbsolutePrintPosition = function(where) {
			data.push(27)
			data.push(36)
			data.push(parseInt(where % 256))
			data.push(parseInt(where / 256))
		}

		//设置相对横向打印位置
		jpPrinter.setRelativePrintPositon = function(where) {
			data.push(27)
			data.push(92)
			data.push(parseInt(where % 256))
			data.push(parseInt(where / 256))
		}

		//对齐方式
		jpPrinter.setSelectJustification = function(which) {
			/*
			0, 48 左对齐
			1, 49 中间对齐
			2, 50 右对齐
			*/
			data.push(27)
			data.push(97)
			data.push(which)
		}

		//是否加粗
		jpPrinter.setSelectBold = function(which) {
			/*
			0 不加粗
			1 加粗
			*/
			data.push(27)
			data.push(69)
			data.push(which)
		}

		//字符大小
		jpPrinter.setSelectTextSize = function(wsize, hsize) {
			data.push(29)
			data.push(33)
			data.push(wsize*16+hsize)
		}

		//选择/取消黑白反显打印模式
		jpPrinter.setSelectReverse = function(which) {
			/*
			0: 不反显（黑字白底）, 1: 反显（黑底白字）
			*/
			data.push(29)
			data.push(66)
			data.push(which)
		}

		//选择/取消下划线模式
		jpPrinter.setSelectUnderline = function(which) {
			/*
			  0: 没下划线, 1: 有下划线
			*/
			data.push(27)
			data.push(45)
			data.push(which)
		}

		//设置左边距
		jpPrinter.setLeftMargin = function(n) {
			data.push(29)
			data.push(76)
			data.push(parseInt(n % 256))
			data.push(parseInt(n / 256))
		}

		//设置打印区域宽度
		jpPrinter.setPrintingAreaWidth = function(width) { 
			data.push(29)
			data.push(87)
			data.push(parseInt(width % 256))
			data.push(parseInt(width / 256))
		}

		//设置蜂鸣器
		jpPrinter.setSound = function(n, t) {
			data.push(27)
			data.push(66)
			if (n < 0) {
				n = 1;
			} else if (n > 9) {
				n = 9;
			}

			if (t < 0) {
				t = 1;
			} else if (t > 9) {
				t = 9;
			}
			data.push(n)
			data.push(t)
		}

		//参数，画布的参数
		jpPrinter.setBitmap = function(res) { 
			var height = res.height;
			var imgWidth = res.width
			var pointList = []
			var resultData = []
			data.push(29)
			data.push(118)
			data.push(48)
			data.push(0)
			data.push((parseInt((res.width + 7) / 8) * 8) / 8)
			data.push(0)
			data.push(parseInt(res.height % 256))
			data.push(parseInt(res.height / 256))
			//console.log(data)
			//console.log("temp=" + temp)
			for (var y = 0; y < height; y++) {
				for (var x = 0; x < imgWidth; x++) {
					let r = res.data[(y * imgWidth + x) * 4];
					let g = res.data[(y * imgWidth + x) * 4 + 1];
					let b = res.data[(y * imgWidth + x) * 4 + 2];
					let a = res.data[(y * imgWidth + x) * 4 + 3]
					//console.log(`当前${y}行${x}列像素,rgba值:(${r},${g},${b},${a})`)
					// 像素灰度值
					let grayColor = r * 0.299 + g * 0.587 + b * 0.114
					//灰度值大于128位 
					if (grayColor > 128) {
						pointList.push(0)
					} else {
						pointList.push(1)
					}
				}
			}
			for (var i = 0; i < pointList.length; i += 8) {
				var p = pointList[i] * 128 + pointList[i + 1] * 64 + pointList[i + 2] * 32 + pointList[i +
						3] * 16 + pointList[i + 4] * 8 + pointList[i + 5] * 4 + pointList[i + 6] * 2 +
					pointList[i + 7]
				resultData.push(p)
			}
			for (var i = 0; i < resultData.length; ++i) {
				data.push(intToByte(resultData[i]))
			}
		}

		//打印并换行
		jpPrinter.setPrint = function() {
			data.push(10)
		}

		//打印并走纸feed个单位
		jpPrinter.setPrintAndFeed = function(feed) {
			data.push(27)
			data.push(74)
			data.push(feed)
		}

		//打印并走纸row行
		jpPrinter.setPrintAndFeedRow = function(row) {
			data.push(27)
			data.push(100)
			data.push(row)
		}

		//获取打印数据
		jpPrinter.getData = function() { 
			return data;
		};
		
		function intToByte(i) {
			// 此处关键 -- android是java平台 byte数值范围是 [-128, 127]
			// 因为java平台的byte类型是有符号的 最高位表示符号，所以数值范围固定
			// 而图片计算出来的是数值是 0 -255 属于int类型
			// 所以把int 转换成byte类型 
			//#ifdef APP-PLUS
				var b = i & 0xFF;
				var c = 0;
				if (b >= 128) {
					c = b % 128;
					c = -1 * (128 - c);
				} else {
					c = b;
				}
				return c
			//#endif
			// 而微信小程序不需要，因为小程序api接收的是 无符号8位
			//#ifdef MP-WEIXIN
				return i
			//#endif
		}
		
		return jpPrinter;
	},

	Query: function() {
		var queryStatus = {};
		var buf;
		var dateView;
		//查询打印机实时状态
		queryStatus.getRealtimeStatusTransmission = function(n) {
			/*
			n = 1：传送打印机状态
			n = 2：传送脱机状态
			n = 3：传送错误状态
			n = 4：传送纸传感器状态
			*/
			buf = new ArrayBuffer(3)
			dateView = new DataView(buf)
			dateView.setUint8(0, 16)
			dateView.setUint8(1, 4)
			dateView.setUint8(2, n)
			queryStatus.query(buf)
		}

		queryStatus.query = function(buf) {
			wx.writeBLECharacteristicValue({
				deviceId: app.BLEInformation.deviceId,
				serviceId: app.BLEInformation.writeServiceId,
				characteristicId: app.BLEInformation.writeCharaterId,
				value: buf,
				success: function(res) {

				},
				complete: function(res) {
					//console.log(res)
					buf = null
					dateView = null;
				}
			})
		}
		
		return queryStatus;
	}

};

module.exports.jpPrinter = jpPrinter;
