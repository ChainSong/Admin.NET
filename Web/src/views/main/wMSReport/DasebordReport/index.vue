<template>
	<div style=" width:100%;height: 100%;">
		<div id="topOutdiv">
			<p style="position: absolute;top: 0px;left: 0px;z-index: 999;margin-top: 1%;margin-left: 35%;font-size: 30px;color: white;font-weight: 600;"
				@click="getTable123()">哈希{{state.MonthNow}}月出入库数据分析看板 </p>

		</div>
		<div id="middleOutdiv">
			<div id="tops">

				<el-card class="cardstop" style="margin-left: 0%;">
					<p class="pfonttop">{{state.num1}}</p><br>
					<p class="pfonttop2">{{state.MonthNow}}月入库总数量</p>
				</el-card>
				<el-card class="cardstop">
					<p class="pfonttop">{{state.num2}}</p><br>
					<p class="pfonttop2">{{state.MonthNow}}月入库总金额</p>
				</el-card>
				<el-card class="cardstop">
					<p class="pfonttop">{{state.num3}}</p><br>
					<p class="pfonttop2">{{state.MonthNow}}月库存数量</p>
				</el-card>
				<el-card class="cardstop">
					<p class="pfonttop">{{state.num4}}</p><br>
					<p class="pfonttop2" style="margin-left: 50px;">{{state.MonthNow}}月与上月库存数量对比</p>
				</el-card>

				<el-card class="cardstop" style="margin-left: 0%;">
					<p class="pfonttop">{{ state.num5 }}</p><br>
					<p class="pfonttop2">{{ state.MonthNow }}月出库总数量</p>
				</el-card>
				<el-card class="cardstop">
					<p class="pfonttop">{{ state.num6 }}</p><br>
					<p class="pfonttop2">{{ state.MonthNow }}月出库总金额</p>
				</el-card>
				<el-card class="cardstop">
					<p class="pfonttop">{{ state.num7 }}</p><br>
					<p class="pfonttop2">{{ state.MonthNow }}月库存金额</p>
				</el-card>
				<el-card class="cardstop">
					<p class="pfonttop">{{ state.num8 }}</p><br>
					<p class="pfonttop2" style="margin-left: 50px;">{{ state.MonthNow }}月与上月库存金额对比</p>
				</el-card>
			</div>
			<!-- <div id="tops1">

				
			</div> -->
			<div id="middles">
				<div class="cardsmiddle" style="margin-left: 0%;">
					<div style="border-bottom: 1px solid rgb(191, 197, 197); width: 100%;height: 13%;">
						<p style="font-size: 18px;margin-left:1%;margin-top: 2%;">{{ state.MonthNow }}月入库数量&出库数量&库存数量</p>
					</div>
					<div style="width: 100%;height: 85%;" id="middleOne">

					</div>
				</div>
				<div class="cardsmiddle">
					<div style="border-bottom: 1px solid rgb(191, 197, 197); width: 100%;height: 13%;">
						<p style="font-size: 18px;margin-left:1%;margin-top: 2%;">{{ state.MonthNow }}月入库金额&出库金额&库存金额</p>
					</div>
					<div style="width: 100%;height: 85%;" id="middleTwo">

					</div>
				</div>
				<div class="cardsmiddle" style="margin-left: 0%;">
					<div style="border-bottom: 1px solid rgb(191, 197, 197); width: 100%;height: 13%;">
						<p style="font-size: 18px;margin-left:1%;margin-top: 2%;">{{ state.MonthNow }}月订单数量</p>
					</div>
					<div style="width: 100%;height: 85%;" id="middleThree">

					</div>
				</div>
				<div class="cardsmiddle">
					<div style="border-bottom: 1px solid rgb(191, 197, 197); width: 100%;height: 13%;">
						<p style="font-size: 18px;margin-left:1%;margin-top: 2%;">{{ state.MonthNow }}月客户区域占比图</p>
					</div>
					<div style="width: 100%;height: 85%;" id="middleFour">

					</div>
				</div>

			</div>
			<div id="bottoms">

				<div class="cardsbottoms">
					<div style="width: 100%;height: 96%;overflow: scroll;" id="bottomThree">
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
										出库
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										库存
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										入库金额
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										出库金额
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										库存金额
									</td>
								</tr>
							</thead>
							<tbody id="tbody1">

							</tbody>
						</table>

					</div>
				</div>
				<div class="cardsbottoms">
					<div style="width: 100%;height: 96%;overflow: scroll;" id="bottomThree">
						<table
							style="border-collapse: collapse;border: 1px solid rgb(204, 190, 190);width: 100%;height: 100%;font:center;background-color:#F0F0F0 ;">
							<thead>
								<tr
									style="border: 1px solid rgb(204, 190, 190);background-color:#D8D8D8;color: darkslategray;text-align:center;">
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										客户</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										上月订单数量
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										当月订单数量</td>
								</tr>
							</thead>
							<tbody id="tbody2">

							</tbody>
						</table>

					</div>
				</div>
				<div class="cardsbottoms">
					<div style="width: 100%;height: 96%;overflow: scroll;" id="bottomThree">
						<table
							style="border-collapse: collapse;border: 1px solid rgb(204, 190, 190);width: 100%;height: 100%;font:center;background-color:#F0F0F0 ;">
							<thead>
								<tr
									style="border: 1px solid rgb(204, 190, 190);background-color:#D8D8D8;color: darkslategray;text-align:center;">
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										区域</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										订单数量
									</td>
									<td style="border: 1px solid rgb(204, 190, 190);height: 35px;text-align:center;">
										占比</td>
								</tr>
							</thead>
							<tbody id="tbody3">

							</tbody>
						</table>

					</div>
				</div>
				<!-- <div class="cardsbottoms">
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
				</div> -->
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
	height: 44%;
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
	height: 32.5%;
	width: 95%;
	margin-left: 2.3%;
	margin-top: 1%;
}

