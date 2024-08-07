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

import { WmsCustomerDto } from './wms-customer-dto';
 /**
 * 分页泛型集合
 *
 * @export
 * @interface SqlSugarPagedListWmsCustomerDto
 */
export interface SqlSugarPagedListWmsCustomerDto {

    /**
     * 页码
     *
     * @type {number}
     * @memberof SqlSugarPagedListWmsCustomerDto
     */
    page?: number;

    /**
     * 页容量
     *
     * @type {number}
     * @memberof SqlSugarPagedListWmsCustomerDto
     */
    pageSize?: number;

    /**
     * 总条数
     *
     * @type {number}
     * @memberof SqlSugarPagedListWmsCustomerDto
     */
    total?: number;

    /**
     * 总页数
     *
     * @type {number}
     * @memberof SqlSugarPagedListWmsCustomerDto
     */
    totalPages?: number;

    /**
     * 当前页集合
     *
     * @type {Array<WmsCustomerDto>}
     * @memberof SqlSugarPagedListWmsCustomerDto
     */
    items?: Array<WmsCustomerDto> | null;

    /**
     * 是否有上一页
     *
     * @type {boolean}
     * @memberof SqlSugarPagedListWmsCustomerDto
     */
    hasPrevPage?: boolean;

    /**
     * 是否有下一页
     *
     * @type {boolean}
     * @memberof SqlSugarPagedListWmsCustomerDto
     */
    hasNextPage?: boolean;
}
