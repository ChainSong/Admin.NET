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
 * @interface WmsCustomerInput
 */
export interface WmsCustomerInput {

    /**
     * @type {number}
     * @memberof WmsCustomerInput
     */
    id?: number;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    code?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    name?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    shortName?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    legalPerson?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    bank?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    account?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    taxId?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    invoiceTitle?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    province?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    city?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    address?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    postcode?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    description?: string | null;

    /**
     * @type {number}
     * @memberof WmsCustomerInput
     */
    status?: number | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    type?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    creditLine?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    contactPerson?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    mobileNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    email?: string | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    remark?: string | null;

    /**
     * @type {number}
     * @memberof WmsCustomerInput
     */
    createUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    createUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsCustomerInput
     */
    createTime?: Date | null;

    /**
     * @type {Array<Date>}
     * @memberof WmsCustomerInput
     */
    createTimeRange?: Array<Date> | null;

    /**
     * @type {number}
     * @memberof WmsCustomerInput
     */
    updateUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsCustomerInput
     */
    updateUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsCustomerInput
     */
    updateTime?: Date | null;

    /**
     * @type {Array<Date>}
     * @memberof WmsCustomerInput
     */
    updateTimeRange?: Array<Date> | null;
}