.pfonttop {
    margin-top: -16px;
    font-size: 20px;
    margin-left: 33%;
    color: green;
    font-weight: 700;
}


.pfonttop2 {
    font-size:13px;
	margin-left: 30%;
	font-weight:500;
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
#tops1 {
	background-color: #E8E8E8;
	width: 74%;
	height: 10%;
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
import { reportScreenSecond } from '/@/api/main/wMSRFIDInfo';
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
	
	num1:'' as any,
	num2:'' as any,
	num3:'' as any,
	num4:'' as any,
	num5:'' as any,
	num6:'' as any,
	num7:'' as any,
	num8:'' as any,
});

onMounted(async () => {
	const now = new Date();
      
    state.MonthNow = now.getMonth() + 1;


	state.Datas = (await reportScreenSecond()).data.result;
	console.log(state.Datas);

	state.num1 =GetIsNullToStr(state.Datas.eightNum.num1);
	state.num2 =GetIsNullToStr(state.Datas.eightNum.num2);
	state.num3 =GetIsNullToStr(state.Datas.eightNum.num3);
	state.num4 =GetIsNullToStr(state.Datas.eightNum.num4);
	state.num5 =GetIsNullToStr(state.Datas.eightNum.num5);
	state.num6 =GetIsNullToStr(state.Datas.eightNum.num6);
	state.num7 =GetIsNullToStr(state.Datas.eightNum.num7);
	state.num8 =GetIsNullToStr(state.Datas.eightNum.num8);


	//InsertDateToEcharts();

	handleQuery1();
	handleQuery2();
	handleQuery3();
	handleQuery4();
	getTable();
	getTable2();
	getTable3();
});

const handleQuery1 = async () => {

	const seriesData1 = state.Datas.pic1In;
	const seriesData2 = state.Datas.pic1Out;
	const seriesData3 = state.Datas.pic1All;
	const xAxisList = state.Datas.pic1Name;
	
	var rightTopLeft = document.getElementById('middleOne');
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
			top: '18%',
			left: '5%',
			right: '10%',
			bottom: '0%',
			containLabel: true
		},
		xAxis: {
			type: 'category',
			data: xAxisList,

			axisLabel: {
				color: "#000000",
				fontSize: 10,
				rotate: 45,
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
				fontSize: 12,
			},
			axisLabel: {
				color: "#000000",
				fontSize: 12,
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
				name: '入库数量',
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
				name: '出库数量',
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
				name: '库存数量',
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
		]
	};
	myChartrightTopLeft.setOption(optionrightTopLeft);
};

const handleQuery2 = async () => {

	const seriesData1 = state.Datas.pic2In;
	const seriesData2 = state.Datas.pic2Out;
	const seriesData3 = state.Datas.pic2All;
	const xAxisList = state.Datas.pic2Name;
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
			top: '18%',
			left: '5%',
			right: '10%',
			bottom: '0%',
			containLabel: true
		},
		xAxis: {
			type: 'category',
			data: xAxisList,

			axisLabel: {
				color: "#000000",
				fontSize: 10,
				rotate: 45,
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
				fontSize: 12,
			},
			axisLabel: {
				color: "#000000",
				fontSize: 12,
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
				name: '入库金额',
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
				name: '出库金额',
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
				name: '库存金额',
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
		]
	};
	myChartrightTopLeft.setOption(optionrightTopLeft);
};

