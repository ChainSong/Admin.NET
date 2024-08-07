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

import { SysRegion } from './sys-region';
 /**
 * 系统行政地区表
 *
 * @export
 * @interface SysRegion
 */
export interface SysRegion {

    /**
     * 雪花Id
     *
     * @type {number}
     * @memberof SysRegion
     */
    id?: number;

    /**
     * 父Id
     *
     * @type {number}
     * @memberof SysRegion
     */
    pid?: number;

    /**
     * 名称
     *
     * @type {string}
     * @memberof SysRegion
     */
    name: string;

    /**
     * 简称
     *
     * @type {string}
     * @memberof SysRegion
     */
    shortName?: string | null;

    /**
     * 组合名
     *
     * @type {string}
     * @memberof SysRegion
     */
    mergerName?: string | null;

    /**
     * 行政代码
     *
     * @type {string}
     * @memberof SysRegion
     */
    code?: string | null;

    /**
     * 邮政编码
     *
     * @type {string}
     * @memberof SysRegion
     */
    zipCode?: string | null;

    /**
     * 区号
     *
     * @type {string}
     * @memberof SysRegion
     */
    cityCode?: string | null;

    /**
     * 层级
     *
     * @type {number}
     * @memberof SysRegion
     */
    level?: number;

    /**
     * 拼音
     *
     * @type {string}
     * @memberof SysRegion
     */
    pinYin?: string | null;

    /**
     * 经度
     *
     * @type {number}
     * @memberof SysRegion
     */
    lng?: number;

    /**
     * 维度
     *
     * @type {number}
     * @memberof SysRegion
     */
    lat?: number;

    /**
     * 排序
     *
     * @type {number}
     * @memberof SysRegion
     */
    orderNo?: number;

    /**
     * 备注
     *
     * @type {string}
     * @memberof SysRegion
     */
    remark?: string | null;

    /**
     * 机构子项
     *
     * @type {Array<SysRegion>}
     * @memberof SysRegion
     */
    children?: Array<SysRegion> | null;
}
