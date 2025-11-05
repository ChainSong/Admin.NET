// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity;
using Admin.NET.Core;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Core.Service;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Identity;

namespace Admin.NET.Application.Service.ExternalDockingInterface.Helper;
public class GetConfig: ITransient
{
    private readonly SqlSugarRepository<HachWmsAuthorizationConfig> _hachWmsAuthorizationConfigRep;
    private readonly UserManager _userManager;
    public GetConfig(SqlSugarRepository<HachWmsAuthorizationConfig> hachWmsAuthorizationConfigRep, UserManager userManager)
    {
        _hachWmsAuthorizationConfigRep = hachWmsAuthorizationConfigRep;
        _userManager = userManager;
    }
    //获取客户信息
    public async Task<HachWmsAuthorizationConfig> GetCustomerInfo(string Type, string? WarehouseCode = null)
    {
        HachWmsAuthorizationConfig hachCustomerMappings = new HachWmsAuthorizationConfig();

        if (string.IsNullOrEmpty(Type))
        {
            return hachCustomerMappings;
        }

        hachCustomerMappings = await _hachWmsAuthorizationConfigRep.AsQueryable()
            .Where(a => a.AppId == _userManager.UserId)
            .Where(a => a.Status == true)
            .Where(a => a.IsDelete == false)
            .Where(a => a.Type == "HachWMSApi")
            .Where(a => a.InterFaceName == Type)
            .WhereIF(!string.IsNullOrEmpty(WarehouseCode), a => a.WarehouseCode == WarehouseCode)
            .OrderByDescending(a => a.Id)
            .FirstAsync();

        return hachCustomerMappings;
    }
}