const handleQuery3 = async () => {
	const seriesData1 = state.Datas.pic3Now;
	const seriesData2 = state.Datas.pic3Last;
	const xAxisList = state.Datas.pic3Name;
	// [
	// 	'11.06',
	// 	'11.07',
	// 	'11.08',
	// 	'11.09',
	// ]
	var rightTopLeft = document.getElementById('middleThree');
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
			top: '18%',
			left: '5%',
			right: '10%',
			bottom: '0%',
			containLabel: true
		},
		xAxis: {
			type: 'category',
			data: xAxisList,

			axisLabel: {
				color: "#000000",
				fontSize: 10,
				rotate: 45,
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
				fontSize: 12,
			},
			axisLabel: {
				color: "#000000",
				fontSize: 12,
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
				name: '当月订单数量',
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
				name: '上月订单数量',
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
		]
	};
	myChartrightTopLeft.setOption(optionrightTopLeft);
};

const handleQuery4 = async () => {

	var rightTopLeft = document.getElementById('middleFour');
	var myChartrightTopLeft = echarts.init(rightTopLeft);
	var optionrightTopLeft = {
		color: ['#59A2FE', '#FFB717', '#FC8F6B', '#70E07B', '#FBDA3D', '#11E5F1', '#E3A0FF', '#8294FC', '#6AD6FE'],
    tooltip: {
        trigger: 'item',
        formatter: '{b} : {c} ({d}%)',
    },
    legend: {
        orient: 'vertical',
        itemWidth: 18,
        itemHeight: 6,
        right: '10%',
        align: 'left',
        top: 'middle',
        textStyle: {
            fontSize: 12,
            fontFamily: 'PingFangSC-Regular, PingFang SC',
            fontWeight: 400,
            color: '#0D1134',
            lineHeight: 17,
        },
    },
    series: [
        {
            type: 'pie',
            radius: '80%',
            labelLine: {
                normal: {
                    length: 20,
                    length2: 30,
                },
            },

            label: {
                normal: {
                    formatter: '{a|{b}}\n{per|{d}%}',
                    rich: {
                        a: {
                            fontSize: 12,
                            lineHeight: 16,
                            align: 'center',
                        },
                        per: {
                            color: '#545454',
                            align: 'center',
                            fontSize: 12,
                            lineHeight: 16,
                        },
                    },
                },
            },

            data:state.Datas.pic4NameVal,
        },
    ],
	};
	myChartrightTopLeft.setOption(optionrightTopLeft);
};


const getTable=async()=>{
	
	var tableappend = "";
    for (var i = 0; i < state.Datas.leftTable1.length; i++) {
       tableappend += '<tr>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable1[i].customerName + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable1[i].reSumQty + '</th>'; 
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable1[i].woSumQty + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable1[i].allSumQty + '</th>';//+
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable1[i].reSumQty + '</th>';//+
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable1[i].woSumQty + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable1[i].allSumPrice + '</th>';
       tableappend += '</tr> '; 
   }
   document.getElementById('tbody1').innerHTML = tableappend;
}
const getTable2=async()=>{
	
	var tableappend = "";
    for (var i = 0; i < state.Datas.leftTable2.length; i++) {
       tableappend += '<tr>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable2[i].customerName + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable2[i].countslast + '</th>'; 
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable2[i].countsnow + '</th>';
       tableappend += '</tr> '; 
   }
   
   document.getElementById('tbody2').innerHTML = tableappend;
}
const getTable3=async()=>{
	
	var tableappend = "";
    for (var i = 0; i < state.Datas.leftTable3.length; i++) {
       tableappend += '<tr>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable3[i].bigName + '</th>';
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable3[i].counts + '</th>'; 
       tableappend += '<th style="border: 1px solid rgb(204, 190, 190);height: 20px;">' + state.Datas.leftTable3[i].percentage + '</th>';
       tableappend += '</tr> '; 
   }
  
   document.getElementById('tbody3').innerHTML = tableappend;
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


function GetIsNullToStr(str) {
	if (str >=10000) {
		return String((str/10000).toFixed(2))+'万';
	}
	else
	{
		return str;
	}
}
</script>