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
 * @interface WmsProduct
 */
export interface WmsProduct {

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    id: number;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    customerId?: number | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    supplierId?: number | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    supplierCode?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    status?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    enabledFlag?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    kitFlag?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    organizationId?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    itemId?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    sku?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    skuName?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    itemNameEn?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    type?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    skuClassification?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    skuGroup?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    shortNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    skuReplaced?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    receivingRoutingId?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    planningMakeBuyCodeMir?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    boxGroup?: string | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    price?: number | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    volume?: number | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    weight?: number | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    length?: number | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    height?: number | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    width?: number | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    skuFamily?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    customerChartNumber?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    unit?: string | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    unitTransFactor?: string | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    isValidityTermAd: number;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    isBatchAd: string;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    isSeqAd: number;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    isQa: number;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    isOffShelf: number;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    isSort: number;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    isDelivery: number;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    isOrderPull: number;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    isKbPull: number;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    isMarket: number;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    inboundType?: string | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    minCount?: number | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    maxCount?: number | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    remark?: string | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    createUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    createUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsProduct
     */
    createTime?: Date | null;

    /**
     * @type {number}
     * @memberof WmsProduct
     */
    updateUserId?: number | null;

    /**
     * @type {string}
     * @memberof WmsProduct
     */
    updateUserName?: string | null;

    /**
     * @type {Date}
     * @memberof WmsProduct
     */
    updateTime?: Date | null;
}