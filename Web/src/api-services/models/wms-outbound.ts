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

import { WmsOutboundAllocationDetail } from './wms-outbound-allocation-detail';
import { WmsOutboundDetail } from './wms-outbound-detail';
import { WmsOutboundVehicleDetail } from './wms-outbound-vehicle-detail';
 /**
 * 
 *
 * @export
 * @interface WmsOutbound
 */
export interface WmsOutbound {

    /**
     * @type {number}
     * @memberof WmsOutbound
     */
    id: number;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    sysOutboundNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    externOrderNumber?: string | null;

    /**
     * @type {number}
     * @memberof WmsOutbound
     */
    customerId?: number | null;

    /**
     * @type {number}
     * @memberof WmsOutbound
     */
    warehouseId?: number | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    warehouseCode?: string | null;

    /**
     * @type {number}
     * @memberof WmsOutbound
     */
    targetWarehouseId?: number | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    targetWarehouseCode?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    type?: string | null;

    /**
     * @type {number}
     * @memberof WmsOutbound
     */
    status?: number | null;

    /**
     * @type {Date}
     * @memberof WmsOutbound
     */
    orderTime?: Date | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    orgId?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    province?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    city?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    district?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    address?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    contactPerson?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    mobileNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    logisticsName?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    vehicleNumber?: string | null;

    /**
     * @type {number}
     * @memberof WmsOutbound
     */
    detailCount?: number | null;

    /**
     * @type {number}
     * @memberof WmsOutbound
     */
    ownerId?: number | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    customerCode?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    customerName?: string | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    lineCode?: string | null;

    /**
     * @type {Date}
     * @memberof WmsOutbound
     */
    sendDate?: Date | null;

    /**
     * @type {Date}
     * @memberof WmsOutbound
     */
    arriveDate?: Date | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    remark?: string | null;

    /**
     * @type {number}
     * @memberof WmsOutbound
     */
    createUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    createUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsOutbound
     */
    createTime?: Date | null;

    /**
     * @type {number}
     * @memberof WmsOutbound
     */
    updateUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsOutbound
     */
    updateUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsOutbound
     */
    updateTime?: Date | null;

    /**
     * @type {Array<WmsOutboundDetail>}
     * @memberof WmsOutbound
     */
    outboundDetail?: Array<WmsOutboundDetail> | null;

    /**
     * @type {Array<WmsOutboundAllocationDetail>}
     * @memberof WmsOutbound
     */
    outboundAllocationDetail?: Array<WmsOutboundAllocationDetail> | null;

    /**
     * @type {Array<WmsOutboundVehicleDetail>}
     * @memberof WmsOutbound
     */
    outboundVehicleDetail?: Array<WmsOutboundVehicleDetail> | null;
}
