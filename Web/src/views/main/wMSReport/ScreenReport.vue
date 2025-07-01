<template>
	<div style=" width:100%;height: 100%;">
		<div id="topOutdiv">
			<p style="position: absolute;top: 0px;left: 0px;z-index: 999;margin-top: 1%;margin-left: 35%;font-size: 30px;color: white;font-weight: 600;"
				@click="getTable123()">哈希{{state.MonthNow}}月出入库数据分析看板 </p>

		</div>
		<div id="middleOutdiv">
			<div id="tops">

				<el-card class="cardstop" style="margin-left: 0%;">
					<p class="pfonttop">{{state.QtyIn}}</p><br>
					<p style="font-size:20px;margin-left: 15%;font-weight:500;">{{state.MonthNow}}月入库总数量</p>
				</el-card>
				<el-card class="cardstop">
					<p class="pfonttop">{{state.MoneyIn}}</p><br>
					<p style="font-size:20px;margin-left: 15%;font-weight:500;">{{state.MonthNow}}月入库总金额</p>
				</el-card>
				<el-card class="cardstop">
					<p class="pfonttop">{{state.QtyOut}}</p><br>
					<p style="font-size:20px;margin-left: 15%;font-weight:500;">{{state.MonthNow}}月出库总数量</p>
				</el-card>
				<el-card class="cardstop">
					<p class="pfonttop">{{state.MoneyOut}}</p><br>
					<p style="font-size:20px;margin-left: 24%;font-weight:500;">{{state.MonthNow}}月出库总金额</p>
				</el-card>
			</div>
			<div id="middles">
				<div class="cardsmiddle" style="margin-left: 0%;">
					<div style="border-bottom: 1px solid rgb(191, 197, 197); width: 100%;height: 13%;">
						<p style="font-size: 18px;margin-left:1%;margin-top: 2%;">TOP前5客户订单占比图</p>
					</div>
					<div style="width: 100%;height: 85%;" id="middleOne">

					</div>
				</div>
				<div class="cardsmiddle">
					<div style="border-bottom: 1px solid rgb(191, 197, 197); width: 100%;height: 13%;">
						<p style="font-size: 18px;margin-left:1%;margin-top: 2%;">出入库数量&出入库金额</p>
					</div>
					<div style="width: 100%;height: 85%;" id="middleTwo">

					</div>
				</div>
				<div class="cardsmiddle" style="margin-left: 0%;">
					<div style="border-bottom: 1px solid rgb(191, 197, 197); width: 100%;height: 13%;">
						<p style="font-size: 18px;margin-left:1%;margin-top: 2%;">TOP前5货号排名</p>
					</div>
					<div style="width: 100%;height: 85%;" id="middleThree">

					</div>
				</div>
				<div class="cardsmiddle">
					<div style="border-bottom: 1px solid rgb(191, 197, 197); width: 100%;height: 13%;">
						<p style="font-size: 18px;margin-left:1%;margin-top: 2%;">TOP前3客户出库数量&金额对比图</p>
					</div>
					<div style="width: 100%;height: 85%;" id="middleFour">

					</div>
				</div>

			</div>
			<div id="bottoms">

				<div class="cardsbottoms">
					<div style="width: 100%;height: 85%;overflow: scroll;" id="bottomThree">
						<table
							style="height: 100%; border-collapse: collapse;border: 1px solid rgb(204, 190, 190);width: 100%;font:center;background-color:#F0F0F0 ;">
							<thead>
								<tr
									style="border: 1px solid rgb(204, 190, 190);background-color:#D8D8D8;color:darkslategray;text-align:center;">
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										客户
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										入库
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										金额
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										出库
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										金额
									</td>
								</tr>
							</thead>
							<tbody id="tbody1">

							</tbody>
						</table>

					</div>
				</div>
				<div class="cardsbottoms">
					<div style="width: 100%;height: 85%;" id="bottomThree">
						<table
							style="border-collapse: collapse;border: 1px solid rgb(204, 190, 190);width: 100%;height: 100%;font:center;background-color:#F0F0F0 ;">
							<thead>
								<tr
									style="border: 1px solid rgb(204, 190, 190);background-color:#D8D8D8;color: darkslategray;text-align:center;">
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										Top前五货号</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">出库数量
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										金额</td>
								</tr>
							</thead>
							<tbody id="tbody2">

							</tbody>
						</table>

					</div>
				</div>
				<div class="cardsbottoms">
					<div style="width: 100%;height: 85%;" id="bottomThree">
						<table
							style="border-collapse: collapse;border: 1px solid rgb(204, 190, 190);width: 100%;height: 100%;font:center;background-color:#F0F0F0 ;">
							<thead>
								<tr
									style="border: 1px solid rgb(204, 190, 190);background-color:#D8D8D8;color: darkslategray;text-align:center;">
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										Top前三客户</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">出库数量
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										金额</td>
								</tr>
							</thead>
							<tbody id="tbody3">

							</tbody>
						</table>

					</div>
				</div>
				<div class="cardsbottoms">
					<div style="width: 100%;height: 85%;" id="bottomThree">
						<table
							style="border-collapse: collapse;border: 1px solid rgb(204, 190, 190);width: 100%;height: 100%;font:center;background-color:#F0F0F0 ;">
							<thead>
								<tr
									style="border: 1px solid rgb(204, 190, 190);background-color:#D8D8D8;color: darkslategray;text-align:center;">
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										Top前五客户</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">订单数
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										占比</td>
								</tr>
							</thead>
							<tbody id="tbody4">

							</tbody>
						</table>

					</div>
				</div>
			</div>
		</div>
	</div>
