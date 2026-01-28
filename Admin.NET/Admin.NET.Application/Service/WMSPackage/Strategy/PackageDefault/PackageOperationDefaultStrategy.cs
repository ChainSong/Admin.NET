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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using XAct;

namespace Admin.NET.Application.Service;
internal class PackageOperationDefaultStrategy : IPackageOperationInterface
{

    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSProductBom> _repProductBom { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public UserManager _userManager { get; set; }
    //public ISqlSugarClient _db { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }
    public SysCacheService _sysCacheService { get; set; }
    public SqlSugarRepository<WMSProduct> _repProduct { get; set; }
    public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }
    public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
    public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }
    public SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo { get; set; }

    public SqlSugarRepository<WMSRFPackageAcquisition> _repRFPackageAcquisition { get; set; }
    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    TimeSpan timeSpan = new TimeSpan(72, 0, 0);
    public async Task<Response<ScanPackageOutput>> GetPackage(ScanPackageInput request)
    {
        request.SN = "";
        request.Lot = "";
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
                //string LOTRegex = @"(?<=\|LOT)[^\|]+|(?<==\d{3}|=\d{4}|=\d({4}CN)[0-9]{5}\b|(?<=\s)[A-Z0-9]{1,5}\b";
                string LOTRegex = @"(?<=\|LOT)[^\|]+|(?<==\d{3}|=\d{4}|=\d{4}CN)[0-9]{5}\b|(?<=\s)[A-Z0-9]{1,5}\b";
                string ExpirationDateRegex = @"(?<=\|EXP)[^\|]+|(?<=\s)\d{6}\b";
                MatchCollection matchesSKU = Regex.Matches(request.Input, SKURegex);
                request.SKU = matchesSKU.Count > 0 ? matchesSKU[0].Value : "";
                //request.Input = request.SKU;

                MatchCollection matchesExpirationDateRegex = Regex.Matches(request.Input, ExpirationDateRegex);
                request.AcquisitionData = matchesExpirationDateRegex.Count > 0 ? matchesExpirationDateRegex[0].Value : "";
                MatchCollection matchesLOT = Regex.Matches(request.Input, LOTRegex);
                request.Lot = matchesLOT.Count > 0 ? matchesLOT[0].Value : "";
                request.Input = request.SKU;

            }
            //扫描的是HTTP 二维码，那么从中解析SKU
            if (request.Input.Contains("http"))
            {

                Uri uri = new Uri(request.Input);
                var collection = HttpUtility.ParseQueryString(uri.Query);
                var p = collection["p"];
                if (p.Count() > 0)
                {
                    request.SKU = collection["p"].Split(':')[1];
                    request.SN = collection["p"].Split(':')[0];
                    request.Input = request.SKU;
                }
            }
        }
        if (!string.IsNullOrEmpty(request.PickTaskNumber))
        {
            request.SKU = request.Input;
            //response.Data.SKU = request.Input;
        }
        //if (!string.IsNullOrEmpty(request.Input) && string.IsNullOrEmpty(request.PickTaskNumber))
        //{

        //}
        //if (!string.IsNullOrEmpty(request.Input) && string.IsNullOrEmpty(request.PickTaskNumber))
        //{

        //}
        response.Data.PickTaskNumber = request.PickTaskNumber;
        response.Data.Weight = request.Weight;
        response.Data.SKU = request.SKU;
        response.Data.Input = request.Input;
        response.Data.SN = request.SN;
        response.Data.Lot = request.Lot;
        response.Data.AcquisitionData = request.AcquisitionData;

        List<PackageData> pickData = new List<PackageData>();
        if (!string.IsNullOrEmpty(request.PickTaskNumber))
        {
            pickData = _sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);
            //判断是不是包装完成，补充了重量  缓存有数据， 取缓存返回
            if (pickData != null && pickData.Count > 0 && pickData.Where(a => a.PackageQty + a.ScanQty != a.PickQty).Count() == 0)
            {
                //判断有没有扫描数据
                if (pickData.Count > 0 && pickData.Where(a => a.ScanQty > 0).Count() > 0)
                {
                    var result = await PackingComplete(pickData, request, PackageBoxTypeEnum.正常);
                    response.Data.PackageDatas = pickData;
                    response.Code = result.Code;
                    response.Msg = result.Msg;
                    response.Data.BoxType = request.BoxType;

                    return response;
                }
                else
                {
                    //说明已经包装完成，删除缓存；
                    _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, null);
                    response.Code = StatusCode.Error;
                    response.Msg = "该拣货任务已经包装完成";
                    response.Data.BoxType = request.BoxType;

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
                response.Data.BoxType = request.BoxType;

                return response;
            }



            var CheckPickData = _repPickTask.AsQueryable().Where(a => a.PickTaskNumber == response.Data.PickTaskNumber)
                .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
            .ToList();
            if (CheckPickData.Where(a => a.PickStatus == (int)PickTaskStatusEnum.包装完成).Count() > 0)
            {
                response.Data.BoxType = request.BoxType;

                response.Code = StatusCode.Error;
                response.Msg = "拣货单已经完成包装";
                return response;
            }
            else if (CheckPickData.Where(a => a.PickStatus == (int)PickTaskStatusEnum.拣货完成).Count() == 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "拣货单还未完成拣货";
                response.Data.BoxType = request.BoxType;

                return response;

            }
            else if (CheckPickData.Count == 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "拣货单号不存在";
                response.Data.BoxType = request.BoxType;

                return response;
            }


            pickData = _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == response.Data.PickTaskNumber && a.PickStatus == (int)PickTaskStatusEnum.拣货完成)
                  .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                  .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
              .GroupBy(a => new { a.SKU, a.PickTaskNumber, a.CustomerId })
              .Select(a => new PackageData { SKU = a.SKU, CustomerId = a.CustomerId, PickQty = SqlFunc.AggregateSum(a.PickQty), RemainingQty = SqlFunc.AggregateSum(a.PickQty), PickTaskNumber = a.PickTaskNumber, GoodsName = SqlFunc.AggregateMax(a.GoodsName), GoodsType = SqlFunc.AggregateMax(a.GoodsType) })
              .ToList();

            if (pickData.Count > 0)
            {

                _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, pickData, timeSpan);
                response.Data.PackageDatas = pickData;
                response.Code = StatusCode.Success;
                response.Data.BoxType = request.BoxType;

                return response;
            }
        }

        //获取备注信息。一个拣货任务一个出库单就直接获取备注。一个拣货任务多个订单就提示自己去看备注
        //1，先获取拣货任务号，判断是一个还是多个
        var preOrderNumbers = _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber).Select(a => a.PreOrderNumber).Distinct();
        if (preOrderNumbers.Count() > 1)
        {
            response.Data.Remark = "该拣货任务为合并订单，请前往查看";
        }
        else
        {
            var preOrderNumber = preOrderNumbers.First();
            //2,根据获取，获取订单号，获取订单备注
            response.Data.Remark = await _repPreOrder.AsQueryable().Where(a => a.PreOrderNumber == preOrderNumber).Select(a => a.Remark).FirstAsync();

        }



        //2，判断扫描数据是不是SKU
        if (pickData != null && pickData.Count > 0)
        {
            response.Data.SKU = request.Input;
            if (request.ScanQty <= 1)
            {
                if (!string.IsNullOrEmpty(request.SN))
                {
                    //判断唯一码是不是重复扫描
                    foreach (var item in pickData.Where(a => a.SKU == request.SKU))
                    {
                        if (item.ScanPackageInput != null)
                        {
                            if (!string.IsNullOrEmpty(request.SN))
                            {
                                var count = item.ScanPackageInput.Where(a => a.SN == request.SN).FirstOrDefault();
                                if (count != null && !string.IsNullOrEmpty(count.SN))
                                {
                                    response.Data.PackageDatas = pickData.OrderBy(a => a.Order).ToList();
                                    response.Code = StatusCode.Error;
                                    response.Msg = "不能重复扫描同一个条码";
                                    response.Data.BoxType = request.BoxType;

                                    return response;
                                }
                            }
                        }

                        //判断JNE 是不是可用
                        if (!string.IsNullOrEmpty(request.SN))
                        {
                            var checkJNE = await _repRFPackageAcquisition.AsQueryable().Where(a => a.SN == request.SN && pickData.First().CustomerId == a.CustomerId).FirstAsync();
                            if (checkJNE != null && !string.IsNullOrEmpty(checkJNE.PreOrderNumber))
                            {
                                response.Data.PackageDatas = pickData.OrderBy(a => a.Order).ToList();
                                response.Data.BoxType = request.BoxType;


                                response.Code = StatusCode.Error;
                                response.Msg = "不能重复扫描同一个条码";
                                return response;
                            }
                        }
                    }
                }

              

                var PickSKUData = pickData.Where(a => a.SKU == request.SKU);
                if (PickSKUData.Count() > 0)
                {

                    foreach (var item in pickData)
                    {
                        item.Order = 99;
                        if (item.SKU == response.Data.SKU)
                        {
                            item.Order = 1;
                        }
                    }
                    if (PickSKUData.Sum(a => a.ScanQty + a.PackageQty) + 1 <= PickSKUData.First().PickQty)
                    {
                        var pick = pickData.Where(a => a.SKU == request.Input && a.PickQty > a.ScanQty + a.PackageQty).First();
                        pick.ScanQty += 1;
                        pick.RemainingQty -= 1;
                        if (pick.ScanPackageInput == null)
                        {
                            pick.ScanPackageInput = new List<ScanPackageInput>();
                        }
                        //pick.ScanPackageInput = new List<ScanPackageInput>();
                        pick.ScanPackageInput.Add(request);
                        _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, pickData, timeSpan);

                        //判断是不是包装完成
                        if (pickData.Where(a => a.ScanQty + a.PackageQty != a.PickQty).Count() == 0)
                        {
                            var result = await PackingComplete(pickData, request, PackageBoxTypeEnum.正常);
                            response.Data.PackageDatas = pickData.OrderBy(a => a.Order).ToList();
                            response.Data.BoxType = request.BoxType;
                            response.Code = result.Code;
                            response.Msg = result.Msg;
                            return response;
                        }
                        response.Data.BoxType = request.BoxType;
                        response.Data.PackageDatas = pickData.OrderBy(a => a.Order).ToList();
                        response.Code = StatusCode.Success;
                        return response;
                    }
                    else
                    {
                        response.Data.PackageDatas = pickData.OrderBy(a => a.Order).ToList();
                        response.Data.BoxType = request.BoxType;
                        response.Code = StatusCode.Error;
                        response.Msg = "该SKU数量已满足";
                        return response;
                    }

                }
                else
                {
                    response.Data.PackageDatas = pickData;
                    response.Data.PickTaskNumber = request.PickTaskNumber;
                    response.Data.BoxType = request.BoxType;
                    response.Code = StatusCode.Error;
                    response.Msg = "SKU 不存在";
                    return response;
                }
            }
            else
            {
                double Qty = request.ScanQty;

                //判断有没有SN,有SN 就记录出库SN
                //WMSRFPackageAcquisition wMSRF=new WMSRFPackageAcquisition();
                //wMSRF.
                //_repRFPackageAcquisition
                var PickSKUData = pickData.Where(a => a.SKU == request.Input);
                if (PickSKUData.Count() > 0)
                {

                    foreach (var item in pickData)
                    {
                        item.Order = 99;
                        if (item.SKU == response.Data.SKU)
                        {
                            item.Order = 1;
                        }
                    }
                    if (PickSKUData.Sum(a => a.ScanQty + a.PackageQty) + Qty <= PickSKUData.First().PickQty)
                    {
                        var pick = pickData.Where(a => a.SKU == request.Input && a.PickQty > a.ScanQty + a.PackageQty).First();
                        pick.ScanQty += Qty;
                        pick.RemainingQty -= Qty;
                        if (pick.ScanPackageInput == null)
                        {
                            pick.ScanPackageInput = new List<ScanPackageInput>();
                        }
                        //pick.ScanPackageInput = new List<ScanPackageInput>();
                        pick.ScanPackageInput.Add(request);
                        _sysCacheService.Set(_userManager.Account + "_Package_" + response.Data.PickTaskNumber, pickData, timeSpan);

                        //判断是不是包装完成
                        if (pickData.Where(a => a.ScanQty + a.PackageQty != a.PickQty).Count() == 0)
                        {
                            var result = await PackingComplete(pickData, request, PackageBoxTypeEnum.正常);
                            response.Data.PackageDatas = pickData.OrderBy(a => a.Order).ToList();
                            response.Code = result.Code;
                            response.Msg = result.Msg;
                            return response;
                        }
                        response.Data.PackageDatas = pickData.OrderBy(a => a.Order).ToList();
                        response.Code = StatusCode.Success;
                        return response;
                    }
                    else
                    {
                        response.Data.PackageDatas = pickData.OrderBy(a => a.Order).ToList();
                        response.Code = StatusCode.Error;
                        response.Msg = "该SKU数量已满足";
                        return response;
                    }

                }
                else
                {
                    response.Data.PackageDatas = pickData;
                    response.Data.PickTaskNumber = request.PickTaskNumber;
                    response.Code = StatusCode.Error;
                    response.Msg = "SKU 不存在";
                    return response;
                }
            }

        }

        response.Code = StatusCode.Error;
        response.Msg = "系统错误";
        return response;
    }



    public async Task<Response<ScanPackageOutput>> AddPackage(ScanPackageInput request)
    {


        Response<ScanPackageOutput> response = new Response<ScanPackageOutput>() { Data = new ScanPackageOutput() };
        response.Data.PickTaskNumber = request.PickTaskNumber;
        response.Data.Weight = request.Weight;
        response.Data.SKU = request.SKU;
        response.Data.Input = request.Input;
        response.Data.BoxType = request.BoxType;
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
    private async Task<Response> PackingComplete(List<PackageData> pickData, ScanPackageInput request, PackageBoxTypeEnum packageBox)
    {
        Response response = new Response();

        //判断是不是输入了重量
        if (request.Weight > 0.2)
        {
            var pickDataTemp = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == request.PickTaskNumber && a.PickStatus == (int)PickTaskStatusEnum.拣货完成)
                 .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                 .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0).OrderBy(a => a.Id).FirstAsync();
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
                        p.PackageNumber = packageNumber;
                        p.PreOrderNumber = packageData.PreOrderNumber;
                        p.OrderNumber = packageData.OrderNumber;
                        p.PickTaskId = packageData.PickTaskId;
                        //p.SN = item;
                        p.Creator = _userManager.Account;
                        p.CreationTime = DateTime.Now;
                        p.Type = "AFC";
                    }

                    PackageAcquisitions.AddRange(PackageAcquisition);

                    if (item.ScanPackageInputOld == null)
                    {
                        item.ScanPackageInputOld = new List<ScanPackageInput>();
                    }
                    item.ScanPackageInputOld.AddRange(item.ScanPackageInput);
                    item.ScanPackageInput = new List<ScanPackageInput>();
                }
            }
            packageData.ExpressCompany = request.ExpressCompany;
            packageData.GrossWeight = request.Weight;
            packageData.NetWeight = request.Weight;
            packageData.Id = 0;
            packageData.PackageType = request.BoxType;
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
            if (CheckPackageData > CheckPickData.Sum(a => a.PickQty))
            {
                throw Oops.Oh("并发请求回滚");
            }
            if (CheckPackageData >= CheckPickData.Sum(a => a.PickQty) || packageBox == PackageBoxTypeEnum.短包)
            {


                await _repPickTask.UpdateAsync(a => new WMSPickTask { PickStatus = (int)PickTaskStatusEnum.包装完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, a => a.PickTaskNumber == request.PickTaskNumber);
                await _repPickTaskDetail.UpdateAsync(a => new WMSPickTaskDetail { PickStatus = (int)PickTaskStatusEnum.包装完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, a => a.PickTaskNumber == request.PickTaskNumber);
                await _repOrder.UpdateAsync(a => new WMSOrder { OrderStatus = (int)OrderStatusEnum.已包装 }, a => CheckPickData.Select(b => b.OrderId).ToList().Contains(a.Id));
                //_repPickTask.UpdateAsync(a => a.PickStatus = (int)PickTaskStatusEnum.包装完成);
                //_repPickTaskDetail.UpdateAsync(a => a.PickStatus = (int)PickTaskStatusEnum.包装完成);
                _sysCacheService.Set(_userManager.Account + "_Package_" + request.PickTaskNumber, null);


                //包装完成，插入
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
                    wMSInstructions.Add(wMSInstruction);
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
                await _repInstruction.InsertRangeAsync(wMSInstructions);
                if (CheckPickData.First().CustomerId == 22)
                {
                    //获取
                    var getSN = await _repRFPackageAcquisition.AsQueryable().Where(a => a.PickTaskNumber == CheckPickData.First().PickTaskNumber).ToListAsync();
                    //将福光的包装数据处理一下
                    foreach (var item in CheckPickData)
                    {
                        //声明一个变量
                        double NumberRemaining = item.PickQty;
                        var itemdata = getSN.Where(a => a.CustomerId == item.CustomerId && a.Type == "AFC" && a.SKU == item.SKU && (a.ReceiptAcquisitionStatus ?? 0) != 1);
                        foreach (var itemSN in itemdata)
                        {
                            if (NumberRemaining <= 0)
                            {
                                break;
                            }
                            itemSN.ReceiptAcquisitionStatus = 1;
                            itemSN.OrderId = item.OrderId;
                            itemSN.PreOrderNumber = item.PreOrderNumber;
                            itemSN.OrderNumber = item.OrderNumber;
                            itemSN.ExternOrderNumber = item.ExternOrderNumber;
                            NumberRemaining--;
                        }
                    }
                    await _repRFPackageAcquisition.UpdateRangeAsync(getSN);
                }



                //wMSInstructions.Add(wMSInstruction);
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
    public Task<Response<ScanPackageOutput>> VerifyPackage(ScanPackageRFIDInput request)
    {
        throw new NotImplementedException();
    }
}
