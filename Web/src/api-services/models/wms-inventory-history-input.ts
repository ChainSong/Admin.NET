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
 * @interface WmsInventoryHistoryInput
 */
export interface WmsInventoryHistoryInput {

    /**
     * 当前页码
     *
     * @type {number}
     * @memberof WmsInventoryHistoryInput
     */
    page?: number;

    /**
     * 页码容量
     *
     * @type {number}
     * @memberof WmsInventoryHistoryInput
     */
    pageSize?: number;

    /**
     * 排序字段
     *
     * @type {string}
     * @memberof WmsInventoryHistoryInput
     */
    field?: string | null;

    /**
     * 排序方向
     *
     * @type {string}
     * @memberof WmsInventoryHistoryInput
     */
    order?: string | null;

    /**
     * 降序排序
     *
     * @type {string}
     * @memberof WmsInventoryHistoryInput
     */
    descStr?: string | null;

    /**
     * @type {number}
     * @memberof WmsInventoryHistoryInput
     */
    id?: number;

    /**
     * @type {number}
     * @memberof WmsInventoryHistoryInput
     */
    customerId?: number | null;

    /**
     * @type {number}
     * @memberof WmsInventoryHistoryInput
     */
    warehouseId?: number | null;

    /**
     * @type {string}
     * @memberof WmsInventoryHistoryInput
     */
    warehouseName?: string | null;

    /**
     * @type {number}
     * @memberof WmsInventoryHistoryInput
     */
    warehouseAreaId?: number | null;

    /**
     * @type {string}
     * @memberof WmsInventoryHistoryInput
     */
    warehouseAreaCode?: string | null;

    /**
     * @type {string}
     * @memberof WmsInventoryHistoryInput
     */
    warehouseAreaName?: string | null;

    /**
     * @type {number}
     * @memberof WmsInventoryHistoryInput
     */
    warehouseLocationId?: number | null;

    /**
     * @type {string}
     * @memberof WmsInventoryHistoryInput
     */
    warehouseLocationCode?: string | null;

    /**
     * @type {string}
     * @memberof WmsInventoryHistoryInput
     */
    warehouseLocationName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsInventoryHistoryInput
     */
    createTime?: Date | null;
}