</template>
<style>
#topOutdiv {
	background-color: black;
	width: 100%;
	height: 8%;
}

#middleOutdiv {
	background-color: white;
	width: 100%;
	height: 92%;
}

.cardstop {
	height: 90%;
	width: 23.3%;
	margin-left: 2.1%;
	margin-top: 0.6%;
	float: left;
	background-color: #f0F0F0;

}

.cardsmiddle {
	background-color: rgb(255, 255, 255);
	height: 48.8%;
	width: 47%;
	margin-left: 0.3%;
	margin-top: 0.6%;
	float: left;
}

.cardsbottoms {
	background-color: white;
	height: 24.73%;
	width: 95%;
	margin-left: 2.3%;
	margin-top: 1%;
}

.pfonttop {
	font-size: 30px;
	margin-left: 27%;
	color: green;
	font-weight: 700;
}

.pfontbottom {
	font-size: 20px;
	margin-left: 25%;
}



#tops {
	background-color: #E8E8E8;
	width: 74%;
	height: 20%;
	float: right;
}

#middles {
	background-color: white;
	width: 74%;
	height: 80%;
	float: right;
}

#bottoms {
	background-color: white;
	width: 25%;
	height: 100%;
}
</style>
<script setup lang="ts" name="home">
import { reportScreen } from '/@/api/main/wMSRFIDInfo';
import { reactive, onMounted } from 'vue';
import * as echarts from 'echarts';
const state = reactive({
	Datas: {} as  any,
	
	arr1customerName:[] as any,
	arr1reSumPrice:[] as any,
	arr1reSumQty:[] as any,
	arr1woSumPrice:[] as any,
	arr1woSumQty:[] as any,

	arr2totalMoney:[] as any,
	arr2orderQty:[] as any,
	arr2sku:[] as any,

	arr3percentage:[] as any,
	arr3customerName:[] as any,
	
	arr4Name:[] as any,
	arr4Value:[] as any,

	QtyIn:'' as any,
	QtyOut:'' as any,
	MoneyIn:'' as any,
	MoneyOut:'' as any,

	MonthNow:'' as any,
});

