uniapp-sdk使用说明
项目结构说明
 
1.	佳博打印机机ESC/TSC打印指令封装组件
2.	uniapp-sdk使用说明文档
3.	安卓原生本地插件
4.	demo演示页面

使用说明
打印指令调用说明
1. 引入组件 
var tsc = require('../../components/gprint/tsc.js') 
var esc = require('../../components/gprint/esc.js') 
2. 创建指令对象 
var command = tsc.jpPrinter.createNew() 
var command = esc.jpPrinter.createNew() 
3. esc指令编程 
esc编程一般流程：调用初始化–>调用打印对象方法–>打印输出（步骤循环） 
//先初始化 
command.init() 
//打印文本，参数说明请见esc.js文件的setText 
command.setText(1,1,2,1,0,0,"5大云优势，提供一体式创新解决方案") 
//打印条码，参数说明请见esc.js文件的setBar 
command.setBar(1,1,5,80,3, "11111111111") 
//打印二维码，参数说明请见esc.js文件的setQR 
command.setQR(2,12,"L","https://www.baidu.com/") 
//打印输出，setPrintAndFeedRow为输出并换行 
command.setPrint() 
4. tsc指令编程 
tsc编程一般流程：设置全局属性(标签大小、间隙、清空缓存、速度等)–>设置对象1–>设置对象2…–>打印输出 
//设置全局标签大小 
command.setSize(40, 30) 
//设置标签间隙 
command.setGap(2) 
//清空缓存 
command.setCls() 
//设置文本对象 
command.setText(50, 10, "TSS24.BF2", 1, 1, "打印测试") 
//设置二维码对象 
command.setQR(50, 50, "L", 5, "A", "www.poscom.cn") 
//打印输出 
command.setPagePrint()
蓝牙打印
蓝牙连接流程
开启定位(uni.getLocation) --> 
打开蓝牙(uni.openBluetoothAdapter) --> 
获取蓝牙状态(getBluetoothAdapterState) --> 
搜索可用蓝牙列表(uni.startBluetoothDevicesDiscovery) --> 
连接打印机蓝牙(uni.createBLEConnection) --> 
获取蓝牙可用服务 (uni.getBLEDeviceServices) --> 
发送十进制数据(uni.writeBLECharacteristicValue)

参考API：
https://uniapp.dcloud.net.cn/api/location/location.html
https://uniapp.dcloud.net.cn/api/system/bluetooth.html


注意事项：
1. 手机蓝牙向打印机发送成功: uni.writeBLECharacteristicValue：ok , 但是不打印的大部分原因，是发送的数据不符合 esc（票据模式）或 tsc（标签模式）数据格式 ，请检查自己的数据格式 还有编码格式应为 gb18030 
2. 蓝牙模块特征值里面write=true （properties.write == true） 代表可以给蓝牙设备写数据。 如果有多个write=true蓝牙特征值，可以任取其中一个都能发送打印数据 
3. 安卓手机Android底层可能做了限制只能接受20个字节，所以数据包要拆分成20字节一组，多组多次发送 demo里面有拆分操作
Socket网络打印
下载HbuildX编译器
https://dcloud.io/hbuilderx.html
制作自定义调试基座
 
连接手机调试
 

打印机启动
佳博打印机开机，使用网线或WIFI连上网络(局域网)后，打印机默认会监听9100端口

连接打印机
页面输入打印机ip和端口号(默认9100)，点击连接，连接成功提示后，即可与打印机通信交互

注意事项：
1. 一台打印机一次仅允许一个客户端连接，如果调试时未成功关闭连接，需要重启打印机才能重新建立连接 
2. 图片打印机未完全实现，请参考uniapp安卓原生获取本地图片的API，然后参考蓝牙页面对图片进行位图处理即可 
3. 项目默认启动页面，可以在pages.json页面的condition->path中修改



