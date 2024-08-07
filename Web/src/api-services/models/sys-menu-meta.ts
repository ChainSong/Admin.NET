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
 * 菜单Meta配置
 *
 * @export
 * @interface SysMenuMeta
 */
export interface SysMenuMeta {

    /**
     * 标题
     *
     * @type {string}
     * @memberof SysMenuMeta
     */
    title?: string | null;

    /**
     * 图标
     *
     * @type {string}
     * @memberof SysMenuMeta
     */
    icon?: string | null;

    /**
     * 是否内嵌
     *
     * @type {boolean}
     * @memberof SysMenuMeta
     */
    isIframe?: boolean;

    /**
     * 外链链接
     *
     * @type {string}
     * @memberof SysMenuMeta
     */
    isLink?: string | null;

    /**
     * 是否隐藏
     *
     * @type {boolean}
     * @memberof SysMenuMeta
     */
    isHide?: boolean;

    /**
     * 是否缓存
     *
     * @type {boolean}
     * @memberof SysMenuMeta
     */
    isKeepAlive?: boolean;

    /**
     * 是否固定
     *
     * @type {boolean}
     * @memberof SysMenuMeta
     */
    isAffix?: boolean;
}
