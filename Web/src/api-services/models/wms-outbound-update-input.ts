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
 * @interface WmsOutboundUpdateInput
 */
export interface WmsOutboundUpdateInput {

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    sysOutboundNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    externOrderNumber?: string | null;

    /**
     * @type {number}
     * @memberof WmsOutboundUpdateInput
     */
    customerId?: number | null;

    /**
     * @type {number}
     * @memberof WmsOutboundUpdateInput
     */
    warehouseId?: number | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    warehouseCode?: string | null;

    /**
     * @type {number}
     * @memberof WmsOutboundUpdateInput
     */
    targetWarehouseId?: number | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    targetWarehouseCode?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    type?: string | null;

    /**
     * @type {number}
     * @memberof WmsOutboundUpdateInput
     */
    status?: number | null;

    /**
     * @type {Date}
     * @memberof WmsOutboundUpdateInput
     */
    orderTime?: Date | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    orgId?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    province?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    city?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    district?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    address?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    contactPerson?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    mobileNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    logisticsName?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    vehicleNumber?: string | null;

    /**
     * @type {number}
     * @memberof WmsOutboundUpdateInput
     */
    detailCount?: number | null;

    /**
     * @type {number}
     * @memberof WmsOutboundUpdateInput
     */
    ownerId?: number | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    customerCode?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    customerName?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    lineCode?: string | null;

    /**
     * @type {Date}
     * @memberof WmsOutboundUpdateInput
     */
    sendDate?: Date | null;

    /**
     * @type {Date}
     * @memberof WmsOutboundUpdateInput
     */
    arriveDate?: Date | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    remark?: string | null;

    /**
     * @type {number}
     * @memberof WmsOutboundUpdateInput
     */
    createUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    createUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsOutboundUpdateInput
     */
    createTime?: Date | null;

    /**
     * @type {number}
     * @memberof WmsOutboundUpdateInput
     */
    updateUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsOutboundUpdateInput
     */
    updateUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsOutboundUpdateInput
     */
    updateTime?: Date | null;

    /**
     * @type {number}
     * @memberof WmsOutboundUpdateInput
     */
    id: number;
}
