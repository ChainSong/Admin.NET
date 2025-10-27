// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Const;
using Admin.NET.Application.Service.ExternalDocking_Interface.Dto;
using Admin.NET.Application.Service.ExternalDockingInterface.Helper;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaBusinessGetUserEncryptKeyResponse.Types;

namespace Admin.NET.Application;

/// <summary>
/// OpenAuth
/// </summary>
[ApiDescriptionSettings("OpenAuth", Order = 1, Groups = new[] { "HACHWMS INTERFACE" })]
public class OpenAuthService : IDynamicApiController, ITransient
{
    private readonly SysAuthService _sysAuthRep;
    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly SysConfigService _sysConfigService;
    private readonly SqlSugarRepository<HachWmsAuthorizationConfig> _hachWmsAuthorizationConfigRep;
    private readonly LogHelper _logHelper;

    public OpenAuthService(SysAuthService sysAuthRep, SqlSugarRepository<SysUser> sysUserRep,
      SysConfigService sysConfigService,
        LogHelper logHelper,
        SqlSugarRepository<HachWmsAuthorizationConfig> hachWmsAuthorizationConfigRep)
    {
        _sysAuthRep = sysAuthRep;
        _sysUserRep = sysUserRep;
        _sysConfigService = sysConfigService;
        _hachWmsAuthorizationConfigRep = hachWmsAuthorizationConfigRep;
        _logHelper = logHelper;
    }

    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "open-auth")]
    public async Task<TokenOutput> Token([FromBody] OpenAuthInput input)
    {
        // 生成批次号（日志追踪唯一标识）
        string batchId = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        // 记录上游请求原始报文
        string jsonPayload = System.Text.Json.JsonSerializer.Serialize(input, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        await _logHelper.LogAsync(
            LogHelper.LogMainType.身份鉴权,
            batchId,
            "BATCH",
            LogHelper.LogLevel.Info,
            "收到上游请求报文",
            true,
            jsonPayload
        );

        if (input.AppId == null || string.IsNullOrEmpty(input.AppSecret))
        {
            await _logHelper.LogAsync(LogHelper.LogMainType.身份鉴权, batchId, "BATCH", LogHelper.LogLevel.Info,
          $"appId：{input.AppId}和appSecret：{input.AppSecret}不能为空", true, jsonPayload );

            throw Oops.Oh(ErrorCode.UnauthorizedEmpty.GetDescription());
        }

        var app = await _hachWmsAuthorizationConfigRep
                  .AsQueryable()
                  .Where(a => a.Type == "HachWMSApi")
                  .Where(a => a.Status == true)
                  .Where(a => a.IsDelete == false)
                  .Where(a => a.AppId == input.AppId)
                  .Where(a => a.AppSecret == input.AppSecret)
                  .OrderByDescending(a => a.CreateTime) // 按创建时间倒序
                  .FirstAsync();

        if (app == null)
        {
            await _logHelper.LogAsync(LogHelper.LogMainType.身份鉴权, batchId, "BATCH", LogHelper.LogLevel.Info,
         $"appId：{input.AppId}和appSecret：{input.AppSecret} 无效的凭证 ", true, jsonPayload);
            throw Oops.Oh(ErrorCode.Unauthorized.GetDescription()); // 无效的凭证
        }
        var tokenOutput = await CreateToken(app);
        return tokenOutput;
    }

    /// <summary>
    /// 根据 appId 创建系统级的 Token（无需用户信息）
    /// </summary>
    /// <param name="appId">应用 ID</param>
    /// <returns>Token 输出</returns>
    private async Task<TokenOutput> CreateToken(HachWmsAuthorizationConfig input)
    {
        var tokenExpire = await _sysConfigService.GetTokenExpire();// 生成Token令牌

        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { ClaimConst.UserId, input.AppId },
                { ClaimConst.TenantId, input.TenantId },
                { ClaimConst.AccountType, "Application" }
            }, tokenExpire);

        var refreshTokenExpire = await _sysConfigService.GetRefreshTokenExpire();// 生成刷新Token令牌

        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);

        return new TokenOutput
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = 1200000,
            TokenType = "Bearer"
        };
    }
}
