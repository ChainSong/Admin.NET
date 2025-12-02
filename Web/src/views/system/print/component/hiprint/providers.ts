/* eslint-disable */
import { hiprint } from 'vue-plugin-hiprint';
import logoImg from '/@/assets/logo.png';

// 自定义设计元素1
export const aProvider = function () {
	var addElementTypes = function (context: any) {
		context.removePrintElementTypes('aProviderModule');
		context.addPrintElementTypes('aProviderModule', [
			new hiprint.PrintElementTypeGroup('【公共组件】', [
				{
					tid: 'aProviderModule.barcode',
					title: '条形码',
					data: '18020030720',
					type: 'text',
					options: {
						field: 'barCode',
						testData: 'ZUO18020030720',
						height: 32,
						fontSize: 12,
						lineHeight: 18,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
						textType: 'barcode',
					},
				},
				{
					tid: 'aProviderModule.qrcode',
					title: '二维码',
					data: 'ZUO18020030720',
					type: 'text',
					options: {
						field: 'qrCode',
						testData: 'ZUO18020030720',
						height: 64,
						width: 64,
						fontSize: 12,
						lineHeight: 18,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
						textType: 'qrcode',
					},
				},
				{
					tid: 'aProviderModule.table',
					title: '表格',
					type: 'table',
					options: {
						field: 'table',
						tableHeaderRepeat: 'first',
						tableFooterRepeat: 'last',
						fields: [
							{ text: 'SKU', field: 'sku' },
							{ text: '名称', field: 'goodsName' },
							{ text: '数量', field: 'qty' },
							{ text: '行号', field: 'lineNumber' },
							{ text: '产品类型', field: 'goodsType' },
							{ text: '库区', field: 'area' },
							{ text: '库位', field: 'location' },
							{ text: '批次号', field: 'batchCode' },
							{ text: '是否SN', field: 'isSN' },
							{ text: 'CN805', field: 'cN805' },
							{ text: '父件', field: 'parents' },
							{ text: 'PO', field: 'poCode' },
						],
					},
					editable: true,
					columnDisplayEditable: true, //列显示是否能编辑
					columnDisplayIndexEditable: true, //列顺序显示是否能编辑
					columnTitleEditable: true, //列标题是否能编辑
					columnResizable: true, //列宽是否能调整
					columnAlignEditable: true, //列对齐是否调整
					isEnableEditField: true, //编辑字段
					isEnableContextMenu: true, //开启右键菜单 默认true
					isEnableInsertRow: true, //插入行
					isEnableDeleteRow: true, //删除行
					isEnableInsertColumn: true, //插入列
					isEnableDeleteColumn: true, //删除列
					isEnableMergeCell: true, //合并单元格
					columns: [
						[
							{ title: 'SKU', align: 'center', field: 'sku', width: 150 },
							{ title: '名称', align: 'center', field: 'goodsName', width: 150 },
							{ title: '数量', align: 'center', field: 'qty', width: 150 },
							{ title: '行号', align: 'center', field: 'lineNumber', width: 150 },
							{ title: '产品类型', align: 'center', field: 'goodsType', width: 150 },
							{ title: '库区', align: 'center', field: 'area', width: 150 },
							{ title: '库位', align: 'center', field: 'location', width: 150 },
							{ title: '批次号', align: 'center', field: 'batchCode', width: 150 },
							{ title: '是否SN', align: 'center', field: 'isSN', width: 150 },
							{ title: 'CN805', align: 'center', field: 'cN805', width: 150 },
							{ title: '父件', align: 'center', field: 'parents', width: 150 },
							{ title: 'PO', align: 'center', field: 'poCode', width: 150 },

							// { title: '名称', align: 'center', field: 'NAME', width: 150 },
							// { title: '数量', align: 'center', field: 'SL', width: 80 },
							// { title: '规格', align: 'center', field: 'GG', width: 80, checked: false },
							// { title: '条码', align: 'center', field: 'TM', width: 100, checked: false },
							// { title: '单价', align: 'center', field: 'DJ', width: 100 },
							// { title: '金额', align: 'center', field: 'JE', width: 100, checked: false },
						],
					],
					// footerFormatter: function (options: unknown, rows: unknown, data: any, currentPageGridRowsData: unknown) {
					//   if (data && data['totalCap']) {
					//     return `<td style="padding:0 10px" colspan="100">${'应收金额大写: ' + data['totalCap']}</td>`
					//   }
					//   return '<td style="padding:0 10px" colspan="100">应收金额大写: </td>'
					// },
				}, {
					tid: 'HachProviderModule.table',
					title: 'Hach发货表格',
					type: 'table',
					options: {
						field: 'table',
						tableHeaderRepeat: 'first',
						tableFooterRepeat: 'last',
						fields: [
							{ text: '序号', field: 'sequence' },
							{ text: '产品代码', field: 'sku' },
							{ text: '产品名称', field: 'goodsName' },
							{ text: '装箱数量', field: 'qty' },
							{ text: '单件箱数', field: 'singelBoxQty' },
							{ text: '总箱数', field: 'totalQty' },
							{ text: '父件SKU', field: 'parentSku' },
							{ text: '备注', field: 'remark' },
						],
					},
					editable: true,
					columnDisplayEditable: true, //列显示是否能编辑
					columnDisplayIndexEditable: true, //列顺序显示是否能编辑
					columnTitleEditable: true, //列标题是否能编辑
					columnResizable: true, //列宽是否能调整
					columnAlignEditable: true, //列对齐是否调整
					isEnableEditField: true, //编辑字段
					isEnableContextMenu: true, //开启右键菜单 默认true
					isEnableInsertRow: true, //插入行
					isEnableDeleteRow: true, //删除行
					isEnableInsertColumn: true, //插入列
					isEnableDeleteColumn: true, //删除列
					isEnableMergeCell: true, //合并单元格
					columns: [
						[
							{ title: '序号', align: 'center', field: 'sequence', width: 150 },
							{ title: '产品代码', align: 'center', field: 'sku', width: 150 },
							{ title: '产品名称', align: 'center', field: 'goodsName', width: 150 },
							{ title: '装箱数量', align: 'center', field: 'qty', width: 150 },
							{ title: '单件箱数', align: 'center', field: 'singelBoxQty', width: 150 },
							{ title: '总箱数', align: 'center', field: 'totalQty', width: 150 },
							{ title: '父件SKU', align: 'center', field: 'parentSku', width: 150 },
							{ title: '备注', align: 'center', field: 'remark', width: 150 },
						],
					],
				},
				{
					tid: 'aProviderModule.customText',
					title: '文本',
					customText: '自定义文本',
					custom: true,
					type: 'text',
					options: {
						width: 200,
						testData: '长文本分页/不分页测试',
					},
				},
				{
					tid: 'aProviderModule.longText',
					title: '长文本',
					type: 'longText',
					options: {
						field: 'test.longText',
						width: 200,
						testData: '长文本分页/不分页测试',
					},
				},
				{ tid: 'aProviderModule.logo', title: 'Logo', data: logoImg, type: 'image', options: { field: 'imageUrl' } },
				{ tid: 'aProviderModule.hline', title: '横线', type: 'hline' },
				{ tid: 'aProviderModule.vline', title: '竖线', type: 'vline' },
				{ tid: 'aProviderModule.rect', title: '矩形', type: 'rect' },
				{ tid: 'aProviderModule.oval', title: '椭圆', type: 'oval' }, {
					tid: 'HachProviderDGModule.table',
					title: 'Hach危险品仓装箱清单',
					type: 'table',
					options: {
						field: 'table',
						tableHeaderRepeat: 'first',
						tableFooterRepeat: 'last',
						fields: [
							{ text: '序号', field: 'sequence' },
							{ text: '产品代码', field: 'sku' },
							{ text: '产品名称', field: 'goodsName' },
							{ text: '装箱数量', field: 'qty' },
							{ text: '单件箱数', field: 'singelBoxQty' },
							{ text: '总箱数', field: 'totalQty' },
							{ text: '父件SKU', field: 'parentSku' },
							{ text: '备注', field: 'remark' },
						],
					},
					editable: true,
					columnDisplayEditable: true, //列显示是否能编辑
					columnDisplayIndexEditable: true, //列顺序显示是否能编辑
					columnTitleEditable: true, //列标题是否能编辑
					columnResizable: true, //列宽是否能调整
					columnAlignEditable: true, //列对齐是否调整
					isEnableEditField: true, //编辑字段
					isEnableContextMenu: true, //开启右键菜单 默认true
					isEnableInsertRow: true, //插入行
					isEnableDeleteRow: true, //删除行
					isEnableInsertColumn: true, //插入列
					isEnableDeleteColumn: true, //删除列
					isEnableMergeCell: true, //合并单元格
					columns: [
						[
							{ title: '序号', align: 'center', field: 'sequence', width: 150 },
							{ title: '产品代码', align: 'center', field: 'sku', width: 150 },
							{ title: '产品名称', align: 'center', field: 'goodsName', width: 150 },
							{ title: '装箱数量', align: 'center', field: 'qty', width: 150 },
							{ title: '单件箱数', align: 'center', field: 'singelBoxQty', width: 150 },
							{ title: '总箱数', align: 'center', field: 'totalQty', width: 150 },
							{ title: '父件SKU', align: 'center', field: 'parentSku', width: 150 },
							{ title: '备注', align: 'center', field: 'remark', width: 150 },
						],
					],
				}, {
					tid: 'HachProviderDGJOBModule.table',
					title: 'Hach危险品仓JOB汇总清单',
					type: 'table',
					options: {
						field: 'table',
						tableHeaderRepeat: 'first',
						tableFooterRepeat: 'last',
						fields: [
							{ text: '序号', field: 'sequence' },
							{ text: '出库日期', field: 'completeTime' },
							{ text: '合同号', field: 'poCode' },
							{ text: '箱号', field: 'packageNumber' },
							{ text: '货号', field: 'sku' },
							{ text: '出库数量', field: 'qty' },
							{ text: '组合箱数', field: 'combinedBoxesNumber' },
							{ text: '类型', field: 'type' },
							{ text: 'JOB总箱', field: 'jobTotalBox' },
						],
					},
					editable: true,
					columnDisplayEditable: true, //列显示是否能编辑
					columnDisplayIndexEditable: true, //列顺序显示是否能编辑
					columnTitleEditable: true, //列标题是否能编辑
					columnResizable: true, //列宽是否能调整
					columnAlignEditable: true, //列对齐是否调整
					isEnableEditField: true, //编辑字段
					isEnableContextMenu: true, //开启右键菜单 默认true
					isEnableInsertRow: true, //插入行
					isEnableDeleteRow: true, //删除行
					isEnableInsertColumn: true, //插入列
					isEnableDeleteColumn: true, //删除列
					isEnableMergeCell: true, //合并单元格
					columns: [
						[
							{ title: '序号', align: 'center', field: 'sequence', width: 150 },
							{ title: '出库日期', align: 'center', field: 'completeTime', width: 200 },
							{ title: '合同号', align: 'center', field: 'poCode', width: 200 },
							{ title: '箱号', align: 'center', field: 'packageNumber', width: 200 },
							{ title: '货号', align: 'center', field: 'sku', width: 150 },
							{ title: '出库数量', align: 'center', field: 'qty', width: 110 },
							{ title: '组合箱数', align: 'center', field: 'combinedBoxesNumber', width: 110 },
							{ title: '类型', align: 'center', field: 'type', width: 150 },
							{ title: 'JOB总箱', align: 'center', field: 'jobTotalBox', width: 110 }
						],
					],
				}
			]),
			new hiprint.PrintElementTypeGroup('【视图字段】', [
				{
					tid: 'aProviderModule.creater',
					title: '制表人',
					data: 'Admin.NET',
					type: 'text',
					options: {
						field: 'creater',
						testData: 'Admin.NET',
						height: 16,
						fontSize: 6.75,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
					},
				},
				{
					tid: 'aProviderModule.printDate',
					title: '打印时间',
					data: '2023-07-20 09:00',
					type: 'text',
					options: {
						field: 'printDate',
						testData: '2023-07-20 09:00',
						height: 16,
						fontSize: 6.75,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
					},
				},
				{
					tid: 'aProviderModule.signer',
					title: '库管签字',
					data: 'Admin.NET',
					type: 'text',
					options: {
						field: 'signer',
						testData: 'Admin.NET',
						height: 16,
						fontSize: 6.75,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
					},
				},
				{
					tid: 'aProviderModule.director',
					title: '经理签字',
					data: 'Admin.NET',
					type: 'text',
					options: {
						field: 'director',
						testData: 'Admin.NET',
						height: 16,
						fontSize: 6.75,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
					},
				},
			]),
		]);
	};
	return {
		addElementTypes: addElementTypes,
	};
};

