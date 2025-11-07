// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AutoMapper;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service;
public class HandoverDefaultStrategy : IHandoverInterface
{

    public SqlSugarRepository<WMSHandover> _repHandover { get; set; }
    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    public UserManager _userManager { get; set; }

    public HandoverDefaultStrategy(

    )
    {

    }

    //默认方法不做任何处理
    public async Task<Response<List<OrderStatusDto>>> Strategy(List<WMSHandover> request)
    {
        //1,根据导入的数据获取箱号(按照外部单号来，但其实箱号也是唯一的)
        //2，获取箱号数据，补冲托信息
        //3，插入到托号表
        Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>();


        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WMSPackage, WMSHandover>()
               //添加创建人为当前用户
               .ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => c.Id))
               .ForMember(a => a.HandoverStatus, opt => opt.MapFrom(c => 1))
               .ForMember(a => a.Handoveror, opt => opt.MapFrom(c => _userManager.RealName))
               .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.RealName))
               //添加库存状态为可用
               .ForMember(a => a.HandoverTime, opt => opt.MapFrom(c => DateTime.Now))
               .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
               .ForMember(a => a.UpdateTime, opt => opt.Ignore());
        });

        var mapper = new Mapper(config);

        var packageData = await _repPackage.AsQueryable().Where(a => request.Select(b => b.PackageNumber).Contains(a.PackageNumber)).ToListAsync();
        var handoverData = packageData.Adapt<List<WMSHandover>>();
        foreach (var item in handoverData)
        {
            var handover = request.Where(a => a.PackageNumber == item.PackageNumber).FirstOrDefault();
            if (handover != null)
            {
                item.Length = handover.Length;
                item.Width = handover.Width;
                item.Height = handover.Height;
                item.GrossWeight = handover.GrossWeight;
                item.NetWeight = handover.NetWeight;
                item.PalletNumber = handover.PalletNumber;
            }
        }
        if (handoverData.Count > 0)
        {
            await _repHandover.InsertRangeAsync(handoverData);
        }
        response.Code = StatusCode.Success;
        response.Msg = "操作成功";
        return response;
    }


    //private List<Table_Columns> GetColumns(string TableName, IAbpSessionExtension abpSession)
    //{
    //    return _table_ColumnsManager.Query()
    //       .Where(a => a.TableName == TableName &&
    //         a.RoleName == abpSession.RoleName &&
    //         a.IsImportColumn == 1
    //       )
    //      .Select(a => new Table_Columns
    //      {
    //          DisplayName = a.DisplayName,
    //          DbColumnName = a.DbColumnName,
    //          IsImportColumn = a.IsImportColumn
    //      }).ToList();
    //}
}
