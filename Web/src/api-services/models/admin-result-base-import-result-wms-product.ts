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

import { BaseImportResultWmsProduct } from './base-import-result-wms-product';
 /**
 * 全局返回结果
 *
 * @export
 * @interface AdminResultBaseImportResultWmsProduct
 */
export interface AdminResultBaseImportResultWmsProduct {

    /**
     * 状态码
     *
     * @type {number}
     * @memberof AdminResultBaseImportResultWmsProduct
     */
    code?: number;

    /**
     * 类型success、warning、error
     *
     * @type {string}
     * @memberof AdminResultBaseImportResultWmsProduct
     */
    type?: string | null;

    /**
     * 错误信息
     *
     * @type {string}
     * @memberof AdminResultBaseImportResultWmsProduct
     */
    message?: string | null;

    /**
     * @type {BaseImportResultWmsProduct}
     * @memberof AdminResultBaseImportResultWmsProduct
     */
    result?: BaseImportResultWmsProduct;

    /**
     * 附加数据
     *
     * @type {any}
     * @memberof AdminResultBaseImportResultWmsProduct
     */
    extras?: any | null;

    /**
     * 时间
     *
     * @type {Date}
     * @memberof AdminResultBaseImportResultWmsProduct
     */
    time?: Date;
}
