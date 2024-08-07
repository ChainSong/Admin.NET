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

 /**
 * 
 *
 * @export
 * @interface WmsCheckDetailInput
 */
export interface WmsCheckDetailInput {

    /**
     * 主键Id
     *
     * @type {number}
     * @memberof WmsCheckDetailInput
     */
    id: number;

    /**
     * @type {number}
     * @memberof WmsCheckDetailInput
     */
    checkid?: number | null;

    /**
     * @type {string}
     * @memberof WmsCheckDetailInput
     */
    checkNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsCheckDetailInput
     */
    sku?: string | null;

    /**
     * @type {string}
     * @memberof WmsCheckDetailInput
     */
    lpn?: string | null;

    /**
     * @type {number}
     * @memberof WmsCheckDetailInput
     */
    isDifference?: number | null;

    /**
     * @type {number}
     * @memberof WmsCheckDetailInput
     */
    isDeal?: number | null;
}
