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

import { StatusEnum } from './status-enum';
 /**
 * 
 *
 * @export
 * @interface AddPosInput
 */
export interface AddPosInput {

    /**
     * 雪花Id
     *
     * @type {number}
     * @memberof AddPosInput
     */
    id?: number;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof AddPosInput
     */
    createTime?: Date | null;

    /**
     * 更新时间
     *
     * @type {Date}
     * @memberof AddPosInput
     */
    updateTime?: Date | null;

    /**
     * 创建者Id
     *
     * @type {number}
     * @memberof AddPosInput
     */
    createUserId?: number | null;

    /**
     * 创建者姓名
     *
     * @type {string}
     * @memberof AddPosInput
     */
    createUserName?: string | null;

    /**
     * 修改者Id
     *
     * @type {number}
     * @memberof AddPosInput
     */
    updateUserId?: number | null;

    /**
     * 修改者姓名
     *
     * @type {string}
     * @memberof AddPosInput
     */
    updateUserName?: string | null;

    /**
     * 软删除
     *
     * @type {boolean}
     * @memberof AddPosInput
     */
    isDelete?: boolean;

    /**
     * 租户Id
     *
     * @type {number}
     * @memberof AddPosInput
     */
    tenantId?: number | null;

    /**
     * 编码
     *
     * @type {string}
     * @memberof AddPosInput
     */
    code?: string | null;

    /**
     * 排序
     *
     * @type {number}
     * @memberof AddPosInput
     */
    orderNo?: number;

    /**
     * 备注
     *
     * @type {string}
     * @memberof AddPosInput
     */
    remark?: string | null;

    /**
     * @type {StatusEnum}
     * @memberof AddPosInput
     */
    status?: StatusEnum;

    /**
     * 名称
     *
     * @type {string}
     * @memberof AddPosInput
     */
    name: string;
}