onMounted(async () => {
	const now = new Date();
      
    state.MonthNow = now.getMonth() + 1;


	state.Datas = (await reportScreen()).data.result;
	state.QtyIn =state.Datas.allMoneyQtyOut[0].inReceiptqty;
	state.MoneyIn =state.Datas.allMoneyQtyOut[0].inTotalMoney;
	state.QtyOut =state.Datas.allMoneyQtyIn[0].outOrderQty;
	state.MoneyOut =state.Datas.allMoneyQtyIn[0].outTotalMoney;


	InsertDateToEcharts();
	handleQuery1();
	handleQuery2();
	handleQuery3();
	handleQuery4();
	getTable();
	getTable2();
	getTable3();
	getTable4();
});

const handleQuery1 = async () => {

	var rightTopLeft = document.getElementById('middleOne');
	var myChartrightTopLeft = echarts.init(rightTopLeft);
	var optionrightTopLeft = {

		tooltip: {
			trigger: 'item',
			formatter: '{a} <br/>{b} : {c} ({d}%)'
		},
		legend: {
			orient: 'vertical',
			// left: 'left',
			right: 5,
			bottom: 30,
			itemWidth: 10,
			data: state.arr4Name,
			textStyle: {
				color: 'rgba(132, 214, 255, 1)',
			},
		},
		series: [
			{
				name: '',
				type: 'pie',
				radius: '80%',
				center: ['40%', '50%'],
				data: state.arr4Value,
				// [
				// 	{ value: 335, name: '寄递行业' },
				// 	{ value: 310, name: '服装' },
				// 	{ value: 234, name: '餐饮 ' },
				// 	{ value: 135, name: '建材' },
				// ],
				label: {
					formatter: '{c}件\n{d}%',
				},
				emphasis: {
					itemStyle: {
						shadowBlur: 10,
						shadowOffsetX: 0,
						shadowColor: 'rgba(0, 0, 0, 0.5)'
					}
				},
				color: ['rgba(240, 232, 136, 1)', 'rgba(36, 205, 220, 1)', 'rgba(75, 106, 217, 1)', 'rgba(242, 100, 99, 1)'],
			}
		]
	};
	myChartrightTopLeft.setOption(optionrightTopLeft);
};

const handleQuery2 = async () => {

	const seriesData1 = state.arr1woSumQty;
	const seriesData2 = state.arr1woSumPrice;
	const seriesData3 = state.arr1reSumQty;
	const seriesData4 = state.arr1reSumPrice;
	const xAxisList = state.arr1customerName;
	// [
	// 	'11.06',
	// 	'11.07',
	// 	'11.08',
	// 	'11.09',
	// ]
	var rightTopLeft = document.getElementById('middleTwo');
	var myChartrightTopLeft = echarts.init(rightTopLeft);
	var optionrightTopLeft = {
		"backgroundColor": "#FFFFFF",
		"legend": {
			"x": "center",
			"textStyle": {
				"color": "#000000",
				"fontSize": 16
			},
			"top": 12,
			"itemWidth": 18,
			"itemHeight": 12,
			"itemGap": 40
		},
		tooltip: {
			trigger: 'axis'
		},
		grid: {
			top: '30%',
			left: '5%',
			right: '10%',
			bottom: '15%',
			containLabel: true
		},
		xAxis: {
			type: 'category',
			data: xAxisList,

			axisLabel: {
				color: "#000000",
				fontSize: 16,
			},
			axisLine: {
				lineStyle: {
					color: '#000000'
				}
			},
			axisTick: {
				show: false
			}
		},
		yAxis: {
			type: 'value',
			name: '',
			nameTextStyle: {
				color: "#000000",
				fontSize: 16,
			},
			axisLabel: {
				color: "#000000",
				fontSize: 16,
			},
			axisLine: {
				lineStyle: {
					color: '#A9AEB2'
				}
			},
			splitLine: {
				"show": true,
				"lineStyle": {
					"color": [
						"#1B2648"
					],
					"width": 1,
					"type": 'dashed'
				},
			},
		},
		series: [
			{
				name: '入库',
				type: 'line',
				symbol: 'circle',
				symbolSize: 10,
				z: 1,
				itemStyle: {
					color: '#16C231'
				},
				lineStyle: {
					color: '#16C231'
				},
				data: seriesData1
			},
			{
				name: '金额',
				type: 'line',
				symbol: 'circle',
				symbolSize: 10,
				z: 1,
				itemStyle: {
					color: '#0A6BC5'
				},
				lineStyle: {
					color: '#0A6BC5'
				},
				data: seriesData2
			},
			{
				name: '出库',
				type: 'line',
				symbol: 'circle',
				symbolSize: 10,
				z: 1,
				itemStyle: {
					color: '#EE5959'
				},
				lineStyle: {
					color: '#EE5959'
				},
				data: seriesData3
			},
			{
				name: '出库金额',
				type: 'line',
				symbol: 'circle',
				symbolSize: 10,
				z: 1,
				itemStyle: {
					color: '#EE5959'
				},
				lineStyle: {
					color: '#EE5959'
				},
				data: seriesData4
			},
		]
	};
	myChartrightTopLeft.setOption(optionrightTopLeft);
};