// 自定义设计元素2
export const bProvider = function () {
	var addElementTypes = function (context: any) {
		context.removePrintElementTypes('bProviderModule');
		context.addPrintElementTypes('bProviderModule', [
			new hiprint.PrintElementTypeGroup('【常规】', [
				{
					tid: 'bProviderModule.header',
					title: '单据表头',
					data: '单据表头',
					type: 'text',
					options: {
						testData: '单据表头',
						height: 17,
						fontSize: 16.5,
						fontWeight: '700',
						textAlign: 'center',
						hideTitle: true,
					},
				},
				{
					tid: 'bProviderModule.type',
					title: '单据类型',
					data: '单据类型',
					type: 'text',
					options: {
						testData: '单据类型',
						height: 16,
						fontSize: 15,
						fontWeight: '700',
						textAlign: 'center',
						hideTitle: true,
					},
				},
				{
					tid: 'bProviderModule.order',
					title: '订单编号',
					data: 'ZUO18020030720',
					type: 'text',
					options: {
						field: 'orderId',
						testData: 'ZUO18020030720',
						height: 16,
						fontSize: 6.75,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
					},
				},
				{
					tid: 'bProviderModule.date',
					title: '业务日期',
					data: '2023-07-20',
					type: 'text',
					options: {
						field: 'date',
						testData: '2023-07-20',
						height: 16,
						fontSize: 6.75,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
					},
				},
				{
					tid: 'bProviderModule.barcode',
					title: '条形码',
					data: '18020030720',
					type: 'text',
					options: {
						testData: 'ZUO18020030720',
						height: 32,
						fontSize: 12,
						lineHeight: 18,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
						textType: 'barcode',
					},
				},
				{
					tid: 'bProviderModule.qrcode',
					title: '二维码',
					data: 'ZUO18020030720',
					type: 'text',
					options: {
						testData: 'ZUO18020030720',
						height: 64,
						width: 64,
						fontSize: 12,
						lineHeight: 18,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
						textType: 'qrcode',
					},
				},
				{
					tid: 'bProviderModule.platform',
					title: '平台名称',
					data: '平台名称',
					type: 'text',
					options: {
						testData: '平台名称',
						height: 17,
						fontSize: 16.5,
						fontWeight: '700',
						textAlign: 'center',
						hideTitle: true,
					},
				},
				{ tid: 'bProviderModule.image', title: 'Logo', data: logoImg, type: 'image' },
			]),
			new hiprint.PrintElementTypeGroup('【客户】', [
				{
					tid: 'bProviderModule.khname',
					title: '客户名称',
					data: '高级客户',
					type: 'text',
					options: {
						field: 'name',
						testData: '高级客户',
						height: 16,
						fontSize: 6.75,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
					},
				},
				{
					tid: 'bProviderModule.tel',
					title: '客户电话',
					data: '18020030720',
					type: 'text',
					options: {
						field: 'tel',
						testData: '18020030720',
						height: 16,
						fontSize: 6.75,
						fontWeight: '700',
						textAlign: 'left',
						textContentVerticalAlign: 'middle',
					},
				},
			]),
			new hiprint.PrintElementTypeGroup('【表格/其他】', [
				{
					tid: 'bProviderModule.table',
					title: '订单数据',
					type: 'table',
					options: {
						field: 'table',
						fields: [
							{ text: '名称', field: 'NAME' },
							{ text: '数量', field: 'SL' },
							{ text: '规格', field: 'GG' },
							{ text: '条码', field: 'TM' },
							{ text: '单价', field: 'DJ' },
							{ text: '金额', field: 'JE' },
							{ text: '备注', field: 'DETAIL' },
						],
					},
					editable: true,
					columnDisplayEditable: true, //列显示是否能编辑
					columnDisplayIndexEditable: true, //列顺序显示是否能编辑
					columnTitleEditable: true, //列标题是否能编辑
					columnResizable: true, //列宽是否能调整
					columnAlignEditable: true, //列对齐是否调整
					columns: [
						[
							{ title: '名称', align: 'center', field: 'NAME', width: 100 },
							{ title: '数量', align: 'center', field: 'SL', width: 100 },
							{ title: '条码', align: 'center', field: 'TM', width: 100 },
							{ title: '规格', align: 'center', field: 'GG', width: 100 },
							{ title: '单价', align: 'center', field: 'DJ', width: 100 },
							{ title: '金额', align: 'center', field: 'JE', width: 100 },
							{ title: '备注', align: 'center', field: 'DETAIL', width: 100 },
						],
					],
					// footerFormatter: function (options: unknown, rows: unknown, data: any, currentPageGridRowsData: unknown) {
					//   if (data && data['totalCap']) {
					//     return `<td style="padding:0 10px" colspan="100">${'应收金额大写: ' + data['totalCap']}</td>`
					//   }
					//   return '<td style="padding:0 10px" colspan="100">应收金额大写: </td>'
					// },
				},
				{ tid: 'bProviderModule.customText', title: '文本', customText: '自定义文本', custom: true, type: 'text' },
				{
					tid: 'bProviderModule.longText',
					title: '长文本',
					type: 'longText',
					options: {
						field: 'test.longText',
						width: 200,
						testData: '长文本分页/不分页测试',
					},
				},
			]),
			new hiprint.PrintElementTypeGroup('【辅助】', [
				{
					tid: 'bProviderModule.hline',
					title: '横线',
					type: 'hline',
				},
				{
					tid: 'bProviderModule.vline',
					title: '竖线',
					type: 'vline',
				},
				{
					tid: 'bProviderModule.rect',
					title: '矩形',
					type: 'rect',
				},
				{
					tid: 'bProviderModule.oval',
					title: '椭圆',
					type: 'oval',
				},
			]),
		]);
	};
	return {
		addElementTypes: addElementTypes,
	};
};

// type: 1供货商 2经销商
export default [
	{
		name: 'A设计',
		value: 'aProviderModule',
		type: 1,
		f: aProvider(),
	},
	{
		name: 'B设计',
		value: 'bProviderModule',
		type: 2,
		f: bProvider(),
	},
];
