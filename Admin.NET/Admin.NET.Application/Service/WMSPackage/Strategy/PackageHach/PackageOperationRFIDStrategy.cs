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
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Application.Service.Enumerate;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Common.TextCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using AutoMapper;
using FluentEmail.Core;
using Furion.FriendlyException;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using XAct;

namespace Admin.NET.Application.Strategy;
internal class PackageOperationRFIDStrategy : IPackageOperationInterface
{

    public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }
    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }

    public SqlSugarRepository<WMSProduct> _repProduct { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public SqlSugarRepository<WMSProductBom> _repProductBom { get; set; }
    public UserManager _userManager { get; set; }
    public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }
    public SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo { get; set; }

    //public ISqlSugarClient _db { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }
    public SysCacheService _sysCacheService { get; set; }

    public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }

    public SqlSugarRepository<WMSRFPackageAcquisition> _repRFPackageAcquisition { get; set; }
    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    TimeSpan timeSpan = new TimeSpan(72, 0, 0);
    public async Task<Response<ScanPackageOutput>> GetPackage(ScanPackageInput request)
    {

        Response<ScanPackageOutput> response = new Response<ScanPackageOutput>() { Data = new ScanPackageOutput() };
        //if (request.PickTaskNumber == request.Input)
        //{
        //    request.PickTaskNumber = "";
        //}
        //判断扫描的是不是条形码（有两种条形码）
        if (!string.IsNullOrEmpty(request.Input))
        {
            //var skuInfo = request.Input.Split('|');
            if (request.Input.Split(' ').Length > 1 || request.Input.Split('|').Length > 1)
            {

                string SKURegex = @"(?<=\|ITM)[^|]+|^[^\s:]+=[0-9]{3,4}[CN]{0,2}(?=[0-9]{5}\b)|^[^|][^\s:]+(?=\s|$)"; // 正则表达式匹配英文字符或数字
                string LOTRegex = @"(?<=\|LOT)[^\|]+|(?<==\d{3}|=\d{4}|=\d({4}CN)[0-9]{5}\b|(?<=\s)[A-Z0-9]{1,5}\b";
                string ExpirationDateRegex = @"(?<=\|EXP)[^\|]+|(?<=\s)\d{6}\b";
                MatchCollection matchesSKU = Regex.Matches(request.Input, SKURegex);
                request.SKU = matchesSKU.Count > 0 ? matchesSKU[0].Value : "";

                MatchCollection matchesExpirationDateRegex = Regex.Matches(request.Input, ExpirationDateRegex);
                request.AcquisitionData = matchesExpirationDateRegex.Count > 0 ? matchesExpirationDateRegex[0].Value : "";
                MatchCollection matchesLOT = Regex.Matches(request.Input, LOTRegex);
                request.Lot = matchesLOT.Count > 0 ? matchesLOT[0].Value : "";
                request.Input = request.SKU;

            }  //扫描的是HTTP 二维码，那么从中解析SKU
            else if (request.Input.Contains("http"))
            {

                Uri uri = new Uri(request.Input);
                var collection = HttpUtility.ParseQueryString(uri.Query);
                var p = collection["p"];
                if (p.Count() > 0)
                {
                    request.SKU = collection["p"].Split(':')[1];
                    request.RFID = collection["p"].Split(':')[0];
                }
            }
            ;



        }


        response.Data.PickTaskNumber = request.PickTaskNumber;
        response.Data.Weight = request.Weight;
        response.Data.SKU = request.SKU;
        response.Data.Input = request.Input;
        response.Data.SN = request.SN;
        response.Data.Lot = request.Lot;
        response.Data.RFID = request.RFID;
        response.Data.AcquisitionData = request.AcquisitionData;

        List<PackageData> pickData = new List<PackageData>();
        if (!string.IsNullOrEmpty(request.PickTaskNumber))
        {
            pickData = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);
            //判断是不是包装完成，补充了重量  缓存有数据， 取缓存返回
            if (pickData != null && pickData.Count > 0 && pickData.Where(a => (a.PackageQty + a.ScanQty) != a.PickQty).Count() == 0)
            {
                //判断有没有扫描数据
                if (pickData.Count > 0 && pickData.Where(a => a.ScanQty > 0).Count() > 0)
                {

                    foreach (var item in pickData)
                    {
                        request.RFIDs.AddRange(item.RFIDs);
                    }
                    var result = await PackingRFIDComplete_Scan(pickData, request, PackageBoxTypeEnum.正常);
                    response.Code = result.Code;
                    response.Msg = result.Msg;
                    return response;
                }
                else
                {
                    //说明已经包装完成，删除缓存；
                    _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, null);
                    response.Code = StatusCode.Error;
                    response.Msg = "该拣货任务已经包装完成";
                    return response;
                }
            }
            if (pickData == null)
            {
                request.PickTaskNumber = "";
            }

        }


        //else
        //{
        //    pickData = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.Input);
        //    //判断是不是包装完成，补充了重量  缓存有数据， 取缓存返回
        //    if (pickData != null && pickData.Count > 0 && pickData.Where(a => (a.PackageQty + a.ScanQty) != a.PickQty).Count() == 0)
        //    {
        //        var result = await PackingComplete(pickData, request);
        //        response.Code = result.Code;
        //        response.Msg = result.Msg;
        //        return response;
        //    }
        //}
        //缓存没数据， 判断订单是不是完成


        //判断是不是第一次加载，要是缓存中有数据，直接取缓存
        //1，判断有没有拣货单号
        if (string.IsNullOrEmpty(request.PickTaskNumber))
        {
            response.Data.PickTaskNumber = request.Input;
            request.PickTaskNumber = request.Input;
            pickData = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);
            if (pickData != null && pickData.Count > 0)
            {
                response.Data.PackageDatas = pickData;
                response.Code = StatusCode.Success;
                return response;
            }




            var CheckPickData = _repPickTask.AsQueryable().Where(a => a.PickTaskNumber == response.Data.PickTaskNumber)
                .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
            .ToList();
            if (CheckPickData.Where(a => a.PickStatus == (int)PickTaskStatusEnum.包装完成).Count() > 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "拣货单已经完成包装";
                return response;
            }
            else if (CheckPickData.Where(a => a.PickStatus == (int)PickTaskStatusEnum.拣货完成).Count() == 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "拣货单还未完成拣货";
                return response;

            }
            else if (CheckPickData.Count == 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "拣货单号不存在";
                return response;
            }


            pickData = _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == response.Data.PickTaskNumber && a.PickStatus == (int)PickTaskStatusEnum.拣货完成)
                  .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                  .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
              .GroupBy(a => new { a.SKU, a.PickTaskNumber })
              .Select(a => new PackageData { SKU = a.SKU, PickQty = SqlFunc.AggregateSum(a.PickQty), RemainingQty = SqlFunc.AggregateSum(a.PickQty), PickTaskNumber = a.PickTaskNumber, GoodsName = SqlFunc.AggregateMax(a.GoodsName), GoodsType = SqlFunc.AggregateMax(a.GoodsType) })
              .ToList();


            if (pickData.Count > 0)
            {

                _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, pickData, timeSpan);
                response.Data.PackageDatas = pickData;
                response.Code = StatusCode.Success;
                return response;
            }


        }

        //2，判断扫描数据是不是SKU
        if (pickData != null && pickData.Count > 0)
        {
            if (string.IsNullOrEmpty(request.RFID))
            {
                response.Code = StatusCode.Error;
                response.Msg = "请扫描RFID二维码";
                return response;
            }
            response.Data.SKU = request.SKU;
            //判断RFID 是否重复

            //判断有没有SN,有SN 就记录出库SN
            //WMSRFPackageAcquisition wMSRF=new WMSRFPackageAcquisition();
            //wMSRF.
            //_repRFPackageAcquisition
            var PickSKUData = pickData.Where(a => a.SKU == request.SKU);
            if (PickSKUData.Count() > 0)
            {
                if (PickSKUData.Sum(a => (a.ScanQty + a.PackageQty)) + 1 <= PickSKUData.First().PickQty)
                {

                    var pick = pickData.Where(a => a.SKU == request.SKU && a.PickQty > (a.ScanQty + a.PackageQty)).First();
                    if (pick.RFIDs.Contains(request.RFID))
                    {
                        response.Data.PackageDatas = pickData;
                        response.Code = StatusCode.Error;
                        response.Msg = "RFID 已存在";
                        return response;
                    }
                    pick.RFIDs.Add(request.RFID);
                    pick.ScanQty += 1;
                    pick.RemainingQty -= 1;
                    pick.ScanPackageInput = new List<ScanPackageInput>();
                    pick.ScanPackageInput.Add(request);
                    _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, pickData, timeSpan);

                    //判断是不是包装完成
                    if (pickData.Where(a => (a.ScanQty + a.PackageQty) != a.PickQty).Count() == 0)
                    {
                        foreach (var item in pickData)
                        {
                            request.RFIDs.AddRange(item.RFIDs);
                        }

                        var result = await PackingRFIDComplete_Scan(pickData, request, PackageBoxTypeEnum.正常);
                        response.Data.PackageDatas = pickData;
                        response.Code = result.Code;
                        response.Msg = result.Msg;
                        return response;
                    }
                    response.Data.PackageDatas = pickData;
                    response.Code = StatusCode.Success;
                    return response;
                }
                else
                {
                    response.Data.PackageDatas = pickData;
                    response.Code = StatusCode.Error;
                    response.Msg = "该SKU数量已满足";
                    return response;
                }

            }
            else
            {
                response.Data.PackageDatas = pickData;
                response.Code = StatusCode.Error;
                response.Msg = "SKU 不存在";
                return response;
            }
        }

        response.Code = StatusCode.Error;
        response.Msg = "系统错误";
        return response;
    }



    public async Task<Response<ScanPackageOutput>> AddPackage(ScanPackageInput request)
    {

        //var aa = await VerifyPackage(new ScanPackageRFIDInput() { RFIDStr = "000591703201100001674522,000591703201100001687266,000591703201100001698666,000591703201100001704611" });
        //return null;
        Response<ScanPackageOutput> response = new Response<ScanPackageOutput>() { Data = new ScanPackageOutput() };
        response.Data.PickTaskNumber = request.PickTaskNumber;
        response.Data.Weight = request.Weight;
        response.Data.SKU = request.SKU;
        response.Data.Input = request.Input;
        var pickData = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);

        //保存缓存中的已经包装的数据
        if (pickData != null && pickData.Sum(a => a.ScanQty) > 0)
        {

            var PackingCompleteCheck = await PackingComplete(pickData, request, PackageBoxTypeEnum.新增);
            pickData = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);
            response.Data.PackageDatas = pickData;
            if (PackingCompleteCheck.Code == StatusCode.Finish)
            {
                response.Code = PackingCompleteCheck.Code;
                response.Msg = PackingCompleteCheck.Msg;
                return response;
            }
            else
            {

                response.Code = PackingCompleteCheck.Code;
                response.Msg = PackingCompleteCheck.Msg;
                //response.Msg = PackingCompleteCheck.Msg;
                return response;
            }
        }
        else
        {
            if (string.IsNullOrEmpty(request.PickTaskNumber) || string.IsNullOrEmpty(request.Input) || string.IsNullOrEmpty(request.SKU))
            {
                throw Oops.Oh("请先扫描数据");
            }
        }



        response.Code = StatusCode.Error;
        response.Msg = "系统错误";
        return response;
    }
    public async Task<Response<ScanPackageOutput>> ShortagePackage(ScanPackageInput request)
    {

        Response<ScanPackageOutput> response = new Response<ScanPackageOutput>() { Data = new ScanPackageOutput() };
        response.Data.PickTaskNumber = request.PickTaskNumber;
        response.Data.Weight = request.Weight;
        response.Data.SKU = request.SKU;
        response.Data.Input = request.Input;
        var pickData = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);

        //保存缓存中的已经包装的数据
        if (pickData != null)
        {

            var PackingCompleteCheck = await PackingComplete(pickData, request, PackageBoxTypeEnum.短包);
            pickData = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);
            response.Data.PackageDatas = pickData;
            if (PackingCompleteCheck.Code == StatusCode.Finish)
            {
                response.Code = PackingCompleteCheck.Code;
                response.Msg = PackingCompleteCheck.Msg;
                return response;
            }
            else
            {

                response.Code = PackingCompleteCheck.Code;
                response.Msg = PackingCompleteCheck.Msg;
                //response.Msg = PackingCompleteCheck.Msg;
                return response;
            }
        }


        response.Code = StatusCode.Error;
        response.Msg = "系统错误";
        return response;
    }

    //保存的方法
    private async Task<Response> PackingComplete(List<PackageData> pickData, ScanPackageInput request, PackageBoxTypeEnum packageBox)
    {

        Response response = new Response();
        if (request.Weight < 0.3)
        {
            request.Weight = 1;
        }
        //判断是不是输入了重量
        if (request.Weight > 0.2)
        {
            var pickDataTemp = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber && a.PickStatus == (int)PickTaskStatusEnum.拣货完成)
                 .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                 .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0).FirstAsync();
            var packageNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
            var packagenumberData = await _repPackage.AsQueryable().Where(a => a.OrderId == pickDataTemp.OrderId).ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WMSPickTaskDetail, WMSPackage>()
                   //添加创建人为当前用户
                   .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                   .ForMember(a => a.PickTaskId, opt => opt.MapFrom(c => c.PickTaskId))
                   .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                   .ForMember(a => a.PackageTime, opt => opt.MapFrom(c => DateTime.Now))
                   // 
                   .ForMember(a => a.PackageStatus, opt => opt.MapFrom(c => PackageStatusEnum.完成))
                   .ForMember(a => a.Id, opt => opt.Ignore())
                   //将为Null的字段设置为"" () 
                   .AddTransform<string>(a => a == null ? "" : a)
                   //将为Null的字段设置为"" () 
                   .AddTransform<int?>(a => a == null ? 0 : a)
                   //将为Null的字段设置为"" () 
                   .AddTransform<double?>(a => a == null ? 0 : a)
                   //添加
                   .ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber));
                //.AddTransform<string>(a => a == null ? "" : a);

                cfg.CreateMap<PackageData, WMSPackageDetail>()
                  //添加创建人为当前用户
                  .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                  .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                  .ForMember(a => a.Qty, opt => opt.MapFrom(c => c.ScanQty))
                   //将为Null的字段设置为"" () 
                   .AddTransform<int?>(a => a == null ? 0 : a)
                   //将为Null的字段设置为"" () 
                   .AddTransform<double?>(a => a == null ? 0 : a)
                  //添加
                  .ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber));
            });
            TextHelper.WrittxtFor("准备更新RFID +request", "/File/TextLog", "RFIDLog" + DateTime.Now.ToString("yyyyMMddhh") + ".txt");
            TextHelper.WrittxtFor(JsonSerializer.Serialize(request), "/File/TextLog", "RFIDLog" + DateTime.Now.ToString("yyyyMMddhh") + ".txt");
            TextHelper.WrittxtFor("准备更新RFID +pickData", "/File/TextLog", "RFIDLog" + DateTime.Now.ToString("yyyyMMddhh") + ".txt");
            TextHelper.WrittxtFor(JsonSerializer.Serialize(pickData.First().RFIDs), "/File/TextLog", "RFIDLog" + DateTime.Now.ToString("yyyyMMddhh") + ".txt");
            foreach (var item in pickData)
            {
                if (item.RFIDInfoOld == null)
                {
                    item.RFIDInfoOld = new List<WMSRFIDInfo>();
                }
                item.RFIDInfoOld.AddRange(item.RFIDInfo);
            }
            var mapper = new Mapper(config);
            var packageData = mapper.Map<WMSPackage>(pickDataTemp);
            var packageDetailData = mapper.Map<List<WMSPackageDetail>>(pickData.Where(a => a.ScanQty > 0));
            //var packageDetailDetail = .WMSRFPackageAcquisition

            packageData.DetailCount = packageDetailData.Sum(a => a.Qty);
            packageData.Details = packageDetailData;
            packageData.Details.ForEach(a =>
            {
                a.CustomerId = packageData.CustomerId;
                a.CustomerName = packageData.CustomerName;
                a.WarehouseId = packageData.WarehouseId;
                a.WarehouseName = packageData.WarehouseName;
                a.OrderId = packageData.OrderId;
                a.ExternOrderNumber = packageData.ExternOrderNumber;
                a.PreOrderNumber = packageData.PreOrderNumber;
                a.OrderNumber = packageData.OrderNumber;
                a.PickTaskId = packageData.PickTaskId;
                //a.Weight = 0;
            });

            List<WMSRFPackageAcquisition> PackageAcquisitions = new List<WMSRFPackageAcquisition>();
            foreach (var item in pickData)
            {
                var PackageAcquisition = item.ScanPackageInput.Adapt<List<WMSRFPackageAcquisition>>();
                if (PackageAcquisition != null)
                {
                    foreach (var p in PackageAcquisition)
                    {
                        p.Qty = 1;
                        p.CustomerId = packageData.CustomerId;
                        p.CustomerName = packageData.CustomerName;
                        p.WarehouseId = packageData.WarehouseId;
                        p.WarehouseName = packageData.WarehouseName;
                        p.OrderId = packageData.OrderId;
                        p.ExternOrderNumber = packageData.ExternOrderNumber;
                        p.PreOrderNumber = packageData.PreOrderNumber;
                        p.OrderNumber = packageData.OrderNumber;
                        p.PickTaskId = packageData.PickTaskId;
                        p.Creator = _userManager.Account;
                        p.CreationTime = DateTime.Now;
                    }
                    PackageAcquisitions.AddRange(PackageAcquisition);
                }
            }

            var result = await _repRFIDInfo.AsQueryable().Where(p => pickData.First().RFIDInfo.Select(q => q.RFID).Contains(p.RFID) && p.Status == (int)RFIDStatusEnum.新增
            && packageData.CustomerId == packageData.CustomerId
            ).ToListAsync();
            if (result.Count > packageData.Details.Sum(a => a.Qty))
            {
                response.Code = StatusCode.Error;
                response.Msg = "RFID 读取错误";
                return response;
            }
            foreach (var item in result)
            {
                item.Status = (int)RFIDStatusEnum.出库;
                item.PackageNumber = packageNumber;
                item.Sequence = pickData.First().RFIDInfo.Where(a => a.RFID.Contains(item.RFID)).FirstOrDefault().Sequence;
                item.ExternOrderNumber = packageData.ExternOrderNumber;
                item.PickTaskNumber = packageData.PickTaskNumber;
                item.OrderTime = DateTime.Now;
                item.OrderPerson = _userManager.Account;
                item.PickTaskNumber = pickData.FirstOrDefault().PickTaskNumber;
                item.OrderNumber = packageData.OrderNumber;
            }
            await _repRFIDInfo.UpdateRangeAsync(result);

            //包装的时候，将RFID  状态修改成为10 标识RFID 已经拣货包装
            //_repRFIDInfo.UpdateAsync(a => a.Status = (int)RFIDStatusEnum.包装).Where(a => request.RFIDs.Contains(a.RFID));
            //   await _repRFIDInfo.Context.Updateable<WMSRFIDInfo>()
            //.SetColumns(p => p.Status == (int)RFIDStatusEnum.出库)
            //.SetColumns(p => p.OrderTime == DateTime.Now)
            //.SetColumns(p => p.OrderPerson == _userManager.Account)
            ////.SetColumns(p => p.OrderNumber == request.RFIDInfo.Where(a=>a.RFID.Contains(p.RFID)).FirstOrDefault().Sequence)
            //.SetColumns(p => p.ExternOrderNumber == packageData.ExternOrderNumber)
            ////.SetColumns(p => p.Sequence == packageData.ExternOrderNumber)
            ////.SetColumns(p => p.PreOrderId == packageData.PreOrderId)
            //.SetColumns(p => p.PickTaskNumber == pickData.FirstOrDefault().PickTaskNumber)
            ////.SetColumns(p => p.ExternOrderNumber == pickData.FirstOrDefault().PickTaskNumber)
            //.Where(p => request.RFIDs.Contains(p.RFID) && p.Status == (int)RFIDStatusEnum.新增)
            //.ExecuteCommandAsync();

            packageData.ExpressCompany = request.ExpressCompany;
            packageData.GrossWeight = request.Weight;
            packageData.NetWeight = request.Weight;
            packageData.Id = 0;
            packageData.SerialNumber = (packagenumberData.Count + 1).ToString();


            await _repPackage.Context.InsertNav(packageData).Include(a => a.Details).ExecuteCommandAsync();
            await _repRFPackageAcquisition.InsertRangeAsync(PackageAcquisitions);
            //await _repPackage.InsertAsync();

            //_sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, null);
            pickData.ForEach(a =>
            {

                a.PackageQty += a.ScanQty;
                a.ScanQty = 0;
            });
            _sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, pickData, timeSpan);
            //判断是否包装完成
            var CheckPackageData = _repPackage.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber).Sum(a => a.DetailCount);
            var CheckPickData = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber).ToListAsync();
            if (CheckPackageData >= CheckPickData.Sum(a => a.PickQty) || packageBox == PackageBoxTypeEnum.短包)
            {
                await _repPickTask.UpdateAsync(a => new WMSPickTask { PickStatus = (int)PickTaskStatusEnum.包装完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, (a => a.PickTaskNumber == request.PickTaskNumber));
                await _repPickTaskDetail.UpdateAsync(a => new WMSPickTaskDetail { PickStatus = (int)PickTaskStatusEnum.包装完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, (a => a.PickTaskNumber == request.PickTaskNumber));
                await _repOrder.UpdateAsync(a => new WMSOrder { OrderStatus = (int)OrderStatusEnum.已包装 }, (a => CheckPickData.Select(b => b.OrderId).ToList().Contains(a.Id)));
                //_repPickTask.UpdateAsync(a => a.PickStatus = (int)PickTaskStatusEnum.包装完成);
                //_repPickTaskDetail.UpdateAsync(a => a.PickStatus = (int)PickTaskStatusEnum.包装完成);
                _sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, null);
                response.Code = StatusCode.Finish;
                response.Msg = "订单完成";
                return response;
            }
            response.Code = StatusCode.Success;
            response.Msg = "成功";
            return response;
            //return response;
        }
        else
        {
            response.Code = StatusCode.Error;
            response.Msg = "请输入重量";
            return response;
            //return response;
        }
        //Response response = new Response();

        ////判断是不是输入了重量
        //if (request.Weight > 0.2)
        //{
        //    var pickDataTemp = _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber && a.PickStatus == (int)PickTaskStatusEnum.拣货完成)
        //         .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
        //         .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0).FirstAsync();
        //    var packageNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<WMSPickTaskDetail, WMSPackage>()
        //           //添加创建人为当前用户
        //           .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
        //           .ForMember(a => a.PickTaskId, opt => opt.MapFrom(c => c.PickTaskId))
        //           .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
        //           .ForMember(a => a.PackageTime, opt => opt.MapFrom(c => DateTime.Now))
        //           // 
        //           .ForMember(a => a.PackageStatus, opt => opt.MapFrom(c => PackageStatusEnum.完成))
        //           .ForMember(a => a.Id, opt => opt.Ignore())
        //           //将为Null的字段设置为"" () 
        //           .AddTransform<string>(a => a == null ? "" : a)
        //           //将为Null的字段设置为"" () 
        //           .AddTransform<int?>(a => a == null ? 0 : a)
        //           //将为Null的字段设置为"" () 
        //           .AddTransform<double?>(a => a == null ? 0 : a)
        //           //添加
        //           .ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber));
        //        //.AddTransform<string>(a => a == null ? "" : a);

        //        cfg.CreateMap<PackageData, WMSPackageDetail>()
        //          //添加创建人为当前用户
        //          .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
        //          .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
        //          .ForMember(a => a.Qty, opt => opt.MapFrom(c => c.ScanQty))
        //           //将为Null的字段设置为"" () 
        //           .AddTransform<int?>(a => a == null ? 0 : a)
        //           //将为Null的字段设置为"" () 
        //           .AddTransform<double?>(a => a == null ? 0 : a)
        //          //添加
        //          .ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber));
        //    });


        //    var mapper = new Mapper(config);
        //    var packageData = mapper.Map<WMSPackage>(pickDataTemp.Result);
        //    var packageDetailData = mapper.Map<List<WMSPackageDetail>>(pickData.Where(a => a.ScanQty > 0));
        //    //var packageDetailDetail = .WMSRFPackageAcquisition

        //    packageData.DetailCount = packageDetailData.Sum(a => a.Qty);
        //    packageData.Details = packageDetailData;
        //    packageData.Details.ForEach(a =>
        //    {
        //        a.CustomerId = packageData.CustomerId;
        //        a.CustomerName = packageData.CustomerName;
        //        a.WarehouseId = packageData.WarehouseId;
        //        a.WarehouseName = packageData.WarehouseName;
        //        a.OrderId = packageData.OrderId;
        //        a.ExternOrderNumber = packageData.ExternOrderNumber;
        //        a.PreOrderNumber = packageData.PreOrderNumber;
        //        a.OrderNumber = packageData.OrderNumber;
        //        a.PickTaskId = packageData.PickTaskId;
        //        //a.Weight = 0;
        //    });

        //    List<WMSRFPackageAcquisition> PackageAcquisitions = new List<WMSRFPackageAcquisition>();
        //    foreach (var item in pickData)
        //    {
        //        var PackageAcquisition = item.ScanPackageInput.Adapt<List<WMSRFPackageAcquisition>>();
        //        if (PackageAcquisition != null)
        //        {
        //            foreach (var p in PackageAcquisition)
        //            {
        //                p.Qty = 1;
        //                p.CustomerId = packageData.CustomerId;
        //                p.CustomerName = packageData.CustomerName;
        //                p.WarehouseId = packageData.WarehouseId;
        //                p.WarehouseName = packageData.WarehouseName;
        //                p.OrderId = packageData.OrderId;
        //                p.ExternOrderNumber = packageData.ExternOrderNumber;
        //                p.PreOrderNumber = packageData.PreOrderNumber;
        //                p.OrderNumber = packageData.OrderNumber;
        //                p.PickTaskId = packageData.PickTaskId;
        //                p.Creator = _userManager.Account;
        //                p.CreationTime = DateTime.Now;
        //            }
        //            PackageAcquisitions.AddRange(PackageAcquisition);
        //        }
        //    }
        //    packageData.ExpressCompany = request.ExpressCompany;
        //    packageData.GrossWeight = request.Weight;
        //    packageData.NetWeight = request.Weight;
        //    packageData.Id = 0;
        //    //try
        //    //{
        //    await _repPackage.Context.InsertNav(packageData).Include(a => a.Details).ExecuteCommandAsync();
        //    await _repRFPackageAcquisition.InsertRangeAsync(PackageAcquisitions);
        //    //await _repPackage.InsertAsync();
        //    //}
        //    //catch (Exception asdas)
        //    //{
        //    //    throw;
        //    //}
        //    //_sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, null);
        //    pickData.ForEach(a =>
        //    {

        //        a.PackageQty += a.ScanQty;
        //        a.ScanQty = 0;
        //    });
        //    _sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, pickData, timeSpan);
        //    //判断是否包装完成
        //    var CheckPackageData = _repPackage.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber).Sum(a => a.DetailCount);
        //    var CheckPickData = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber).ToListAsync();
        //    if (CheckPackageData >= CheckPickData.Sum(a => a.PickQty) || packageBox == PackageBoxTypeEnum.短包)
        //    {
        //        await _repPickTask.UpdateAsync(a => new WMSPickTask { PickStatus = (int)PickTaskStatusEnum.包装完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, (a => a.PickTaskNumber == request.PickTaskNumber));
        //        await _repPickTaskDetail.UpdateAsync(a => new WMSPickTaskDetail { PickStatus = (int)PickTaskStatusEnum.包装完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, (a => a.PickTaskNumber == request.PickTaskNumber));
        //        await _repOrder.UpdateAsync(a => new WMSOrder { OrderStatus = (int)OrderStatusEnum.已包装 }, (a => CheckPickData.Select(b => b.OrderId).ToList().Contains(a.Id)));
        //        //_repPickTask.UpdateAsync(a => a.PickStatus = (int)PickTaskStatusEnum.包装完成);
        //        //_repPickTaskDetail.UpdateAsync(a => a.PickStatus = (int)PickTaskStatusEnum.包装完成);
        //        _sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, null);
        //        response.Code = StatusCode.Finish;
        //        response.Msg = "订单完成";
        //        return response;
        //    }
        //    response.Code = StatusCode.Success;
        //    response.Msg = "成功";
        //    return response;
        //    //return response;
        //}
        //else
        //{
        //    response.Code = StatusCode.Error;
        //    response.Msg = "请输入重量";
        //    return response;
        //    //return response;
        //}
        //PackageData. = PackageDetailData.Sum(a => a.Qty);
    }



    private async Task<Response> PackingRFIDComplete(List<PackageData> pickData, ScanPackageRFIDInput request, PackageBoxTypeEnum packageBox)
    {
        Response response = new Response();
        if (request.Weight < 0.3)
        {
            request.Weight = 1;
        }
        //foreach (var item in pickData.Select(a=>a.SKU))
        //{

        //}
        List<WMSRFIDInfo> rfidInfos = new List<WMSRFIDInfo>();

        rfidInfos.AddRange(pickData.First().RFIDInfo);
        if (pickData.First() != null && pickData.First().RFIDInfoOld != null)
        {
            rfidInfos.AddRange(pickData.First().RFIDInfoOld);
        }
        var lookup = rfidInfos.ToLookup(a => a.RFID);
        var distinctRFID = lookup.Select(g => g.First()).ToList();
        //判断需要包装的数量和RFID的数量
        if (pickData.Sum(a => a.PickQty) < distinctRFID.Count)
        {
            response.Code = StatusCode.Error;
            response.Msg = "RFID数量超出拣货数量";
            return response;
        }
        //判断是不是输入了重量
        if (request.Weight > 0.2)
        {
            var pickDataTemp = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber && a.PickStatus == (int)PickTaskStatusEnum.拣货完成)
                 .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                 .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0).FirstAsync();
            var packageNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WMSPickTaskDetail, WMSPackage>()
                   //添加创建人为当前用户
                   .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                   .ForMember(a => a.PickTaskId, opt => opt.MapFrom(c => c.PickTaskId))
                   .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                   .ForMember(a => a.PackageTime, opt => opt.MapFrom(c => DateTime.Now))
                   // 
                   .ForMember(a => a.PackageStatus, opt => opt.MapFrom(c => PackageStatusEnum.完成))
                   .ForMember(a => a.Id, opt => opt.Ignore())
                   //将为Null的字段设置为"" () 
                   .AddTransform<string>(a => a == null ? "" : a)
                   //将为Null的字段设置为"" () 
                   .AddTransform<int?>(a => a == null ? 0 : a)
                   //将为Null的字段设置为"" () 
                   .AddTransform<double?>(a => a == null ? 0 : a)
                   //添加
                   .ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber));
                //.AddTransform<string>(a => a == null ? "" : a);

                cfg.CreateMap<PackageData, WMSPackageDetail>()
                  //添加创建人为当前用户
                  .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                  .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                  .ForMember(a => a.Qty, opt => opt.MapFrom(c => c.ScanQty))
                   //将为Null的字段设置为"" () 
                   .AddTransform<int?>(a => a == null ? 0 : a)
                   //将为Null的字段设置为"" () 
                   .AddTransform<double?>(a => a == null ? 0 : a)
                  //添加
                  .ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber));
            });

            foreach (var item in pickData)
            {
                if (item.RFIDInfoOld == null)
                {
                    item.RFIDInfoOld = new List<WMSRFIDInfo>();
                }
                item.RFIDInfoOld.AddRange(item.RFIDInfo);
            }
            //TextHelper.WrittxtFor("准备更新RFID +request", "/File/TextLog", "RFIDLog" + DateTime.Now.ToString("yyyyMMddhh") + ".txt");
            //TextHelper.WrittxtFor(JsonSerializer.Serialize(request), "/File/TextLog", "RFIDLog" + DateTime.Now.ToString("yyyyMMddhh") + ".txt");
            //TextHelper.WrittxtFor("准备更新RFID +pickData", "/File/TextLog", "RFIDLog" + DateTime.Now.ToString("yyyyMMddhh") + ".txt");
            //TextHelper.WrittxtFor(JsonSerializer.Serialize(pickData.First().RFIDs), "/File/TextLog", "RFIDLog" + DateTime.Now.ToString("yyyyMMddhh") + ".txt");

            var mapper = new Mapper(config);
            var packageData = mapper.Map<WMSPackage>(pickDataTemp);
            var packageDetailData = mapper.Map<List<WMSPackageDetail>>(pickData.Where(a => a.ScanQty > 0));
            //var packageDetailDetail = .WMSRFPackageAcquisition
            //var packagenumberData = await _repPackage.AsQueryable().Where(a => a.OrderId == pickDataTemp.OrderId).ToListAsync();
            var orderDn = await _repOrder.AsQueryable().Where(a => a.Id == pickDataTemp.OrderId).FirstAsync();
            List<WMSOrder> orderSo = new List<WMSOrder>();
            if (orderDn != null && !string.IsNullOrEmpty(orderDn.Dn))
            {
                orderSo = await _repOrder.AsQueryable().Where(a => a.Dn == orderDn.Dn && orderDn.CustomerId == a.CustomerId).ToListAsync();

            }
            else
            {
                orderSo = await _repOrder.AsQueryable().Where(a => a.Id == pickDataTemp.OrderId && orderDn.CustomerId == a.CustomerId).ToListAsync();

            }
            var packagenumberData = await _repPackage.AsQueryable().Where(a => orderSo.Select(b => b.Id).Contains(a.OrderId)).ToListAsync();

            packageData.DetailCount = packageDetailData.Sum(a => a.Qty);
            packageData.Details = packageDetailData;
            packageData.Details.ForEach(a =>
            {
                a.CustomerId = packageData.CustomerId;
                a.CustomerName = packageData.CustomerName;
                a.WarehouseId = packageData.WarehouseId;
                a.WarehouseName = packageData.WarehouseName;
                a.OrderId = packageData.OrderId;
                a.ExternOrderNumber = packageData.ExternOrderNumber;
                a.PreOrderNumber = packageData.PreOrderNumber;
                a.OrderNumber = packageData.OrderNumber;
                a.PickTaskId = packageData.PickTaskId;
                //a.Weight = 0;
            });

            List<WMSRFPackageAcquisition> PackageAcquisitions = new List<WMSRFPackageAcquisition>();
            foreach (var item in pickData)
            {
                var PackageAcquisition = item.ScanPackageInput.Adapt<List<WMSRFPackageAcquisition>>();
                if (PackageAcquisition != null)
                {
                    foreach (var p in PackageAcquisition)
                    {
                        p.Qty = 1;
                        p.CustomerId = packageData.CustomerId;
                        p.CustomerName = packageData.CustomerName;
                        p.WarehouseId = packageData.WarehouseId;
                        p.WarehouseName = packageData.WarehouseName;
                        p.OrderId = packageData.OrderId;
                        p.ExternOrderNumber = packageData.ExternOrderNumber;
                        p.PreOrderNumber = packageData.PreOrderNumber;
                        p.PackageNumber = packageNumber;
                        p.OrderNumber = packageData.OrderNumber;
                        p.PickTaskId = packageData.PickTaskId;
                        p.Creator = _userManager.Account;
                        p.CreationTime = DateTime.Now;
                    }
                    PackageAcquisitions.AddRange(PackageAcquisition);
                }
            }

            var result = await _repRFIDInfo.AsQueryable().Where(p => pickData.First().RFIDInfo.Select(q => q.RFID).Contains(p.RFID) && p.Status == (int)RFIDStatusEnum.新增
            && packageData.CustomerId == packageData.CustomerId
            ).ToListAsync();
            //判断需要包装的数量和RFID的数量
            if (pickData.Sum(a => a.PickQty) < result.Count)
            {
                response.Code = StatusCode.Error;
                response.Msg = "RFID数量超出拣货数量";
                return response;
            }
            foreach (var item in result)
            {
                item.Status = (int)RFIDStatusEnum.出库;
                item.PackageNumber = packageNumber;
                item.Sequence = pickData.First().RFIDInfo.Where(a => a.RFID.Contains(item.RFID)).FirstOrDefault().Sequence;
                item.ExternOrderNumber = packageData.ExternOrderNumber;
                item.PickTaskNumber = packageData.PickTaskNumber;
                item.OrderTime = DateTime.Now;
                item.OrderPerson = _userManager.Account;
                item.PickTaskNumber = pickData.FirstOrDefault().PickTaskNumber;
                item.OrderNumber = packageData.OrderNumber;
            }
            await _repRFIDInfo.UpdateRangeAsync(result);

            //包装的时候，将RFID  状态修改成为10 标识RFID 已经拣货包装
            //_repRFIDInfo.UpdateAsync(a => a.Status = (int)RFIDStatusEnum.包装).Where(a => request.RFIDs.Contains(a.RFID));
            //   await _repRFIDInfo.Context.Updateable<WMSRFIDInfo>()
            //.SetColumns(p => p.Status == (int)RFIDStatusEnum.出库)
            //.SetColumns(p => p.OrderTime == DateTime.Now)
            //.SetColumns(p => p.OrderPerson == _userManager.Account)
            ////.SetColumns(p => p.OrderNumber == request.RFIDInfo.Where(a=>a.RFID.Contains(p.RFID)).FirstOrDefault().Sequence)
            //.SetColumns(p => p.ExternOrderNumber == packageData.ExternOrderNumber)
            ////.SetColumns(p => p.Sequence == packageData.ExternOrderNumber)
            ////.SetColumns(p => p.PreOrderId == packageData.PreOrderId)
            //.SetColumns(p => p.PickTaskNumber == pickData.FirstOrDefault().PickTaskNumber)
            ////.SetColumns(p => p.ExternOrderNumber == pickData.FirstOrDefault().PickTaskNumber)
            //.Where(p => request.RFIDs.Contains(p.RFID) && p.Status == (int)RFIDStatusEnum.新增)
            //.ExecuteCommandAsync();

            packageData.ExpressCompany = request.ExpressCompany;
            packageData.GrossWeight = request.Weight;
            packageData.NetWeight = request.Weight;
            packageData.Id = 0;
            packageData.PackageType = request.BoxType;
            packageData.SerialNumber = (packagenumberData.Count + 1).ToString();

            //packageData.SerialNumber=,
            await _repPackage.Context.InsertNav(packageData).Include(a => a.Details).ExecuteCommandAsync();
            await _repRFPackageAcquisition.InsertRangeAsync(PackageAcquisitions);
            //await _repPackage.InsertAsync();

            //_sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, null);
            pickData.ForEach(a =>
            {

                a.PackageQty += a.ScanQty;
                a.ScanQty = 0;
            });
            _sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, pickData, timeSpan);
            //判断是否包装完成
            var CheckPackageData = _repPackage.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber).Sum(a => a.DetailCount);
            var CheckPickData = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber).ToListAsync();
            if (CheckPackageData >= CheckPickData.Sum(a => a.PickQty) || packageBox == PackageBoxTypeEnum.短包)
            {
               
                await _repPickTask.UpdateAsync(a => new WMSPickTask { PickStatus = (int)PickTaskStatusEnum.包装完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, (a => a.PickTaskNumber == request.PickTaskNumber));
                await _repPickTaskDetail.UpdateAsync(a => new WMSPickTaskDetail { PickStatus = (int)PickTaskStatusEnum.包装完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, (a => a.PickTaskNumber == request.PickTaskNumber));
                await _repOrder.UpdateAsync(a => new WMSOrder { OrderStatus = (int)OrderStatusEnum.已包装 }, (a => CheckPickData.Select(b => b.OrderId).ToList().Contains(a.Id)));
                //_repPickTask.UpdateAsync(a => a.PickStatus = (int)PickTaskStatusEnum.包装完成);
                //_repPickTaskDetail.UpdateAsync(a => a.PickStatus = (int)PickTaskStatusEnum.包装完成);
                _sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, null);

                List<WMSInstruction> wMSInstructions = new List<WMSInstruction>();
                foreach (var item in CheckPickData.GroupBy(a => new { a.CustomerId, a.CustomerName, a.WarehouseId, a.WarehouseName, a.OrderId, a.ExternOrderNumber }).Distinct().ToList())
                {
                    //插入反馈指令
                    WMSInstruction wMSInstruction = new WMSInstruction();
                    //wMSInstruction.OrderId = orderData[0].Id;
                    wMSInstruction.InstructionStatus = (int)InstructionStatusEnum.新增;
                    wMSInstruction.InstructionType = "出库单回传HachDG";
                    wMSInstruction.BusinessType = "出库单回传HachDG";
                    //wMSInstruction.InstructionTaskNo = DateTime.Now;
                    wMSInstruction.CustomerId = item.Key.CustomerId;
                    wMSInstruction.CustomerName = item.Key.CustomerName;
                    wMSInstruction.WarehouseId = item.Key.WarehouseId;
                    wMSInstruction.WarehouseName = item.Key.WarehouseName;
                    wMSInstruction.OperationId = item.Key.OrderId;
                    wMSInstruction.OrderNumber = item.Key.ExternOrderNumber;
                    wMSInstruction.Creator = _userManager.Account;
                    wMSInstruction.CreationTime = DateTime.Now;
                    wMSInstruction.InstructionTaskNo = item.Key.ExternOrderNumber;
                    wMSInstruction.TableName = "WMS_Order";
                    wMSInstruction.InstructionPriority = 63;
                    wMSInstruction.Remark = "";
                    //var getInstruction63 = await _repInstruction.AsQueryable().Where(a => a.OperationId == item.Key.OrderId && a.BusinessType == "出库单回传HachDG").ToListAsync();
                    //if (getInstruction63 == null && getInstruction63.Count == 0)
                    //{
                    wMSInstructions.Add(wMSInstruction);
                    //}
                }
                //wMSInstructions.Add(wMSInstruction);
                //出库装箱回传判断DN 是不是都完成了。ND下的所有的so 都完成才可以插入出库装箱回传 (客户系统需要对接WMS)
                //让安琪将DN 字段对接到业务表中 STR1 可以通过dn 字段来判断是不是所有的dn 都已经完成，那么可以插入装箱信息
                //判断里面有哪些DN 
                //var checkDn = await _repOrder.AsQueryable()
                //    .Where(a => CheckPickData.Select(b => b.OrderNumber).Contains(a.OrderNumber) && !string.IsNullOrEmpty(a.Dn))
                //    .Select(a => new
                //    {
                //        a.Dn,
                //        a.CustomerId,
                //        a.CustomerName,
                //        a.WarehouseId,
                //        a.WarehouseName,
                //        a.Id
                //    }).FirstAsync();
                //if (checkDn != null && !string.IsNullOrEmpty(checkDn.Dn))
                //{
                //    //foreach (var item in checkDn)
                //    //{
                //    var checkOrderDN = await _repOrder.AsQueryable().Where(a => a.Dn == checkDn.Dn && a.OrderStatus < (int)OrderStatusEnum.已包装).ToListAsync();
                //    //已经转出库单的都已经完成， 且预出库单没有新增
                //    var checkPreOrderDN = await _repPreOrder.AsQueryable().Where(a => a.Dn == checkDn.Dn && a.PreOrderStatus == (int)PreOrderStatusEnum.新增).ToListAsync();

                //    if ((checkOrderDN == null || checkOrderDN.Count == 0) && checkPreOrderDN.Count == 0)
                //    {
                //        WMSInstruction wMSInstructionSNGRHach = new WMSInstruction();
                //        //wMSInstruction.OrderId = orderData[0].Id;
                //        wMSInstructionSNGRHach.InstructionStatus = (int)InstructionStatusEnum.新增;
                //        wMSInstructionSNGRHach.InstructionType = "出库装箱回传HachDG";
                //        wMSInstructionSNGRHach.BusinessType = "出库装箱回传HachDG";
                //        //wMSInstruction.InstructionTaskNo = DateTime.Now;
                //        wMSInstructionSNGRHach.CustomerId = checkDn.CustomerId;
                //        wMSInstructionSNGRHach.CustomerName = checkDn.CustomerName;
                //        wMSInstructionSNGRHach.WarehouseId = checkDn.WarehouseId;
                //        wMSInstructionSNGRHach.WarehouseName = checkDn.WarehouseName;
                //        wMSInstructionSNGRHach.OperationId = checkDn.Id;
                //        wMSInstructionSNGRHach.OrderNumber = checkDn.Dn ?? "";
                //        wMSInstructionSNGRHach.Creator = _userManager.Account;
                //        wMSInstructionSNGRHach.CreationTime = DateTime.Now;
                //        wMSInstructionSNGRHach.InstructionTaskNo = checkDn.Dn ?? "";
                //        wMSInstructionSNGRHach.TableName = "WMS_Order";
                //        wMSInstructionSNGRHach.InstructionPriority = 4;
                //        wMSInstructionSNGRHach.Remark = "";
                //        //判断是否插入过一次
                //        var getInstruction = await _repInstruction.AsQueryable().Where(a => a.CustomerId == checkDn.CustomerId && a.OrderNumber == checkDn.Dn && a.BusinessType == "出库装箱回传HachDG").ToListAsync();
                //        if (getInstruction == null || getInstruction.Count == 0)
                //        {
                //            if (!string.IsNullOrEmpty(checkDn.Dn))
                //            {
                //                if (wMSInstructions.Where(a => a.OperationId == checkDn.Id && a.InstructionType == "出库装箱回传HachDG").Count() == 0)
                //                {
                //                    wMSInstructions.Add(wMSInstructionSNGRHach);
                //                }
                //            }
                //        }
                //    }
                //}
                //判断是不是回传过
                await _repInstruction.InsertRangeAsync(wMSInstructions);

                response.Code = StatusCode.Finish;
                response.Msg = "订单完成";
                return response;
            }
            response.Code = StatusCode.Success;
            response.Msg = "成功";
            return response;
            //return response;
        }
        else
        {
            response.Code = StatusCode.Error;
            response.Msg = "请输入重量";
            return response;
            //return response;
        }
        //PackageData. = PackageDetailData.Sum(a => a.Qty);
    }


    private async Task<Response> PackingRFIDComplete_Scan(List<PackageData> pickData, ScanPackageInput request, PackageBoxTypeEnum packageBox)
    {
        Response response = new Response();

        //判断是不是输入了重量
        if (request.Weight > 0.2)
        {
            var pickDataTemp = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber && a.PickStatus == (int)PickTaskStatusEnum.拣货完成)
                 .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                 .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0).FirstAsync();
            var packageNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WMSPickTaskDetail, WMSPackage>()
                   //添加创建人为当前用户
                   .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                   .ForMember(a => a.PickTaskId, opt => opt.MapFrom(c => c.PickTaskId))
                   .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                   .ForMember(a => a.PackageTime, opt => opt.MapFrom(c => DateTime.Now))
                   // 
                   .ForMember(a => a.PackageStatus, opt => opt.MapFrom(c => PackageStatusEnum.完成))
                   .ForMember(a => a.Id, opt => opt.Ignore())
                   //将为Null的字段设置为"" () 
                   .AddTransform<string>(a => a == null ? "" : a)
                   //将为Null的字段设置为"" () 
                   .AddTransform<int?>(a => a == null ? 0 : a)
                   //将为Null的字段设置为"" () 
                   .AddTransform<double?>(a => a == null ? 0 : a)
                   //添加
                   .ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber));
                //.AddTransform<string>(a => a == null ? "" : a);

                cfg.CreateMap<PackageData, WMSPackageDetail>()
                  //添加创建人为当前用户
                  .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                  .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                  .ForMember(a => a.Qty, opt => opt.MapFrom(c => c.ScanQty))
                   //将为Null的字段设置为"" () 
                   .AddTransform<int?>(a => a == null ? 0 : a)
                   //将为Null的字段设置为"" () 
                   .AddTransform<double?>(a => a == null ? 0 : a)
                  //添加
                  .ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber));
            });


            var mapper = new Mapper(config);
            var packageData = mapper.Map<WMSPackage>(pickDataTemp);
            var packageDetailData = mapper.Map<List<WMSPackageDetail>>(pickData.Where(a => a.ScanQty > 0));
            //var packageDetailDetail = .WMSRFPackageAcquisition
            var packagenumberData = await _repPackage.AsQueryable().Where(a => a.OrderId == pickDataTemp.OrderId).ToListAsync();

            packageData.DetailCount = packageDetailData.Sum(a => a.Qty);
            packageData.Details = packageDetailData;
            packageData.Details.ForEach(a =>
            {
                a.CustomerId = packageData.CustomerId;
                a.CustomerName = packageData.CustomerName;
                a.WarehouseId = packageData.WarehouseId;
                a.WarehouseName = packageData.WarehouseName;
                a.OrderId = packageData.OrderId;
                a.ExternOrderNumber = packageData.ExternOrderNumber;
                a.PreOrderNumber = packageData.PreOrderNumber;
                a.OrderNumber = packageData.OrderNumber;
                a.PickTaskId = packageData.PickTaskId;
                //a.Weight = 0;
            });

            List<WMSRFPackageAcquisition> PackageAcquisitions = new List<WMSRFPackageAcquisition>();
            foreach (var item in pickData)
            {
                var PackageAcquisition = item.ScanPackageInput.Adapt<List<WMSRFPackageAcquisition>>();
                if (PackageAcquisition != null)
                {
                    foreach (var p in PackageAcquisition)
                    {
                        p.Qty = 1;
                        p.CustomerId = packageData.CustomerId;
                        p.CustomerName = packageData.CustomerName;
                        p.WarehouseId = packageData.WarehouseId;
                        p.WarehouseName = packageData.WarehouseName;
                        p.OrderId = packageData.OrderId;
                        p.ExternOrderNumber = packageData.ExternOrderNumber;
                        p.PreOrderNumber = packageData.PreOrderNumber;
                        p.OrderNumber = packageData.OrderNumber;
                        p.PickTaskId = packageData.PickTaskId;
                        p.Creator = _userManager.Account;
                        p.CreationTime = DateTime.Now;
                    }
                    PackageAcquisitions.AddRange(PackageAcquisition);
                }
            }
            //包装的时候，将RFID  状态修改成为10 标识RFID 已经拣货包装
            //_repRFIDInfo.UpdateAsync(a => a.Status =(int)RFIDStatusEnum.包装).Where(a => request.RFIDs.Contains(a.RFID));
            await _repRFIDInfo.Context.Updateable<WMSRFIDInfo>()
               .SetColumns(p => p.Status == (int)RFIDStatusEnum.出库)
               .SetColumns(p => p.OrderTime == DateTime.Now)
               .SetColumns(p => p.OrderPerson == _userManager.Account)
               .SetColumns(p => p.OrderNumber == packageData.OrderNumber)
               .SetColumns(p => p.ExternOrderNumber == packageData.ExternOrderNumber)
               //.SetColumns(p => p.PreOrderId == packageData.PreOrderId)
               .SetColumns(p => p.PickTaskNumber == pickData.FirstOrDefault().PickTaskNumber)
               //.SetColumns(p => p.ExternOrderNumber == pickData.FirstOrDefault().PickTaskNumber)
               .Where(p => request.RFIDs.Contains(p.RFID) && p.Status == (int)RFIDStatusEnum.新增)
               .ExecuteCommandAsync();

            packageData.ExpressCompany = request.ExpressCompany;
            packageData.GrossWeight = request.Weight;
            packageData.NetWeight = request.Weight;
            packageData.Id = 0;
            packageData.SerialNumber = (packagenumberData.Count + 1).ToString();

            //try
            //{
            await _repPackage.Context.InsertNav(packageData).Include(a => a.Details).ExecuteCommandAsync();
            await _repRFPackageAcquisition.InsertRangeAsync(PackageAcquisitions);
            //await _repPackage.InsertAsync();
            //}
            //catch (Exception asdas)
            //{
            //    throw;
            //}
            //_sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, null);
            pickData.ForEach(a =>
            {

                a.PackageQty += a.ScanQty;
                a.ScanQty = 0;
            });
            _sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, pickData, timeSpan);
            //判断是否包装完成
            var CheckPackageData = _repPackage.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber).Sum(a => a.DetailCount);
            var CheckPickData = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber).ToListAsync();
            if (CheckPackageData >= CheckPickData.Sum(a => a.PickQty) || packageBox == PackageBoxTypeEnum.短包)
            {
                await _repPickTask.UpdateAsync(a => new WMSPickTask { PickStatus = (int)PickTaskStatusEnum.包装完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, (a => a.PickTaskNumber == request.PickTaskNumber));
                await _repPickTaskDetail.UpdateAsync(a => new WMSPickTaskDetail { PickStatus = (int)PickTaskStatusEnum.包装完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, (a => a.PickTaskNumber == request.PickTaskNumber));
                await _repOrder.UpdateAsync(a => new WMSOrder { OrderStatus = (int)OrderStatusEnum.已包装 }, (a => CheckPickData.Select(b => b.OrderId).ToList().Contains(a.Id)));
                //_repPickTask.UpdateAsync(a => a.PickStatus = (int)PickTaskStatusEnum.包装完成);
                //_repPickTaskDetail.UpdateAsync(a => a.PickStatus = (int)PickTaskStatusEnum.包装完成);
                _sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, null);
                response.Code = StatusCode.Finish;
                response.Msg = "订单完成";
                return response;
            }
            response.Code = StatusCode.Success;
            response.Msg = "成功";
            return response;
            //return response;
        }
        else
        {
            response.Code = StatusCode.Error;
            response.Msg = "请输入重量";
            return response;
            //return response;
        }
        //PackageData. = PackageDetailData.Sum(a => a.Qty);
    }
    /// <summary>
    /// 校验包装
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Response<ScanPackageOutput>> VerifyPackage(ScanPackageRFIDInput request)
    {
        //request.RFIDs = request.RFIDStr.Split(',').ToList();
        //request.PickTaskNumber = "160615271544128";
        //request.Input = "160615271544128";
        List<string> rftds = new List<string>();
        //Response<Dictionary<string, string>> response = new Response<Dictionary<string, string>>();
        foreach (var item in request.RFIDStr.Split(',').ToList())
        {

            //校验位 以24位为基准，多于24位则截取前24位，少于24位则反馈错误信息
            if (string.IsNullOrEmpty(item) || item.Length < 24)
            {
                //throw Oops.Oh(ErrorCodeEnum.D1002);
                continue;
            }
            if (string.IsNullOrEmpty(item) || item.Length >= 24)
            {
                //截取前24位
                rftds.Add(item.Substring(0, 24));
            }
        }
        request.RFIDs = rftds;

        Response<ScanPackageOutput> response = new Response<ScanPackageOutput>() { Data = new ScanPackageOutput() };


        response.Data.PickTaskNumber = request.PickTaskNumber;
        response.Data.SKU = request.SKU;
        response.Data.SN = request.SN;
        response.Data.Input = request.Input;
        response.Data.AcquisitionData = request.AcquisitionData;
        response.Data.WMSRFIDInfos = request.RFIDInfo;


        //根据拣货任务号获取订单信息
        var getOrderData = _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber).ToList();
        //获取RFID的入库信息 getOrderData.Select(a => a.PoCode).Contains(a.PoCode) &&
        var getRFIDData = _repRFIDInfo.AsQueryable().Where(a => request.RFIDs.Contains(a.RFID) && a.Status == (int)RFIDStatusEnum.新增).ToList();
        if (string.IsNullOrEmpty(request.PickTaskNumber))
        {
            response.Code = StatusCode.Error;
            response.Msg = "请输入拣货任务号";
            return response;
        }
        //比较得到的RFID的数量和允许出库的数量是否一致  
        //如果一致，则将RFID的状态设置为出库中，并记录出库信息
        if (request.RFIDs.Count > getRFIDData.Count)
        {
            response.Code = StatusCode.Error;
            response.Msg = "有不合法的RFID";
            response.Data.PackageDatas = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);
            return response;
        }

        List<PackageData> pickData = new List<PackageData>();

        if (!string.IsNullOrEmpty(request.PickTaskNumber))
        {
            pickData = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);

            //判断是不是包装完成，补充了重量  缓存有数据， 取缓存返回
            if (pickData != null && pickData.Count > 0 && pickData.Where(a => (a.PackageQty + a.ScanQty) != a.PickQty).Count() == 0)
            {
                //判断有没有扫描数据
                if (pickData.Count > 0 && pickData.Where(a => a.ScanQty > 0).Count() > 0)
                {

                    var result = await PackingRFIDComplete(pickData, request, PackageBoxTypeEnum.正常);
                    response.Code = result.Code;
                    response.Msg = result.Msg;
                    return response;
                }
                else
                {
                    //说明已经包装完成，删除缓存；
                    _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, null);
                    response.Code = StatusCode.Error;
                    response.Msg = "该拣货任务已经包装完成";
                    return response;
                }
            }
            if (pickData == null)
            {
                request.PickTaskNumber = "";
            }

        }


        //每次进来先初始化缓存，提交才固定缓存

        //判断是不是第一次加载，要是缓存中有数据，直接取缓存
        //1，判断有没有拣货单号
        if (string.IsNullOrEmpty(request.PickTaskNumber))
        {
            response.Data.PickTaskNumber = request.Input;
            request.PickTaskNumber = request.Input;
            pickData = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);
            if (pickData != null && pickData.Count > 0)
            {
                response.Data.PackageDatas = pickData;
                //response.Data.PickTaskNumber = request.PickTaskNumber;
                //response.Data.SKU = request.SKU;
                //response.Data.SN = request.SN;
                //response.Data.Input = request.Input;
                //response.Data.AcquisitionData = request.AcquisitionData;
                response.Code = StatusCode.Success;
                return response;
            }



            var CheckPickData = _repPickTask.AsQueryable().Where(a => a.PickTaskNumber == response.Data.PickTaskNumber)
                .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
            .ToList();
            if (CheckPickData.Where(a => a.PickStatus == (int)PickTaskStatusEnum.包装完成).Count() > 0)
            {

                response.Code = StatusCode.Error;
                response.Msg = "拣货单已经完成包装";
                return response;
            }
            else if (CheckPickData.Where(a => a.PickStatus == (int)PickTaskStatusEnum.拣货完成).Count() == 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "拣货单还未完成拣货";
                return response;

            }
            else if (CheckPickData.Count == 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "拣货单号不存在";
                return response;
            }


            pickData = _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == response.Data.PickTaskNumber && a.PickStatus == (int)PickTaskStatusEnum.拣货完成)
                  .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                  .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
              .GroupBy(a => new { a.SKU, a.PickTaskNumber })
              .Select(a => new PackageData { SKU = a.SKU, PickQty = SqlFunc.AggregateSum(a.PickQty), RemainingQty = SqlFunc.AggregateSum(a.PickQty), PickTaskNumber = a.PickTaskNumber, GoodsName = SqlFunc.AggregateMax(a.GoodsName), GoodsType = SqlFunc.AggregateMax(a.GoodsType) })
              .ToList();

            if (pickData.Count > 0)
            {

                _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, pickData, timeSpan);
                response.Data.PackageDatas = pickData;
                response.Code = StatusCode.Success;
                return response;
            }


        }
        //<<<<<<< HEAD
        foreach (var item in getRFIDData)
        {
            var Sequence = request.RFIDInfo.Where(a => a.RFID == item.RFID).FirstOrDefault();
            if (Sequence != null)
            {
                item.Sequence = Sequence.Sequence;
            }
        }

        List<WMSRFIDInfo> rfidInfos = new List<WMSRFIDInfo>();

        //        rfidInfos.AddRange(getRFIDData);
        //        if (pickData.First() != null && pickData.First().RFIDInfoOld != null)
        //        {
        //=======
        //        List<WMSRFIDInfo> rfidInfos = new List<WMSRFIDInfo>();

        rfidInfos.AddRange(getRFIDData);
        if (pickData.First() != null && pickData.First().RFIDInfoOld != null)
        {
            //>>>>>>> 5c83cb3 (提交最新代码)
            rfidInfos.AddRange(pickData.First().RFIDInfoOld);
        }
        var lookup = rfidInfos.ToLookup(a => a.RFID);
        var distinctRFID = lookup.Select(g => g.First()).ToList();
        pickData.ForEach(a =>
        {
            a.RemainingQty = a.PickQty - distinctRFID.Where(p => p.SKU == a.SKU && p.Status == (int)RFIDStatusEnum.新增).Count();
            a.ScanQty = getRFIDData.Where(p => p.SKU == a.SKU && p.Status == (int)RFIDStatusEnum.新增).Count();
            a.RFIDInfo = getRFIDData;
        });
        _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, pickData, timeSpan);

        if (pickData.Sum(a => a.PickQty) < distinctRFID.Count())
        {
            response.Code = StatusCode.Warning;
            response.Msg = "有不合法的RFID";
            response.Data.PackageDatas = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);
            return response;
        }

        foreach (var item in getRFIDData)
        {
            response.Data.SKU = item.SKU;
            response.Data.PickTaskNumber = request.PickTaskNumber;

            //判断有没有SN,有SN 就记录出库SN
            //WMSRFPackageAcquisition wMSRF=new WMSRFPackageAcquisition();
            //wMSRF.
            //_repRFPackageAcquisition
            var PickSKUData = pickData.Where(a => a.SKU == item.SKU);
            if (PickSKUData.Count() > 0)
            {
                if (PickSKUData.Sum(a => (a.ScanQty + a.PackageQty)) + 1 <= PickSKUData.First().PickQty)
                {
                    var pick = pickData.Where(a => a.SKU == item.SKU && a.PickQty > (a.ScanQty + a.PackageQty)).First();
                    //pick.ScanQty += 1;
                    //pick.RemainingQty -= 1;
                    //pick.ScanPackageRFIDInput = new List<ScanPackageRFIDInput>();
                    //pick.ScanPackageRFIDInput.Add(request);
                    _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, pickData, timeSpan);
                    //判断是不是包装完成
                    //if (pickData.Where(a => (a.ScanQty + a.PackageQty) != a.PickQty).Count() == 0)
                    //{
                    //    var result = await PackingRFIDComplete(pickData, request, PackageBoxTypeEnum.正常);
                    //    response.Data.PackageDatas = pickData;
                    //    response.Code = result.Code;
                    //    response.Msg = result.Msg;
                    //    return response;
                    //}
                }
                else
                {
                    var pick = pickData.Where(a => a.SKU == item.SKU).First();
                    //pick.ScanQty += 1;
                    //pick.RemainingQty -= 1;
                    //pick.ScanPackageRFIDInput = new List<ScanPackageRFIDInput>();
                    //pick.ScanPackageRFIDInput.Add(request);
                    _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, pickData, timeSpan);
                }
            }
            else
            {
                response.Data.PackageDatas = pickData;
                response.Code = StatusCode.Error;
                response.Msg = "SKU 不存在";
                return response;
            }

        }

        //判断最终扫描的RFID 数量是否等于 允许出库的数量，如果一致，则将RFID的状态设置为出库中，并记录出库信息

        if (pickData.Where(a => (a.ScanQty + a.PackageQty) != a.PickQty).Count() == 0)
        {
            var result = await PackingRFIDComplete(pickData, request, PackageBoxTypeEnum.正常);
            response.Data.PackageDatas = pickData;
            response.Code = result.Code;
            response.Msg = result.Msg;
            return response;
        }
        else
        {
            response.Data.PackageDatas = pickData;
            response.Code = StatusCode.Error;
            response.Msg = "SKU数量有差异";
            return response;
        }

        //response.Data.PackageDatas = pickData;
        //response.Code = StatusCode.Success;
        ////return response;

        ////response.Code = StatusCode.Success;
        //response.Msg = "成功";
        //return response;

        //比较RFID出库信息是否合理



    }
}
