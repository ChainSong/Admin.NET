/* tslint:disable */
/* eslint-disable */
/**
 * HiGenious 通用权限开发平台
 * 让 .NET 开发更简单、更通用、更流行。前后端分离架构(.NET6/Vue3)，开箱即用紧随前沿技术。<br/><a href='https://gitee.com/zuohuaijun/HiGenious/'>https://gitee.com/zuohuaijun/HiGenious</a>
 *
 * OpenAPI spec version: 1.0.0
 * Contact: 515096995@qq.com
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */

import { WmsReceiptInfoOutput } from './wms-receipt-info-output';
 /**
 * 全局返回结果
 *
 * @export
 * @interface AdminResultWmsReceiptInfoOutput
 */
export interface AdminResultWmsReceiptInfoOutput {

    /**
     * 状态码
     *
     * @type {number}
     * @memberof AdminResultWmsReceiptInfoOutput
     */
    code?: number;

    /**
     * 类型success、warning、error
     *
     * @type {string}
     * @memberof AdminResultWmsReceiptInfoOutput
     */
    type?: string | null;

    /**
     * 错误信息
     *
     * @type {string}
     * @memberof AdminResultWmsReceiptInfoOutput
     */
    message?: string | null;

    /**
     * @type {WmsReceiptInfoOutput}
     * @memberof AdminResultWmsReceiptInfoOutput
     */
    result?: WmsReceiptInfoOutput;

    /**
     * 附加数据
     *
     * @type {any}
     * @memberof AdminResultWmsReceiptInfoOutput
     */
    extras?: any | null;

    /**
     * 时间
     *
     * @type {Date}
     * @memberof AdminResultWmsReceiptInfoOutput
     */
    time?: Date;
}