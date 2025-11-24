// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using System.Reflection;
using XAct.Library.Settings;
using Admin.NET.Application.Service;
using Admin.NET.Common;
using RulesEngine.Models;

namespace Admin.NET.Application.Service;
public class PackageDGPrintDefaultStrategy : IPackagePrintInterface
{
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<Admin.NET.Core.Entity.WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
    //包装
    public SqlSugarRepository<Admin.NET.Core.Entity.WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSOrderAddress> _repOrderAddress { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }
    public SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable { get; set; }
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    public SqlSugarRepository<WMSCustomerConfig> _repCustomerConfig { get; set; }
    public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
    public UserManager _userManager { get; set; }
    public PackageDGPrintDefaultStrategy() //: base( _repTableColumns, _userManager)
    {

    }
    //默认方法不做任何处理
    public async Task<Response<PrintBase<dynamic>>> Strategy(List<long> request)
    {
        Response<PrintBase<dynamic>> response = new Response<PrintBase<dynamic>>();
        WMSPackageDto packageDto = new WMSPackageDto();
        string sql = $@"select 
                        WMS_Order.CustomerId,--客户ID
                        WMS_Order.CustomerName,--客户名称
                        WMS_Order.ExternOrderNumber,--客户单号
                        (select top 1 PoCode from WMS_PickTaskDetail 
                        where PickTaskNumber =WMS_Package.PickTaskNumber) Po,--客户合同号
                        OrderAllocation.BoxCode,--箱号
                        OrderAllocation.Str1,--箱号
                        WMS_Order.CompleteTime,--交货时间
                        WMS_OrderAddress.CompanyName,--客户代码
                        WMS_OrderAddress.CompanyName,--客户代码
                        WMS_OrderAddress.Address,--客户地址
                        WMS_OrderAddress.Name as ContactName,--客户联系人
                        WMS_OrderAddress.Phone,--客户电话
                        WMS_Package.PackageNumber, --装箱清单编号
                        hachOb.DeliveryNumber as JobNumber,--JOB单号
                        wmsPackageTime.CreationTime as HandoverTime--交接时间
                        from WMS_Package  
                        outer apply (select top 1 *  from WMS_Order 
                        where WMS_Order.Id=WMS_Package.OrderId) WMS_Order
                        left join  WMS_OrderAddress on WMS_Order.ExternOrderNumber=WMS_OrderAddress.ExternOrderNumber
                        left join  hach_wms_outBound hachOb on WMS_Order.ExternOrderNumber=hachOb.OrderNumber
                        outer apply (select top 1 *  from WMS_OrderAllocation where WMS_Order.Id=WMS_OrderAllocation.OrderId) OrderAllocation
                        outer apply (select top 1 *  from WMS_Package wp where WMS_Order.Id=wp.OrderId order by createtime desc) wmsPackageTime
                        where WMS_Package.Id in ({string.Join(",", request)}) ";

     string sqlDetail =$@"select  
    ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) as [sequence],
    WMS_Package.PackageNumber, 
    SKU,
    GoodsName,
    SUM(Qty) Qty,
    SerialNumber as 'SingelBoxQty',
    (select COUNT(Id) from WMS_Package wp where wp.OrderId=(select orderid from WMS_Package where id=10425)) as TotalQty,
    hachObInfo.ParentItemNumber as parentSku, --父件SKU
    WMS_Package.Remark  
    from WMS_Package 
    left join WMS_PackageDetail on WMS_Package.PackageNumber=WMS_PackageDetail.PackageNumber
    outer apply(
    select 
    hachobDetail.ItemNumber,
    hachobDetail.ParentItemNumber 
    from hach_wms_outBound hachob 
    left join hach_wms_outBound_detail hachobDetail on hachob.Id=hachobDetail.OutBoundId
    where hachob.OrderNumber = (select externordernumber from WMS_Package where id=10425)
    and hachobDetail.ItemNumber=WMS_PackageDetail.SKU
    ) hachObInfo
    where WMS_Package.Id in ({string.Join(",", request)}) 
    group by SKU,GoodsName,WMS_Package.Remark,WMS_Package.PackageNumber,hachObInfo.ParentItemNumber,WMS_Package.SerialNumber";
        
        var hachPrintPackageData = _repOrder.Context.Ado.SqlQuery<HachDGPrintPackageData>(sql.ToString());
        var hachPrintPackageDataDetail = _repOrder.Context.Ado.SqlQuery<HachDGPrintPackageDataDetail>(sqlDetail.ToString());

        foreach (var item in hachPrintPackageData)
        {
            item.Detail = hachPrintPackageDataDetail.Where(x => x.PackageNumber == item.PackageNumber).ToList();
            item.CustomerConfig = await _repCustomerConfig.AsQueryable().Where(x => x.CustomerId.ToString() == item.CustomerId).FirstAsync();
        }
        response.Data = new PrintBase<dynamic>()
        {
            Data = hachPrintPackageData
        };
        response.Code = StatusCode.Success;
        return response;
    }


}

