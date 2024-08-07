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
 * @interface UpdatePosInput
 */
export interface UpdatePosInput {

    /**
     * 雪花Id
     *
     * @type {number}
     * @memberof UpdatePosInput
     */
    id?: number;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof UpdatePosInput
     */
    createTime?: Date | null;

    /**
     * 更新时间
     *
     * @type {Date}
     * @memberof UpdatePosInput
     */
    updateTime?: Date | null;

    /**
     * 创建者Id
     *
     * @type {number}
     * @memberof UpdatePosInput
     */
    createUserId?: number | null;

    /**
     * 创建者姓名
     *
     * @type {string}
     * @memberof UpdatePosInput
     */
    createUserName?: string | null;

    /**
     * 修改者Id
     *
     * @type {number}
     * @memberof UpdatePosInput
     */
    updateUserId?: number | null;

    /**
     * 修改者姓名
     *
     * @type {string}
     * @memberof UpdatePosInput
     */
    updateUserName?: string | null;

    /**
     * 软删除
     *
     * @type {boolean}
     * @memberof UpdatePosInput
     */
    isDelete?: boolean;

    /**
     * 租户Id
     *
     * @type {number}
     * @memberof UpdatePosInput
     */
    tenantId?: number | null;

    /**
     * 编码
     *
     * @type {string}
     * @memberof UpdatePosInput
     */
    code?: string | null;

    /**
     * 排序
     *
     * @type {number}
     * @memberof UpdatePosInput
     */
    orderNo?: number;

    /**
     * 备注
     *
     * @type {string}
     * @memberof UpdatePosInput
     */
    remark?: string | null;

    /**
     * @type {StatusEnum}
     * @memberof UpdatePosInput
     */
    status?: StatusEnum;

    /**
     * 名称
     *
     * @type {string}
     * @memberof UpdatePosInput
     */
    name: string;
}
