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

import globalAxios, { AxiosResponse, AxiosInstance, AxiosRequestConfig } from 'axios';
import { Configuration } from '../configuration';
// Some imports not used depending on template conditions
// @ts-ignore
import { BASE_PATH, COLLECTION_FORMATS, RequestArgs, BaseAPI, RequiredError } from '../base';
import { AdminResultBoolean } from '../models';
import { AdminResultInt64 } from '../models';
import { AdminResultListWmsAsnDetail } from '../models';
import { AdminResultSqlSugarPagedListWmsAsnDto } from '../models';
import { AdminResultWmsAsn } from '../models';
import { WmsAsnAddInput } from '../models';
import { WmsAsnDeleteInput } from '../models';
import { WmsAsnInput } from '../models';
import { WmsAsnPageListInput } from '../models';
import { WmsAsnUpdateInput } from '../models';
/**
 * WmsAsnApi - axios parameter creator
 * @export
 */
export const WmsAsnApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * 
         * @summary 添加Asn
         * @param {WmsAsnAddInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiWmsAsnAddAsnPost: async (body?: WmsAsnAddInput, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/wmsAsn/addAsn`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required
            // http bearer authentication required
            if (configuration && configuration.accessToken) {
                const accessToken = typeof configuration.accessToken === 'function'
                    ? await configuration.accessToken()
                    : await configuration.accessToken;
                localVarHeaderParameter["Authorization"] = "Bearer " + accessToken;
            }

            localVarHeaderParameter['Content-Type'] = 'application/json-patch+json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @summary 查询Asn明细详情
         * @param {WmsAsnInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiWmsAsnAsnDetailInfoPost: async (body?: WmsAsnInput, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/wmsAsn/asnDetailInfo`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required
            // http bearer authentication required
            if (configuration && configuration.accessToken) {
                const accessToken = typeof configuration.accessToken === 'function'
                    ? await configuration.accessToken()
                    : await configuration.accessToken;
                localVarHeaderParameter["Authorization"] = "Bearer " + accessToken;
            }

            localVarHeaderParameter['Content-Type'] = 'application/json-patch+json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @summary 查询Asn详情
         * @param {WmsAsnInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiWmsAsnAsnInfoPost: async (body?: WmsAsnInput, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/wmsAsn/asnInfo`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required
            // http bearer authentication required
            if (configuration && configuration.accessToken) {
                const accessToken = typeof configuration.accessToken === 'function'
                    ? await configuration.accessToken()
                    : await configuration.accessToken;
                localVarHeaderParameter["Authorization"] = "Bearer " + accessToken;
            }

            localVarHeaderParameter['Content-Type'] = 'application/json-patch+json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @summary 查询Asn列表
         * @param {WmsAsnPageListInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiWmsAsnAsnPageListPost: async (body?: WmsAsnPageListInput, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/wmsAsn/asnPageList`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required
            // http bearer authentication required
            if (configuration && configuration.accessToken) {
                const accessToken = typeof configuration.accessToken === 'function'
                    ? await configuration.accessToken()
                    : await configuration.accessToken;
                localVarHeaderParameter["Authorization"] = "Bearer " + accessToken;
            }

            localVarHeaderParameter['Content-Type'] = 'application/json-patch+json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @summary 关闭Asn
         * @param {WmsAsnInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiWmsAsnCloseAsnPost: async (body?: WmsAsnInput, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/wmsAsn/closeAsn`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required
            // http bearer authentication required
            if (configuration && configuration.accessToken) {
                const accessToken = typeof configuration.accessToken === 'function'
                    ? await configuration.accessToken()
                    : await configuration.accessToken;
                localVarHeaderParameter["Authorization"] = "Bearer " + accessToken;
            }

            localVarHeaderParameter['Content-Type'] = 'application/json-patch+json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @summary 删除Asn
         * @param {WmsAsnDeleteInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiWmsAsnDeleteAsnPost: async (body?: WmsAsnDeleteInput, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/wmsAsn/deleteAsn`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required
            // http bearer authentication required
            if (configuration && configuration.accessToken) {
                const accessToken = typeof configuration.accessToken === 'function'
                    ? await configuration.accessToken()
                    : await configuration.accessToken;
                localVarHeaderParameter["Authorization"] = "Bearer " + accessToken;
            }

            localVarHeaderParameter['Content-Type'] = 'application/json-patch+json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @summary 更新Asn
         * @param {WmsAsnUpdateInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiWmsAsnUpdateAsnPost: async (body?: WmsAsnUpdateInput, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/wmsAsn/updateAsn`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            // authentication Bearer required
            // http bearer authentication required
            if (configuration && configuration.accessToken) {
                const accessToken = typeof configuration.accessToken === 'function'
                    ? await configuration.accessToken()
                    : await configuration.accessToken;
                localVarHeaderParameter["Authorization"] = "Bearer " + accessToken;
            }

            localVarHeaderParameter['Content-Type'] = 'application/json-patch+json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
    }
};

/**
 * WmsAsnApi - functional programming interface
 * @export
 */
export const WmsAsnApiFp = function(configuration?: Configuration) {
    return {
        /**
         * 
         * @summary 添加Asn
         * @param {WmsAsnAddInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnAddAsnPost(body?: WmsAsnAddInput, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultInt64>>> {
            const localVarAxiosArgs = await WmsAsnApiAxiosParamCreator(configuration).apiWmsAsnAddAsnPost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @summary 查询Asn明细详情
         * @param {WmsAsnInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnAsnDetailInfoPost(body?: WmsAsnInput, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultListWmsAsnDetail>>> {
            const localVarAxiosArgs = await WmsAsnApiAxiosParamCreator(configuration).apiWmsAsnAsnDetailInfoPost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @summary 查询Asn详情
         * @param {WmsAsnInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnAsnInfoPost(body?: WmsAsnInput, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultWmsAsn>>> {
            const localVarAxiosArgs = await WmsAsnApiAxiosParamCreator(configuration).apiWmsAsnAsnInfoPost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @summary 查询Asn列表
         * @param {WmsAsnPageListInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnAsnPageListPost(body?: WmsAsnPageListInput, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultSqlSugarPagedListWmsAsnDto>>> {
            const localVarAxiosArgs = await WmsAsnApiAxiosParamCreator(configuration).apiWmsAsnAsnPageListPost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @summary 关闭Asn
         * @param {WmsAsnInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnCloseAsnPost(body?: WmsAsnInput, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultBoolean>>> {
            const localVarAxiosArgs = await WmsAsnApiAxiosParamCreator(configuration).apiWmsAsnCloseAsnPost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @summary 删除Asn
         * @param {WmsAsnDeleteInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnDeleteAsnPost(body?: WmsAsnDeleteInput, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultBoolean>>> {
            const localVarAxiosArgs = await WmsAsnApiAxiosParamCreator(configuration).apiWmsAsnDeleteAsnPost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @summary 更新Asn
         * @param {WmsAsnUpdateInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnUpdateAsnPost(body?: WmsAsnUpdateInput, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<AdminResultInt64>>> {
            const localVarAxiosArgs = await WmsAsnApiAxiosParamCreator(configuration).apiWmsAsnUpdateAsnPost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
    }
};

/**
 * WmsAsnApi - factory interface
 * @export
 */
export const WmsAsnApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
    return {
        /**
         * 
         * @summary 添加Asn
         * @param {WmsAsnAddInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnAddAsnPost(body?: WmsAsnAddInput, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultInt64>> {
            return WmsAsnApiFp(configuration).apiWmsAsnAddAsnPost(body, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 查询Asn明细详情
         * @param {WmsAsnInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnAsnDetailInfoPost(body?: WmsAsnInput, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultListWmsAsnDetail>> {
            return WmsAsnApiFp(configuration).apiWmsAsnAsnDetailInfoPost(body, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 查询Asn详情
         * @param {WmsAsnInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnAsnInfoPost(body?: WmsAsnInput, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultWmsAsn>> {
            return WmsAsnApiFp(configuration).apiWmsAsnAsnInfoPost(body, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 查询Asn列表
         * @param {WmsAsnPageListInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnAsnPageListPost(body?: WmsAsnPageListInput, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultSqlSugarPagedListWmsAsnDto>> {
            return WmsAsnApiFp(configuration).apiWmsAsnAsnPageListPost(body, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 关闭Asn
         * @param {WmsAsnInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnCloseAsnPost(body?: WmsAsnInput, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultBoolean>> {
            return WmsAsnApiFp(configuration).apiWmsAsnCloseAsnPost(body, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 删除Asn
         * @param {WmsAsnDeleteInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnDeleteAsnPost(body?: WmsAsnDeleteInput, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultBoolean>> {
            return WmsAsnApiFp(configuration).apiWmsAsnDeleteAsnPost(body, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @summary 更新Asn
         * @param {WmsAsnUpdateInput} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiWmsAsnUpdateAsnPost(body?: WmsAsnUpdateInput, options?: AxiosRequestConfig): Promise<AxiosResponse<AdminResultInt64>> {
            return WmsAsnApiFp(configuration).apiWmsAsnUpdateAsnPost(body, options).then((request) => request(axios, basePath));
        },
    };
};

/**
 * WmsAsnApi - object-oriented interface
 * @export
 * @class WmsAsnApi
 * @extends {BaseAPI}
 */
export class WmsAsnApi extends BaseAPI {
    /**
     * 
     * @summary 添加Asn
     * @param {WmsAsnAddInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof WmsAsnApi
     */
    public async apiWmsAsnAddAsnPost(body?: WmsAsnAddInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultInt64>> {
        return WmsAsnApiFp(this.configuration).apiWmsAsnAddAsnPost(body, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @summary 查询Asn明细详情
     * @param {WmsAsnInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof WmsAsnApi
     */
    public async apiWmsAsnAsnDetailInfoPost(body?: WmsAsnInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultListWmsAsnDetail>> {
        return WmsAsnApiFp(this.configuration).apiWmsAsnAsnDetailInfoPost(body, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @summary 查询Asn详情
     * @param {WmsAsnInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof WmsAsnApi
     */
    public async apiWmsAsnAsnInfoPost(body?: WmsAsnInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultWmsAsn>> {
        return WmsAsnApiFp(this.configuration).apiWmsAsnAsnInfoPost(body, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @summary 查询Asn列表
     * @param {WmsAsnPageListInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof WmsAsnApi
     */
    public async apiWmsAsnAsnPageListPost(body?: WmsAsnPageListInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultSqlSugarPagedListWmsAsnDto>> {
        return WmsAsnApiFp(this.configuration).apiWmsAsnAsnPageListPost(body, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @summary 关闭Asn
     * @param {WmsAsnInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof WmsAsnApi
     */
    public async apiWmsAsnCloseAsnPost(body?: WmsAsnInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultBoolean>> {
        return WmsAsnApiFp(this.configuration).apiWmsAsnCloseAsnPost(body, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @summary 删除Asn
     * @param {WmsAsnDeleteInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof WmsAsnApi
     */
    public async apiWmsAsnDeleteAsnPost(body?: WmsAsnDeleteInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultBoolean>> {
        return WmsAsnApiFp(this.configuration).apiWmsAsnDeleteAsnPost(body, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @summary 更新Asn
     * @param {WmsAsnUpdateInput} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof WmsAsnApi
     */
    public async apiWmsAsnUpdateAsnPost(body?: WmsAsnUpdateInput, options?: AxiosRequestConfig) : Promise<AxiosResponse<AdminResultInt64>> {
        return WmsAsnApiFp(this.configuration).apiWmsAsnUpdateAsnPost(body, options).then((request) => request(this.axios, this.basePath));
    }
}