const handleQuery3 = async () => {
	var rightTopLeft = document.getElementById('middleThree');
	var myChartrightTopLeft = echarts.init(rightTopLeft);
	var optionrightTopLeft = {
		title: {
			// text: '热源实时流量',
			textStyle: {
				align: 'center',
				color: '#fff',
				fontSize: 20,
			},
			top: '3%',
			left: '10%',
		},
		backgroundColor: '#FFFFFF',
		grid: {
			top: "25%",
			bottom: "10%"//也可设置left和right设置距离来控制图表的大小
		},
		tooltip: {
			trigger: "axis",
			axisPointer: {
				type: "shadow",
				label: {
					show: false, //X轴的
				}
			}
		},
		legend: {
			data: ["计划", "完成"],
			top: "15%",
			textStyle: {
				color: "#000000"
			}
		},
		xAxis: {
			data: state.arr2sku,
			// [
			// 	"C15喷射",
			// 	"C25组",
			// 	"C36水泵送",
			// 	"C85补偿收缩"

			// ],
			axisLine: {
				show: true, //隐藏X轴轴线
				lineStyle: {
					color: '#000000'
				}
			},
			axisTick: {
				show: true //隐藏X轴刻度
			},
			axisLabel: {
				show: true,
				textStyle: {
					color: "#000000" //X轴文字颜色
				}
			},

		},
		yAxis: [
			{
				type: "value",
				name: "m³",
				nameTextStyle: {
					color: "#000000",
					fontSize: 15
				},
				splitLine: {
					show: false
				},
				axisTick: {
					show: true
				},
				axisLine: {
					show: true,
					lineStyle: {
						color: '#000000'
					}
				},
				axisLabel: {
					show: true,
					textStyle: {
						color: "#000000"
					}
				},

			},

			{
				type: "value",
				gridIndex: 0,
				min: 50,
				max: 100,
				splitNumber: 8,
				splitLine: {
					show: false
				},
				axisLine: {
					show: false
				},
				axisTick: {
					show: false
				},
				axisLabel: {
					show: false
				},
				splitArea: {
					show: true,
					areaStyle: {
						color: ["#FFFFFF", "#FFFFFF"]
					}
				}
			}
		],
		series: [{
			name: "出库数量",
			type: "bar",
			// yAxisIndex: 1,
			// barGap:'100%',
			barWidth: 15,//使用的 y 轴的 index，在单个图表实例中存在多个 y轴的时候有用
			smooth: true, //平滑曲线显示
			showAllSymbol: true, //显示所有图形。
			symbol: "circle", //标记的图形为实心圆
			//  color: '#FF8247',


			itemStyle: {
				normal: {
					color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
						offset: 0,
						color: "#A0C2F6",
					},
					{
						offset: 0.5,
						color: "#7DABF7"
					},
					{
						offset: 1,
						color: "#6397F7"
					}
					]),
				},
			},
			data: state.arr2orderQty
		},
		{
			name: "金额",
			type: "bar",
			barWidth: 15,

			itemStyle: {
				normal: {
					color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
						offset: 0,
						color: "#FAF2BC",
					},
					{
						offset: 0.5,
						color: "#F8EB97"
					},
					{
						offset: 1,
						color: "#F5E46C"
					}
					]),
				},
			},
			data: state.arr2totalMoney
		}
		]
	};
	myChartrightTopLeft.setOption(optionrightTopLeft);
};

