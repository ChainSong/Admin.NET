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
 * @interface WmsWarehouseLocationPageListInput
 */
export interface WmsWarehouseLocationPageListInput {

    /**
     * 当前页码
     *
     * @type {number}
     * @memberof WmsWarehouseLocationPageListInput
     */
    page?: number;

    /**
     * 页码容量
     *
     * @type {number}
     * @memberof WmsWarehouseLocationPageListInput
     */
    pageSize?: number;

    /**
     * 排序字段
     *
     * @type {string}
     * @memberof WmsWarehouseLocationPageListInput
     */
    field?: string | null;

    /**
     * 排序方向
     *
     * @type {string}
     * @memberof WmsWarehouseLocationPageListInput
     */
    order?: string | null;

    /**
     * 降序排序
     *
     * @type {string}
     * @memberof WmsWarehouseLocationPageListInput
     */
    descStr?: string | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseLocationPageListInput
     */
    id?: number | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseLocationPageListInput
     */
    name?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseLocationPageListInput
     */
    code?: string | null;

    /**
     * @type {string}
     * @memberof WmsWarehouseLocationPageListInput
     */
    type?: string | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseLocationPageListInput
     */
    status?: number | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseLocationPageListInput
     */
    isMultiSku?: number | null;

    /**
     * @type {number}
     * @memberof WmsWarehouseLocationPageListInput
     */
    areaId?: number | null;
}