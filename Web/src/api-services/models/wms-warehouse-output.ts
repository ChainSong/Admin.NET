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
 * @interface WmsWarehouseOutput
 */
export interface WmsWarehouseOutput {

    /**
     * @type {number}
     * @memberof WmsWarehouseOutput
     */
    id?: number;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    code?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    name?: string | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseOutput
     */
    operationArea?: number | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseOutput
     */
    storageArea?: number | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseOutput
     */
    totalArea?: number | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseOutput
     */
    status?: number | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    type?: string | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseOutput
     */
    isLpn?: number | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    description?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    customerId?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    creditLine?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    address?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    province?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    city?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    postcode?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    contactPerson?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    mobileNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    fax?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    email?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    remark?: string | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseOutput
     */
    createUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    createUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsWarehouseOutput
     */
    createTime?: Date | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseOutput
     */
    updateUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    updateUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsWarehouseOutput
     */
    updateTime?: Date | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseOutput
     */
    customerName?: string | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseOutput
     */
    pageSize?: number;

    /**
     * @type {number}
     * @memberof WmsWarehouseOutput
     */
    page?: number;
}