const handleQuery4 = async () => {

	var xdata = state.arr3customerName;
	var data = state.arr3percentage;
	var rightTopLeft = document.getElementById('middleFour');
	var myChartrightTopLeft = echarts.init(rightTopLeft);
	var optionrightTopLeft = {
		//你的代码
		backgroundColor: '#FFFFFF',
		tooltip: {
			trigger: "axis",
			axisPointer: { // 坐标轴指示器，坐标轴触发有效
				type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
			}
		},
		grid: {
			left: "3%",
			right: "4%",
			bottom: "5%",
			top: "5%",
			containLabel: true
		},
		xAxis: {
			data: xdata,
			triggerEvent: true,
			axisTick: {
				show: false
			},
			axisLine: {
				show: true,
				lineStyle: {
					color: 'rgba(255, 255, 255, 0.3)'
				}
			},
			axisLabel: {
				show: true,
				rotate: 0,
				interval: 0,
				textStyle: {
					// padding: [14,0,0,0],
					fontSize: 14,
					color: "#000000"
				}
			}
		},
		yAxis: {
			triggerEvent: true,
			nameTextStyle: {
				color: "000000",
				fontSize: 16,
				padding: [0, 0, 10, -20]
			},
			splitLine: {
				show: true,
				lineStyle: {
					color: '000000',
					"type": 'dashed'
				}
			},
			
			axisTick: {
				show: false
			},
			axisLine: {
				show: true,
				lineStyle: {
					color: '000000'
				}
			},
			axisLabel: {
				show: true,
				textStyle: {
					color: "#000000",
					fontSize: 14
				}
			}
		},
		series: [{
			name: "",
			barMinHeight: 10,
			type: "pictorialBar",
			barCategoryGap: "50%",
			symbol: 'path://M0,10 L10,10 L5,0 L0,10 z',
			// symbol: "path://M0,10 L10,10 C5.5,10 5.5,5 5,0 C4.5,5 4.5,10 0,10 z",
			itemStyle: {
				normal: {
					//barBorderRadius: 5,
					// borderColor: 'RGBA(100, 218, 237, 1)',
					// borderWidth: 1,
					//渐变色
					color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
						offset: 0,
						color: "RGBA(250, 200, 88, 1)"
					},
					{
						offset: 1,
						color: "RGBA(250, 200, 88, 1)"
					}
					])
				}
			},
			label: {
				normal: {
					show: true,
					position: "top",
					textStyle: {
						color: "#000000",
						fontSize: 14
					}
				}
			},
			data: data,
			z: 10
		}
		]
	};
	myChartrightTopLeft.setOption(optionrightTopLeft);
};


