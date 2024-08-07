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
 * 用户登录参数
 *
 * @export
 * @interface LoginInput
 */
export interface LoginInput {

    /**
     * 账号
     *
     * @type {string}
     * @memberof LoginInput
     * @example admin
     */
    account: string;

    /**
     * 密码
     *
     * @type {string}
     * @memberof LoginInput
     * @example 123456
     */
    password: string;

    /**
     * 验证码Id
     *
     * @type {number}
     * @memberof LoginInput
     */
    codeId?: number;

    /**
     * 验证码
     *
     * @type {string}
     * @memberof LoginInput
     */
    code?: string | null;
}
