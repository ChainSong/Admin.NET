// var app = getApp();
var encode = require("./encoding.js");
var jpPrinter = {
	createNew: function() {
		var jpPrinter = {};
		var data = "";
		var command = []

		jpPrinter.name = "标签模式";

		jpPrinter.init = function() {};

		//将指令转成数组装起
		jpPrinter.addCommand = function(content) {
			var code = new encode.TextEncoder(
				'gb18030', {
					NONSTANDARD_allowLegacyEncoding: true
				}).encode(content)
			//console.log('addCommand content ', content)
			// console.log('addCommand code ', code)
			for (var i = 0; i < code.length; ++i) {
				command.push(code[i])
			}
		}

		/**
		 * @description 设置页面大小
		 * @param {int} pageWidght 标签纸宽度，单位mm
		 * @param {int} pageHeight 标签纸高度，单位mm
		 */
		jpPrinter.setSize = function(pageWidght, pageHeight) {
			data = "SIZE " + pageWidght.toString() + " mm" + "," + pageHeight.toString() + " mm" + "\r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 设置打印速度
		 * @param {int} printSpeed 速度值，1-6
		 */
		jpPrinter.setSpeed = function(printSpeed) {
			// data = "SPEED " + printSpeed.toString() + "\r\n";
			data = `SPEED ${printSpeed}\r\n`
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 设置打印浓度
		 * @param {int} printDensity 速度值，0-15
		 */
		jpPrinter.setDensity = function(printDensity) {
			data = "DENSITY " + printDensity.toString() + "\r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 设置纸间间隙，单位mm
		 * @param {int} printGap 间隙值
		 */
		jpPrinter.setGap = function(printGap) {
			data = "GAP " + printGap.toString() + " mm\r\n";
			jpPrinter.addCommand(data)
		};
		 
		
		jpPrinter.setGapMM = function(m, n) { 
			data = `GAP ${m} mm,${n} mm\r\n`
			jpPrinter.addCommand(data)
		};

		//选择国际字符集
		jpPrinter.setCountry = function(country) {
			/*
			001:USA
			002:French
			003:Latin America
			034:Spanish
			039:Italian
			044:United Kingdom
			046:Swedish
			047:Norwegian
			049:German
			 */
			data = "COUNTRY " + country + "\r\n";
			jpPrinter.addCommand(data)
		};

		//选择国际代码页
		jpPrinter.setCodepage = function(codepage) {
			/*
			8-bit codepage 字符集代表
			437:United States
			850:Multilingual
			852:Slavic
			860:Portuguese
			863:Canadian/French
			865:Nordic
			Windows code page
			1250:Central Europe
			1252:Latin I
			1253:Greek
			1254:Turkish
			以下代码页仅限于 12×24 dot 英数字体
			WestEurope:WestEurope
			Greek:Greek
			Hebrew:Hebrew
			EastEurope:EastEurope
			Iran:Iran
			IranII:IranII
			Latvian:Latvian
			Arabic:Arabic
			Vietnam:Vietnam
			Uygur:Uygur
			Thai:Thai
			1252:Latin I
			1257:WPC1257
			1251:WPC1251
			866:Cyrillic
			858:PC858
			747:PC747
			864:PC864
			1001:PC100
			*/
			data = "CODEPAGE " + codepage + "\r\n";
			jpPrinter.addCommand(data)
		}

		//清除打印机缓存
		jpPrinter.setCls = function() {
			data = "CLS" + "\r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 将纸向前推出n行
		 * @param {int} feed 行数
		 */
		jpPrinter.setFeed = function(feed) {
			data = "FEED " + feed + "\r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 将纸向后回拉n行
		 * @param {int} backup 行数
		 */
		jpPrinter.setBackFeed = function(backup) {
			data = "BACKFEED " + backup + "\r\n";
			jpPrinter.addCommand(data)
		}

		/**
		 * @description 设置打印方向
		 * @param {int} direction 方向（0-出纸头部，1-出纸尾部）
		 */
		jpPrinter.setDirection = function(direction) {
			data = "DIRECTION " + direction + "\r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 设置开始打印的坐标原点，与打印方向有关
		 * @param {int} x x轴坐标值
		 * @param {int} y x轴坐标值
		 */
		jpPrinter.setReference = function(x, y) {
			data = "REFERENCE " + x + "," + y + "\r\n";
			jpPrinter.addCommand(data)
		};

		//根据Size进一张标签纸
		jpPrinter.setFromfeed = function() {
			data = "FORMFEED \r\n";
			jpPrinter.addCommand(data)
		};

		//根据Size找到下一张标签纸的位置
		jpPrinter.setHome = function() {
			data = "HOME \r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 控制蜂鸣器
		 * @param {int} level 音阶（0-9）
		 * @param {int} interval 声音长短(毫秒)
		 */
		jpPrinter.setSound = function(level, interval) {
			data = "SOUND " + level + "," + interval + "\r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 检测垂直间距,该指令用于设定打印机进纸时，若经过所设定的长度仍无法侦测到垂直间距，则打印机在连续纸模式工作
		 * @param {int} limit 间距距离（毫米）
		 */
		jpPrinter.setLimitfeed = function(limit) {
			data = "LIMITFEED " + limit + "\r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 绘制线条
		 * @param {int} x 起始x轴坐标
		 * @param {int} y 起始y轴坐标
		 * @param {int} width 线条长度 dot
		 * @param {int} height 线条高度 dot
		 */
		jpPrinter.setBar = function(x, y, width, height) {
			data = "BAR " + x + "," + y + "," + width + "," + height + "\r\n"
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 绘制方框
		 * @param {int} x_start 方框左上角x轴坐标
		 * @param {int} y_start 方框左上角y轴坐标
		 * @param {int} x_end  方框右下角x轴坐标
		 * @param {int} y_end  方框右下角y轴坐标
		 * @param {int} thickness 方框线宽 dot
		 */
		jpPrinter.setBox = function(x_start, y_start, x_end, y_end, thickness) {
			data = "BOX " + x_start + "," + y_start + "," + x_end + "," + y_end + "," + thickness + "\r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 清楚指定区域的数据
		 * @param {int} x_start 左上角x轴坐标
		 * @param {int} y_start 左上角y轴坐标
		 * @param {int} x_width 宽度 dot
		 * @param {int} y_height 高度 dot
		 */
		jpPrinter.setErase = function(x_start, y_start, x_width, y_height) {
			data = "ERASE " + x_start + "," + y_start + "," + x_width + "," + y_height + "\r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 将指定的区域反相打印(白色变黑色，黑色变白色)
		 * @param {int} x_start 左上角x轴坐标
		 * @param {int} y_start 左上角y轴坐标
		 * @param {int} x_width  宽度 dot
		 * @param {int} y_height 高度 dot
		 */
		jpPrinter.setReverse = function(x_start, y_start, x_width, y_height) {
			data = "REVERSE " + x_start + "," + y_start + "," + x_width + "," + y_height + "\r\n";
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 打印文字
		 * @param {int} x 文字 X 方向起始点坐标
		 * @param {int} y 文字 Y 方向起始点坐标
		 * @param {String} font 字体名称
		 * @param {int} x_ X 方向放大倍率 1-10
		 * @param {int} y_ Y 方向放大倍率 1-10
		 * @param {String} str 文字内容
		 */
		jpPrinter.setText = function(x, y, font, x_, y_, str) { //打印文字
			data = "TEXT " + x + "," + y + ",\"" + font + "\"," + 0 + "," + x_ + "," + y_ + "," + "\"" +
				str + "\"\r\n"
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 打印二维码
		 * @param {int} x 二维码水平方向起始点坐标
		 * @param {int} y 二维码垂直方向起始点坐标
		 * @param {int} level 选择 QRCODE 纠错等级
		 * @param {int} width 二维码宽度 1-10
		 * @param {String} mode 手动 A /自动编码 M
		 * @param {String} content 二维码内容
		 */
		jpPrinter.setQR = function(x, y, level, width, mode, content) { //打印二维码
			data = "QRCODE " + x + "," + y + "," + level + "," + width + "," + mode + "," + 0 + ",\"" +
				content + "\"\r\n"
			jpPrinter.addCommand(data)
		};

		/**
		 * @description 画一维条码
		 * @param {int} x 左上角水平坐标起点，以点（dot）表示
		 * @param {int} y 左上角垂直坐标起点，以点（dot）表示
		 * @param {int} codetype 一维条码种类 详情看佳博TSPL编程手册
		 * @param {int} height 条形码高度，以点（dot）表示
		 * @param {int} readable 0 表示人眼不可识，1 表示人眼可识
		 * @param {int} narrow 窄 bar 宽度，以点（dot）表示 
		 * @param {int} wide 宽 bar 宽度，以点（dot）表示
		 * @param {String} content 条码内容
		 */
		jpPrinter.setBar = function(x, y, codetype, height, readable, narrow, wide, content) { //打印条形码
			data = "BARCODE " + x + "," + y + ",\"" + codetype + "\"," + height + "," + readable + "," + 0 +
				"," + narrow + "," + wide + ",\"" + content + "\"\r\n"
			jpPrinter.addCommand(data)
		};

		//添加图片，res为画布参数
		jpPrinter.setBitmap = function(x, y, width, height, mode, res) {
			// var width = parseInt((res.width) / 8 * 8 / 8)
			// var height = res.height
			// var imgWidth = res.width
			var pointList = []
			var resultData = [] 
			data = "BITMAP " + x + "," + y + "," + width / 8 + "," + height + "," + mode + ","
			jpPrinter.addCommand(data)
			// console.log(res.data)
			// console.log('---以上是原始数据---')

			// for循环顺序不要错了，外层遍历高度，内层遍历宽度，因为横向每8个像素点组成一个字节
			for (var y = 0; y < height; y++) {
				for (var x = 0; x < width; x++) {
					let r = res.data[(y * width + x) * 4];
					let g = res.data[(y * width + x) * 4 + 1];
					let b = res.data[(y * width + x) * 4 + 2];
					let a = res.data[(y * width + x) * 4 + 3]
					//console.log(`当前${y}行${x}列像素,rgba值:(${r},${g},${b},${a})`)
					// 像素灰度值
					let grayColor = r * 0.299 + g * 0.587 + b * 0.114
					//灰度值大于128位：浅色，可以不打印。128 还是其他值（0~255），可以自己拿捏
					//1不打印, 0打印 （参考：佳博标签打印机编程手册tspl）
					if (grayColor > 128) {
						pointList.push(1)
					} else {
						pointList.push(0)
					}
				}
			}

			for (var i = 0; i < pointList.length; i += 8) {
				var p = pointList[i] * 128 + pointList[i + 1] * 64 + pointList[i + 2] * 32 + pointList[i +
						3] * 16 + pointList[i + 4] * 8 + pointList[i + 5] * 4 + pointList[i + 6] * 2 +
					pointList[i + 7]
				resultData.push(p)
			}
			console.log('resultData', resultData.length)


			for (var i = 0; i < resultData.length; ++i) {
				// 使用 intToByte 后无法打印数据
				// command.push(intToByte(resultData[i])) 
				command.push(resultData[i])
			}

			// \r\n
			command.push(0x0D)
			command.push(0x0A)
		}
 
		//打印页面
		jpPrinter.setPagePrint = function(nums) {
			data = "PRINT 1," + nums +"\r\n"
			jpPrinter.addCommand(data)
		};

		//获取打印数据
		jpPrinter.getData = function() {
			return command;
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
	}
};

module.exports.jpPrinter = jpPrinter;