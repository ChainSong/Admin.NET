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
 * @interface WmsWarehouseAreaListOutput
 */
export interface WmsWarehouseAreaListOutput {

    /**
     * @type {number}
     * @memberof WmsWarehouseAreaListOutput
     */
    id?: number;

    /**
     * @type {number}
     * @memberof WmsWarehouseAreaListOutput
     */
    warehouseId?: number | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseAreaListOutput
     */
    code?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseAreaListOutput
     */
    name?: string | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseAreaListOutput
     */
    status?: number | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseAreaListOutput
     */
    type?: string | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseAreaListOutput
     */
    isOneOne?: number;

    /**
     * @type {number}
     * @memberof WmsWarehouseAreaListOutput
     */
    isQa?: number;

    /**
     * @type {string}
     * @memberof WmsWarehouseAreaListOutput
     */
    remark?: string | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseAreaListOutput
     */
    createUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseAreaListOutput
     */
    createUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsWarehouseAreaListOutput
     */
    createTime?: Date | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseAreaListOutput
     */
    updateUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseAreaListOutput
     */
    updateUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsWarehouseAreaListOutput
     */
    updateTime?: Date | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseAreaListOutput
     */
    warehouseName?: string | null;
}