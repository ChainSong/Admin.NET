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
 * 
 *
 * @export
 * @interface WmsAsnDto
 */
export interface WmsAsnDto {

    /**
     * @type {number}
     * @memberof WmsAsnDto
     */
    id?: number;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    asnNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    sysNumber?: string | null;

    /**
     * @type {number}
     * @memberof WmsAsnDto
     */
    customerId?: number | null;

    /**
     * @type {number}
     * @memberof WmsAsnDto
     */
    warehouseId?: number | null;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    orgId?: string | null;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    vendorNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    vendorSiteCode?: string | null;

    /**
     * @type {Date}
     * @memberof WmsAsnDto
     */
    shippedDate?: Date | null;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    customerNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    customerName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsAsnDto
     */
    expectTime?: Date | null;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    batchNumber?: string | null;

    /**
     * @type {number}
     * @memberof WmsAsnDto
     */
    status?: number | null;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    asnType?: string | null;

    /**
     * @type {Date}
     * @memberof WmsAsnDto
     */
    completeTime?: Date | null;

    /**
     * @type {number}
     * @memberof WmsAsnDto
     */
    isConcession?: number;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    remark?: string | null;

    /**
     * @type {number}
     * @memberof WmsAsnDto
     */
    createUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    createUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsAsnDto
     */
    createTime?: Date | null;

    /**
     * @type {number}
     * @memberof WmsAsnDto
     */
    updateUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsAsnDto
     */
    updateUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsAsnDto
     */
    updateTime?: Date | null;
}
