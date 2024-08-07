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

import { MenuTypeEnum } from './menu-type-enum';
import { StatusEnum } from './status-enum';
import { SysMenu } from './sys-menu';
 /**
 * 
 *
 * @export
 * @interface AddMenuInput
 */
export interface AddMenuInput {

    /**
     * 雪花Id
     *
     * @type {number}
     * @memberof AddMenuInput
     */
    id?: number;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof AddMenuInput
     */
    createTime?: Date | null;

    /**
     * 更新时间
     *
     * @type {Date}
     * @memberof AddMenuInput
     */
    updateTime?: Date | null;

    /**
     * 创建者Id
     *
     * @type {number}
     * @memberof AddMenuInput
     */
    createUserId?: number | null;

    /**
     * 创建者姓名
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    createUserName?: string | null;

    /**
     * 修改者Id
     *
     * @type {number}
     * @memberof AddMenuInput
     */
    updateUserId?: number | null;

    /**
     * 修改者姓名
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    updateUserName?: string | null;

    /**
     * 软删除
     *
     * @type {boolean}
     * @memberof AddMenuInput
     */
    isDelete?: boolean;

    /**
     * 父Id
     *
     * @type {number}
     * @memberof AddMenuInput
     */
    pid?: number;

    /**
     * @type {MenuTypeEnum}
     * @memberof AddMenuInput
     */
    type?: MenuTypeEnum;

    /**
     * 路由名称
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    name?: string | null;

    /**
     * 路由地址
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    path?: string | null;

    /**
     * 组件路径
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    component?: string | null;

    /**
     * 重定向
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    redirect?: string | null;

    /**
     * 权限标识
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    permission?: string | null;

    /**
     * 图标
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    icon?: string | null;

    /**
     * 是否内嵌
     *
     * @type {boolean}
     * @memberof AddMenuInput
     */
    isIframe?: boolean;

    /**
     * 外链链接
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    outLink?: string | null;

    /**
     * 是否隐藏
     *
     * @type {boolean}
     * @memberof AddMenuInput
     */
    isHide?: boolean;

    /**
     * 是否缓存
     *
     * @type {boolean}
     * @memberof AddMenuInput
     */
    isKeepAlive?: boolean;

    /**
     * 是否固定
     *
     * @type {boolean}
     * @memberof AddMenuInput
     */
    isAffix?: boolean;

    /**
     * 排序
     *
     * @type {number}
     * @memberof AddMenuInput
     */
    orderNo?: number;

    /**
     * @type {StatusEnum}
     * @memberof AddMenuInput
     */
    status?: StatusEnum;

    /**
     * 备注
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    remark?: string | null;

    /**
     * 菜单子项
     *
     * @type {Array<SysMenu>}
     * @memberof AddMenuInput
     */
    children?: Array<SysMenu> | null;

    /**
     * 名称
     *
     * @type {string}
     * @memberof AddMenuInput
     */
    title: string;
}