const getTable=async()=>{
	
	var tableappend = "";
    for (var i = 0; i < state.Datas.custOrderReceiptAmount.length; i++) {
       tableappend += '<tr>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.custOrderReceiptAmount[i].customerName + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.custOrderReceiptAmount[i].reSumQty + '</th>'; 
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.custOrderReceiptAmount[i].reSumPrice + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.custOrderReceiptAmount[i].woSumQty + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.custOrderReceiptAmount[i].woSumPrice + '</th>';
       tableappend += '</tr> '; 
   }
   document.getElementById('tbody1').innerHTML = tableappend;
}
const getTable2=async()=>{
	
	var tableappend = "";
    for (var i = 0; i < state.Datas.topFiveOrderCountAmount.length; i++) {
       tableappend += '<tr>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.topFiveOrderCountAmount[i].sku + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.topFiveOrderCountAmount[i].orderQty + '</th>'; 
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.topFiveOrderCountAmount[i].totalMoney + '</th>';
       tableappend += '</tr> '; 
   }
   
   document.getElementById('tbody2').innerHTML = tableappend;
}
const getTable3=async()=>{
	
	var tableappend = "";
    for (var i = 0; i < state.Datas.topThreeOrderCountAmount.length; i++) {
       tableappend += '<tr>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.topThreeOrderCountAmount[i].customerName + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.topThreeOrderCountAmount[i].orderQty + '</th>'; 
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.topThreeOrderCountAmount[i].totalMoney + '</th>';
       tableappend += '</tr> '; 
   }
  
   document.getElementById('tbody3').innerHTML = tableappend;
}
const getTable4=async()=>{
	
	var tableappend = "";
    for (var i = 0; i < state.Datas.topFiveCustOrderCountGdp.length; i++) {
       tableappend += '<tr>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.topFiveCustOrderCountGdp[i].customerName + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.topFiveCustOrderCountGdp[i].orderCount + '</th>'; 
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.topFiveCustOrderCountGdp[i].percentage + '</th>';
       tableappend += '</tr> '; 
   }
   
   document.getElementById('tbody4').innerHTML = tableappend;
}



const InsertDateToEcharts=async()=>{
	
	for (var i = 0; i < state.Datas.topFiveCustOrderCountGdp.length; i++) {
		state.arr4Name[i]=state.Datas.topFiveCustOrderCountGdp[i].customerName;		
		var datas={
		name:'',
		value:''
		};
		datas.name=state.Datas.topFiveCustOrderCountGdp[i].customerName;
		datas.value=state.Datas.topFiveCustOrderCountGdp[i].orderCount;
		state.arr4Value[i]=datas;
   }


   for (var j = 0; j < state.Datas.custOrderReceiptAmount.length; j++) {
		state.arr1customerName[j]=state.Datas.custOrderReceiptAmount[j].customerName;
		state.arr1reSumPrice[j]=state.Datas.custOrderReceiptAmount[j].reSumPrice;	
		state.arr1reSumQty[j]=state.Datas.custOrderReceiptAmount[j].reSumQty;	
		state.arr1woSumPrice[j]=state.Datas.custOrderReceiptAmount[j].woSumPrice;	
		state.arr1woSumQty[j]=state.Datas.custOrderReceiptAmount[j].woSumQty;			
		
   }


   for (var k = 0; k < state.Datas.topFiveOrderCountAmount.length; k++) {
		state.arr2totalMoney[k]=state.Datas.topFiveOrderCountAmount[k].totalMoney;
		state.arr2orderQty[k]=state.Datas.topFiveOrderCountAmount[k].orderQty;	
		state.arr2sku[k]=state.Datas.topFiveOrderCountAmount[k].sku;	
				
		
   }
   
   for (var u = 0; u < state.Datas.topThreeOrderCountAmount.length; u++) {
		state.arr3customerName[u]=state.Datas.topThreeOrderCountAmount[u].customerName;
		state.arr3percentage[u]=state.Datas.topThreeOrderCountAmount[u].totalMoney;	
				
		
   }
   
   
   
   
   


}



//调试专用刷新数据
const getTable123=async()=>{
	
	handleQuery1();
	handleQuery2();
	handleQuery3();
	handleQuery4();
	getTable();
	getTable2();
	getTable3();
	getTable4();
	InsertDateToEcharts();
}
</script>