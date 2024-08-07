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

import { WmsWorkOrderOutput } from './wms-work-order-output';
 /**
 * 全局返回结果
 *
 * @export
 * @interface AdminResultListWmsWorkOrderOutput
 */
export interface AdminResultListWmsWorkOrderOutput {

    /**
     * 状态码
     *
     * @type {number}
     * @memberof AdminResultListWmsWorkOrderOutput
     */
    code?: number;

    /**
     * 类型success、warning、error
     *
     * @type {string}
     * @memberof AdminResultListWmsWorkOrderOutput
     */
    type?: string | null;

    /**
     * 错误信息
     *
     * @type {string}
     * @memberof AdminResultListWmsWorkOrderOutput
     */
    message?: string | null;

    /**
     * 数据
     *
     * @type {Array<WmsWorkOrderOutput>}
     * @memberof AdminResultListWmsWorkOrderOutput
     */
    result?: Array<WmsWorkOrderOutput> | null;

    /**
     * 附加数据
     *
     * @type {any}
     * @memberof AdminResultListWmsWorkOrderOutput
     */
    extras?: any | null;

    /**
     * 时间
     *
     * @type {Date}
     * @memberof AdminResultListWmsWorkOrderOutput
     */
    time?: Date;
}
