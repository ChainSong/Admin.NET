using Microsoft.IdentityModel.Tokens;
using SqlSugar;
using TaskPlaApplication.Application.Contracts;
using TaskPlaApplication.Models.Entities;
using TaskPlaApplication.Shared.Options;

namespace TaskPlaApplication.Infrastructure.Persistence;

/// <summary>【读库配置】强类型配置表实现</summary>
public sealed class DbThirdPartyConfigProvider(ISqlSugarClient db) : IThirdPartyConfigProvider
{
    public async Task<HachConfirmWmsAuthorizationConfig> GetAsync(HachConfirmWmsAuthorizationConfig input, CancellationToken ct = default)
    {
        HachConfirmWmsAuthorizationConfig row = new HachConfirmWmsAuthorizationConfig();
        try
        {
         
            row = await db.Queryable<HachConfirmWmsAuthorizationConfig>()
                             .Where(x => x.Status)
                             .Where(x => !x.IsDelete)
                             .Where(x => x.Type == input.Type)
                             .WhereIF(!string.IsNullOrEmpty(input.InterfaceName), x => x.InterfaceName == input.InterfaceName)
                             .WhereIF(!string.IsNullOrEmpty(input.CustomerName), x => x.CustomerName == input.CustomerName)
                             .WhereIF(input.CustomerId.HasValue, x => x.CustomerId == input.CustomerId)
                              .WhereIF(!string.IsNullOrEmpty(input.WarehouseName), x => x.WarehouseName == input.WarehouseName)
                             .WhereIF(input.WarehouseId.HasValue, x => x.WarehouseId == input.WarehouseId)
                             .OrderBy(x => new { x.CreateTime, x.UpdateTime, x.Id })  // 自行加排序条件
                             .FirstAsync();
        }
        catch (Exception ex)
        {

            throw;
        }
        return row;
    }
}
