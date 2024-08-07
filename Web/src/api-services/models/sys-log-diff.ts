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
 * 系统差异日志表
 *
 * @export
 * @interface SysLogDiff
 */
export interface SysLogDiff {

    /**
     * 雪花Id
     *
     * @type {number}
     * @memberof SysLogDiff
     */
    id?: number;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof SysLogDiff
     */
    createTime?: Date | null;

    /**
     * 更新时间
     *
     * @type {Date}
     * @memberof SysLogDiff
     */
    updateTime?: Date | null;

    /**
     * 创建者Id
     *
     * @type {number}
     * @memberof SysLogDiff
     */
    createUserId?: number | null;

    /**
     * 创建者姓名
     *
     * @type {string}
     * @memberof SysLogDiff
     */
    createUserName?: string | null;

    /**
     * 修改者Id
     *
     * @type {number}
     * @memberof SysLogDiff
     */
    updateUserId?: number | null;

    /**
     * 修改者姓名
     *
     * @type {string}
     * @memberof SysLogDiff
     */
    updateUserName?: string | null;

    /**
     * 软删除
     *
     * @type {boolean}
     * @memberof SysLogDiff
     */
    isDelete?: boolean;

    /**
     * 操作前记录
     *
     * @type {string}
     * @memberof SysLogDiff
     */
    beforeData?: string | null;

    /**
     * 操作后记录
     *
     * @type {string}
     * @memberof SysLogDiff
     */
    afterData?: string | null;

    /**
     * Sql
     *
     * @type {string}
     * @memberof SysLogDiff
     */
    sql?: string | null;

    /**
     * 参数  手动传入的参数
     *
     * @type {string}
     * @memberof SysLogDiff
     */
    parameters?: string | null;

    /**
     * 业务对象
     *
     * @type {string}
     * @memberof SysLogDiff
     */
    businessData?: string | null;

    /**
     * 差异操作
     *
     * @type {string}
     * @memberof SysLogDiff
     */
    diffType?: string | null;

    /**
     * 耗时
     *
     * @type {number}
     * @memberof SysLogDiff
     */
    elapsed?: number | null;
}
