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
 * @interface WmsWorkOrderUpdateInput
 */
export interface WmsWorkOrderUpdateInput {

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    id?: number;

    /**
     * @type {string}
     * @memberof WmsWorkOrderUpdateInput
     */
    workOrderNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsWorkOrderUpdateInput
     */
    sysNumber?: string | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    customerId?: number | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    warehouseId?: number | null;

    /**
     * @type {string}
     * @memberof WmsWorkOrderUpdateInput
     */
    parentProductCode?: string | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    qty?: number | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    oraCompleteQty?: number | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    wmsCompleteQty?: number | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    sortingStatus?: number | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    deliveryStatus?: number | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    isLack?: number | null;

    /**
     * @type {string}
     * @memberof WmsWorkOrderUpdateInput
     */
    lineNumber?: string | null;

    /**
     * @type {Date}
     * @memberof WmsWorkOrderUpdateInput
     */
    expectTime?: Date | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    status?: number | null;

    /**
     * @type {string}
     * @memberof WmsWorkOrderUpdateInput
     */
    type?: string | null;

    /**
     * @type {Date}
     * @memberof WmsWorkOrderUpdateInput
     */
    completeTime?: Date | null;

    /**
     * @type {string}
     * @memberof WmsWorkOrderUpdateInput
     */
    remark?: string | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    createUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsWorkOrderUpdateInput
     */
    createUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsWorkOrderUpdateInput
     */
    createTime?: Date | null;

    /**
     * @type {number}
     * @memberof WmsWorkOrderUpdateInput
     */
    updateUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsWorkOrderUpdateInput
     */
    updateUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsWorkOrderUpdateInput
     */
    updateTime?: Date | null;
}