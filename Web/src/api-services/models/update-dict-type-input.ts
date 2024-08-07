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
import { SysDictData } from './sys-dict-data';
 /**
 * 
 *
 * @export
 * @interface UpdateDictTypeInput
 */
export interface UpdateDictTypeInput {

    /**
     * 雪花Id
     *
     * @type {number}
     * @memberof UpdateDictTypeInput
     */
    id?: number;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof UpdateDictTypeInput
     */
    createTime?: Date | null;

    /**
     * 更新时间
     *
     * @type {Date}
     * @memberof UpdateDictTypeInput
     */
    updateTime?: Date | null;

    /**
     * 创建者Id
     *
     * @type {number}
     * @memberof UpdateDictTypeInput
     */
    createUserId?: number | null;

    /**
     * 创建者姓名
     *
     * @type {string}
     * @memberof UpdateDictTypeInput
     */
    createUserName?: string | null;

    /**
     * 修改者Id
     *
     * @type {number}
     * @memberof UpdateDictTypeInput
     */
    updateUserId?: number | null;

    /**
     * 修改者姓名
     *
     * @type {string}
     * @memberof UpdateDictTypeInput
     */
    updateUserName?: string | null;

    /**
     * 软删除
     *
     * @type {boolean}
     * @memberof UpdateDictTypeInput
     */
    isDelete?: boolean;

    /**
     * 名称
     *
     * @type {string}
     * @memberof UpdateDictTypeInput
     */
    name: string;

    /**
     * 编码
     *
     * @type {string}
     * @memberof UpdateDictTypeInput
     */
    code: string;

    /**
     * 排序
     *
     * @type {number}
     * @memberof UpdateDictTypeInput
     */
    orderNo?: number;

    /**
     * 备注
     *
     * @type {string}
     * @memberof UpdateDictTypeInput
     */
    remark?: string | null;

    /**
     * @type {StatusEnum}
     * @memberof UpdateDictTypeInput
     */
    status?: StatusEnum;

    /**
     * 字典值集合
     *
     * @type {Array<SysDictData>}
     * @memberof UpdateDictTypeInput
     */
    children?: Array<SysDictData> | null;
}
