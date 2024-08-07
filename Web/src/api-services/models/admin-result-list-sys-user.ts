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

import { SysUser } from './sys-user';
 /**
 * 全局返回结果
 *
 * @export
 * @interface AdminResultListSysUser
 */
export interface AdminResultListSysUser {

    /**
     * 状态码
     *
     * @type {number}
     * @memberof AdminResultListSysUser
     */
    code?: number;

    /**
     * 类型success、warning、error
     *
     * @type {string}
     * @memberof AdminResultListSysUser
     */
    type?: string | null;

    /**
     * 错误信息
     *
     * @type {string}
     * @memberof AdminResultListSysUser
     */
    message?: string | null;

    /**
     * 数据
     *
     * @type {Array<SysUser>}
     * @memberof AdminResultListSysUser
     */
    result?: Array<SysUser> | null;

    /**
     * 附加数据
     *
     * @type {any}
     * @memberof AdminResultListSysUser
     */
    extras?: any | null;

    /**
     * 时间
     *
     * @type {Date}
     * @memberof AdminResultListSysUser
     */
    time?: Date;
}
