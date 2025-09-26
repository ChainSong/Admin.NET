// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.ExternalDocking_Interface.Dto;
public enum ErrorCode
{
    //请求参数错误
    [Description("request parameter error")]
    BadRequest = 400,
    //未经授权
    [Description("Invalid app credentials")]
    Unauthorized = 401,
    //未经授权
    [Description("appId and appSecret must not be empty")]
    UnauthorizedEmpty = 401,
    //权限不足
    [Description("insufficient permissions")]
    Forbidden = 403,
    //资源未找到
    [Description("resource not found")]
    NotFound = 404,
    //请求超时
    [Description("request timeout")]
    Timeout = 408,
    //内部服务器错误
    [Description("internal Server Error")]
    InternalServerError = 500,
    //服务不可用
    [Description("service unavailable")]
    ServiceUnavailable = 503,
    //操作失败
    [Description("operation failed")]
    OperationFailed = 1004,
    //数据验证失败
    [Description("data validation failed")]
    DataValidationFailed = 1005,
    //请求中缺少必需的字段
    [Description("required fields are missing from the request")]
    MissingRequiredField = 1006,
    //无效的请求格式
    [Description("invalid request format")]
    InvalidRequestFormat = 1007,
    //资源已存在
    [Description("the resource already exists")]
    ResourceAlreadyExists = 1008,
    //数据库连接失败
    [Description("database connection failed")]
    DatabaseConnectionFailed = 1009,
    //Token 无效
    [Description("token is invalid")]
    InvalidToken = 2001,
    //Token 已过期
    [Description("token has expired")]
    TokenExpired = 2002,
    // 外部服务不可用
    [Description("external service unavailable")]
    ExternalServiceUnavailable = 3001,
    //外部 API 调用失败
    [Description("external API call failed")]
    ExternalApiCallFailed = 3002
}
public enum SuccessCode
{
    [Description("Operation Successful")]
    Success = 0,
}