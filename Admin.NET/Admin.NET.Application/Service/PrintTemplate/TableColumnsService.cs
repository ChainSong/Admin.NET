using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using COSXML.Network;
using FluentEmail.Core;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Extensions.Caching.Distributed;
using NewLife.Net;
using NewLife.Reflection;
using NPOI.HPSF;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBCreateCloudBaseRunServerVersionRequest.Types.Code.Types;

namespace Admin.NET.Application;
/// <summary>
/// 表管理服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class PrintTemplateService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysPrint> _rep;

    private readonly UserManager _userManager;


    private readonly SysCacheService _sysCacheService;
    public PrintTemplateService(SqlSugarRepository<SysPrint> rep, SysCacheService sysCacheService, UserManager userManager)
    {
        _rep = rep;

        _sysCacheService = sysCacheService;
        _userManager = userManager;
        //_db = db;
    }

    /// <summary>
    /// 增加表管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<SysPrint> Query(string input)
    {
        var entity =await _rep.AsQueryable().Where(x => x.Name == input && x.Status == StatusEnum.Enable).FirstAsync();
        return entity;
        //await _rep.InsertAsync(entity);
    }



}

