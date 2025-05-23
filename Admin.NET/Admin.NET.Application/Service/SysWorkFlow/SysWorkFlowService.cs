﻿using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using System.Collections.Generic;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBCreateCloudBaseRunServerVersionRequest.Types.Code.Types;
using System.Linq;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WeDataQueryBindListResponse.Types;
using System.Reflection;
using Yitter.IdGenerator;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Service.Dto;
using XAct;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Common;
using RulesEngine.Models;
using Newtonsoft.Json;
using NewLife.Reflection;
using System.Linq.Expressions;
using AngleSharp.Io;

namespace Admin.NET.Application;
/// <summary>
/// SysWorkFlow服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class SysWorkFlowService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysWorkFlow> _rep;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysPos> _repPos;
    private readonly SqlSugarRepository<SysRole> _repRole;
    private readonly SqlSugarRepository<SysUser> _repUser;
    private readonly SqlSugarRepository<SysWorkFlowTable> _repWorkFlowTable;

    public SysWorkFlowService(SqlSugarRepository<SysWorkFlow> rep, UserManager userManager, SqlSugarRepository<SysPos> repPos, SqlSugarRepository<SysRole> repRole, SqlSugarRepository<SysUser> repUser, SqlSugarRepository<SysWorkFlowTable> repWorkFlowTable)
    {
        _rep = rep;
        _userManager = userManager;
        _repPos = repPos;
        _repRole = repRole;
        _repUser = repUser;
        _repWorkFlowTable = repWorkFlowTable;
    }

    /// <summary>
    /// 分页查询SysWorkFlow
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>Abstract
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<SysWorkFlowOutput>> Page(SysWorkFlowInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WorkName), u => u.WorkName.Contains(input.WorkName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WorkTable), u => u.WorkTable.Contains(input.WorkTable.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WorkTableName), u => u.WorkTableName.Contains(input.WorkTableName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.NodeConfig), u => u.NodeConfig.Contains(input.NodeConfig.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.LineConfig), u => u.LineConfig.Contains(input.LineConfig.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(input.Weight > 0, u => u.Weight == input.Weight)
                    .WhereIF(input.CreateID > 0, u => u.CreateID == input.CreateID)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Modifier), u => u.Modifier.Contains(input.Modifier.Trim()))
                    .WhereIF(input.ModifyID > 0, u => u.ModifyID == input.ModifyID)
                    .WhereIF(input.AuditingEdit > 0, u => u.AuditingEdit == input.AuditingEdit)

                    .Select<SysWorkFlowOutput>()
;
        if (input.CreateDateRange != null && input.CreateDateRange.Count > 0)
        {
            DateTime? start = input.CreateDateRange[0];
            query = query.WhereIF(start.HasValue, u => u.CreateDate > start);
            if (input.CreateDateRange.Count > 1 && input.CreateDateRange[1].HasValue)
            {
                var end = input.CreateDateRange[1].Value.AddDays(1);
                query = query.Where(u => u.CreateDate < end);
            }
        }
        if (input.ModifyDateRange != null && input.ModifyDateRange.Count > 0)
        {
            DateTime? start = input.ModifyDateRange[0];
            query = query.WhereIF(start.HasValue, u => u.ModifyDate > start);
            if (input.ModifyDateRange.Count > 1 && input.ModifyDateRange[1].HasValue)
            {
                var end = input.ModifyDateRange[1].Value.AddDays(1);
                query = query.Where(u => u.ModifyDate < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }



    /// <summary>
    /// 增加SysWorkFlow
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response> Add(AddSysWorkFlowInput saveDataModel)
    {
        Response response = new Response();
        //List < SysWorkFlow >
        var MainData = saveDataModel.MainData.Adapt<SysWorkFlow>();
        MainData.SysWorkFlowSteps = new List<SysWorkFlowStep>();
        saveDataModel.DetailData.ForEach(a =>
        {
            MainData.SysWorkFlowSteps.Add(a.Adapt<SysWorkFlowStep>());
        });
        //var DetailData = 

        //MainData.SysWorkFlowSteps = DetailData;
        //   if (saveDataModel == null
        //|| saveDataModel.MainData == null
        //|| saveDataModel.MainData.Count == 0)
        //return;
        //如果存在，就先删除在插入
        if (MainData.Id != 0)
        {
            var oldData = await _rep.Context.UpdateNav(MainData).Include(a => a.SysWorkFlowSteps).ExecuteCommandAsync();
        }
        else
        {
            var oldData = await _rep.Context.InsertNav(MainData).Include(a => a.SysWorkFlowSteps).ExecuteCommandAsync();

        }
        response.Code = StatusCode.Success;
        response.Msg = "成功";
        return response;
        //saveDataModel.DetailData = saveDataModel.DetailData?.Where(x => x.Count > 0).ToList();
        ////Type type = typeof(T);
        //// 修改为与Update一致，先设置默认值再进行实体的校验
        ////UserInfo userInfo = UserContext.Current.UserInfo;
        //saveDataModel.SetDefaultVal(AppSetting.CreateMember, userInfo);

        //string validReslut = type.ValidateDicInEntity(saveDataModel.MainData, true, UserIgnoreFields);

        //if (!string.IsNullOrEmpty(validReslut)) return Response.Error(validReslut);

        //if (saveDataModel.MainData.Count == 0)
        //    return Response.Error("保存的数据为空，请检查model是否配置正确!");

        //PropertyInfo keyPro = type.GetKeyProperty();
        //if (keyPro.PropertyType == typeof(Guid) || keyPro.PropertyType == typeof(string))
        //{
        //    saveDataModel.MainData.Add(keyPro.Name, Guid.NewGuid());
        //}
        //else
        //{
        //    if (AppSetting.EnableSnowFlakeID && keyPro.PropertyType == typeof(long))
        //    {
        //        saveDataModel.MainData.Add(keyPro.Name, YitIdHelper.NextId());
        //    }
        //    else
        //    {
        //        saveDataModel.MainData.Remove(keyPro.Name);
        //    }
        //}
        ////没有明细直接保存返回
        //if (saveDataModel.DetailData == null || saveDataModel.DetailData.Count == 0)
        //{
        //    T mainEntity = saveDataModel.MainData.DicToEntity<T>();
        //    SetAuditDefaultValue(mainEntity);
        //    if (base.AddOnExecuting != null)
        //    {
        //        Response = base.AddOnExecuting(mainEntity, null);
        //        if (CheckResponseResult()) return Response;
        //    }
        //    Response = repository.DbContextBeginTransaction(() =>
        //    {
        //        repository.Add(mainEntity, true);
        //        saveDataModel.MainData[keyPro.Name] = keyPro.GetValue(mainEntity);
        //        Response.OK(ResponseType.SaveSuccess);
        //        if (base.AddOnExecuted != null)
        //        {
        //            Response = base.AddOnExecuted(mainEntity, null);
        //        }
        //        return Response;
        //    });
        //    if (Response.Status) Response.Data = new { data = saveDataModel.MainData };
        //    AddProcese(mainEntity);
        //    return Response;
        //}

        //Type detailType = GetRealDetailType();
        //var entity = saveDataModel.Adapt<SysWorkFlow>();
        //await _rep.InsertAsync(entity);





        //aveDataModel.MainData["Enable"] = 1;


        //AddOnExecuting = (SysWorkFlow workFlow, object list) =>
        //{
        //    workFlow.WorkFlow_Id = Guid.NewGuid();

        //    webResponse = WorkFlowContainer.Instance.AddTable(workFlow, list as List<Sys_WorkFlowStep>);
        //    if (!webResponse.Status)
        //    {
        //        return webResponse;
        //    }
        //    return webResponse.OK();
        //};

        //AddOnExecuted = (SysWorkFlow workFlow, object list) =>
        //{
        //    return webResponse.OK();
        //};
        //return base.Add(saveDataModel);
    }


    //[HttpPost]
    //[ApiDescriptionSettings(Name = "GetVueDictionary")]
    //public object GetVueDictionary(string[] dicNos)
    //{
    //    if (dicNos == null || dicNos.Count() == 0) return new string[] { };
    //    var dicConfig = DictionaryManager.GetDictionaries(dicNos, false).Select(s => new
    //    {
    //        dicNo = s.DicNo,
    //        config = s.Config,
    //        dbSql = s.DbSql,
    //        list = s.Sys_DictionaryList.OrderByDescending(o => o.OrderNo)
    //                  .Select(list => new { key = list.DicValue, value = list.DicName })
    //    }).ToList();

    //    object GetSourceData(string dicNo, string dbSql, object data)
    //    {
    //        //  2020.05.01增加根据用户信息加载字典数据源sql
    //        dbSql = DictionaryHandler.GetCustomDBSql(dicNo, dbSql);
    //        if (string.IsNullOrEmpty(dbSql))
    //        {
    //            return data as object;
    //        }
    //        return repository.DapperContext.QueryList<object>(dbSql, null);
    //    }
    //    return dicConfig.Select(item => new
    //    {
    //        item.dicNo,
    //        item.config,
    //        data = GetSourceData(item.dicNo, item.dbSql, item.list)
    //    }).ToList();
    //}

    /// <summary>
    /// 删除SysWorkFlow
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteSysWorkFlowInput input)
    {
        //var entity = await _rep.GetFirstAsync(u => u.WorkFlowId == input.WorkFlowId) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        var entity = input.Adapt<SysWorkFlow>();
        await _rep.Context.DeleteNav(entity).Include(a => a.SysWorkFlowSteps).ExecuteCommandAsync();   //删除
    }

    /// <summary>
    /// 更新SysWorkFlow
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateSysWorkFlowInput input)
    {
        var entity = input.Adapt<SysWorkFlow>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }


    /// <summary>
    /// 获取SysWorkFlow 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<SysWorkFlow> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.SysWorkFlowSteps).Where(u => u.Id == id).FirstAsync();
        return entity;
    }


    /// <summary>
    /// 获取SysWorkFlow 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "GetUser")]
    public async Task<Response<List<SysWorkFlowForUserDto>>> GetUser()
    {
        Response<List<SysWorkFlowForUserDto>> response = new Response<List<SysWorkFlowForUserDto>>() { Data = new List<SysWorkFlowForUserDto>() };

        var entity = await _repUser.AsQueryable().Select(a => new SelectListItem { Text = a.Account, Value = a.Account }).ToListAsync();
        SysWorkFlowForUserDto flowForUserDto = new SysWorkFlowForUserDto();
        flowForUserDto.data = entity;
        flowForUserDto.dicNo = "roles";
        response.Data.Add(flowForUserDto);
        return response;
    }

    /// <summary>
    /// 获取SysWorkFlow列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<SysWorkFlowOutput>> List([FromQuery] SysWorkFlowInput input)
    {
        return await _rep.AsQueryable().Select<SysWorkFlowOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取SysWorkFlow列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "copyWorkFlow")]
    public async Task<Response> CopyWorkFlow(SysWorkFlow input)
    {
        //先判断有没有这个流程
        var entityCheck = await _rep.AsQueryable().Includes(a => a.SysWorkFlowSteps).Where(u => u.WorkName == input.WorkName).FirstAsync();
        if (entityCheck != null && entityCheck.Id != 0)
        {
            return new Response() { Code = StatusCode.Error, Msg = "流程名称已存在" };
        }
        var entity = await _rep.AsQueryable().Includes(a => a.SysWorkFlowSteps).Where(u => u.Id == input.Id).FirstAsync();
        entity.Id = 0;
        entity.WorkName = input.WorkName;
        entity.WorkTable = input.WorkName;
        foreach (var item in entity.SysWorkFlowSteps)
        {
            item.Id = 0;
            item.WorkFlowId = 0;
        }
        await _rep.Context.InsertNav(entity).Include(a => a.SysWorkFlowSteps).ExecuteCommandAsync();
        return new Response() { Code = StatusCode.Success, Msg = "复制成功" };
        //return await _rep.AsQueryable().Select<SysWorkFlowOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取SysWorkFlow列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<string> GetSystemWorkFlow(string CustomerName, string WorkFlow, string WorkFlowStepName, string OrderType)
    {

        //使用简单工厂定制化修改和新增的方法
        //根据订单类型判断是否存在该流程
        var workflow = await _rep.AsQueryable()
           .Includes(a => a.SysWorkFlowSteps)
           .Where(a => a.WorkName == CustomerName + WorkFlow).FirstAsync();

        string workFlowStrategy = ""
;            //判断是不是有定制化的流程
        if (workflow != null)
        {
            var customWorkFlow = workflow.SysWorkFlowSteps.Where(p => p.StepName == WorkFlowStepName).ToList();
            if (customWorkFlow.Count > 0)
            {
                //判断有没有子流程
                if (!string.IsNullOrEmpty(customWorkFlow[0].Filters))
                {
                    //将customWorkFlow[0].Filters 反序列化成List<SysWorkFlowFieldDto>
                    List<SysWorkFlowFieldDto> sysWorkFlowFieldDtos = JsonConvert.DeserializeObject<List<SysWorkFlowFieldDto>>(customWorkFlow[0].Filters);
                    workFlowStrategy = sysWorkFlowFieldDtos.Where(p => p.Field == OrderType).Select(p => p.Value).FirstOrDefault("");
                }
                else
                {
                    workFlowStrategy = customWorkFlow[0].Remark;
                }
            }
        }
        return workFlowStrategy;
        //return await _rep.AsQueryable().Select<SysWorkFlowOutput>().ToListAsync();
    }


    /// <summary>
    /// 审核默认对应数据库字段为AuditId审核人ID ,AuditStatus审核状态,Auditor审核人,Auditdate审核时间,Auditreason审核原因
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="auditStatus"></param>
    /// <param name="auditReason"></param>
    /// <returns></returns>
    public async Task<Response> Audit(object[] keys, int? auditStatus, string auditReason)
    {
        Response response = new Response();
        if (keys == null || keys.Length == 0)
        {
            response.Msg = "请选择要审核的数据";
            response.Code = StatusCode.Error;
            return response;
        }
        return response;
        //Expression<Func<T, bool>> whereExpression = typeof(T).GetKeyName().CreateExpression<T>(keys[0], LinqExpressionType.Equal);
        //T entity = repository.FindAsIQueryable(whereExpression).FirstOrDefault();

        ////进入流程审批
        //if (WorkFlowManager.Exists<T>(entity))
        //{
        //    var auditProperty = TProperties.Where(x => x.Name.ToLower() == "auditstatus").FirstOrDefault();
        //    if (auditProperty == null)
        //    {
        //        return Response.Error("表缺少审核状态字段：AuditStatus");
        //    }

        //    AuditStatus status = (AuditStatus)Enum.Parse(typeof(AuditStatus), auditStatus.ToString());
        //    if (auditProperty.GetValue(entity).GetInt() != (int)AuditStatus.审核中)
        //    {
        //        return Response.Error("只能审批审核中的数据");
        //    }
        //    Response = repository.DbContextBeginTransaction(() =>
        //    {
        //        return WorkFlowManager.Audit<T>(entity, status, auditReason, auditProperty, AuditWorkFlowExecuting, AuditWorkFlowExecuted);
        //    });
        //    if (Response.Status)
        //    {
        //        return Response.OK(ResponseType.AuditSuccess);
        //    }
        //    return Response.Error(Response.Message ?? "审批失败");
        //}


        ////获取主键
        //PropertyInfo property = TProperties.GetKeyProperty();
        //if (property == null)
        //    return Response.Error("没有配置好主键!");

        //UserInfo userInfo = UserContext.Current.UserInfo;

        ////表如果有审核相关字段，设置默认审核

        //PropertyInfo[] updateFileds = TProperties.Where(x => auditFields.Contains(x.Name.ToLower())).ToArray();
        //List<T> auditList = new List<T>();
        //foreach (var value in keys)
        //{
        //    object convertVal = value.ToString().ChangeType(property.PropertyType);
        //    if (convertVal == null) continue;

        //    entity = Activator.CreateInstance<T>();
        //    property.SetValue(entity, convertVal);
        //    foreach (var item in updateFileds)
        //    {
        //        switch (item.Name.ToLower())
        //        {
        //            case "auditid":
        //                item.SetValue(entity, userInfo.User_Id);
        //                break;
        //            case "auditstatus":
        //                item.SetValue(entity, auditStatus);
        //                break;
        //            case "auditor":
        //                item.SetValue(entity, userInfo.UserTrueName);
        //                break;
        //            case "auditdate":
        //                item.SetValue(entity, DateTime.Now);
        //                break;
        //            case "auditreason":
        //                item.SetValue(entity, auditReason);
        //                break;
        //        }
        //    }
        //    auditList.Add(entity);
        //}
        //if (base.AuditOnExecuting != null)
        //{
        //    Response = AuditOnExecuting(auditList);
        //    if (CheckResponseResult()) return Response;
        //}
        //Response = repository.DbContextBeginTransaction(() =>
        //{
        //    repository.UpdateRange(auditList, updateFileds.Select(x => x.Name).ToArray(), true);
        //    if (base.AuditOnExecuted != null)
        //    {
        //        Response = AuditOnExecuted(auditList);
        //        if (CheckResponseResult()) return Response;
        //    }
        //    return Response.OK();
        //});
        //if (Response.Status)
        //{
        //    return Response.OK(ResponseType.AuditSuccess);
        //}
        //return Response.Error(Response.Message);
    }


    /// <summary>
    /// 写入流程
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <param name="rewrite">是否重新生成流程</param>
    /// <param name="changeTableStatus">是否修改原表的审批状态</param>
    public static void AddProcese(SysWorkFlowTable workFlowTable, bool rewrite = false, bool changeTableStatus = true)
    {
        //var workFlowTable_Id = Guid.NewGuid();
        //SysWorkFlowTable workFlowTable = new SysWorkFlowTable()
        //{
        //    WorkFlowTable_Id = workFlowTable_Id,
        //    AuditStatus = (int)AuditStatus.审核中,
        //    CurrentOrderId = 1,
        //    Enable = 1,
        //    WorkFlow_Id = workFlow.WorkFlow_Id,
        //    WorkName = workFlow.WorkName,
        //    WorkTable = workTable,
        //    WorkTableKey = typeof(T).GetKeyProperty().GetValue(entity).ToString(),
        //    WorkTableName = workFlow.WorkTableName,
        //    CreateID = userInfo.User_Id,
        //    CreateDate = DateTime.Now,
        //    Creator = userInfo.UserTrueName,
        //    Sys_WorkFlowTableStep = workFlow.Sys_WorkFlowStep.OrderBy(x => x.OrderId).Select(s => new Sys_WorkFlowTableStep()
        //    {
        //        Sys_WorkFlowTableStep_Id = Guid.NewGuid(),
        //        WorkFlowTable_Id = workFlowTable_Id,
        //        WorkFlow_Id = s.WorkFlow_Id,
        //        StepId = s.StepId,
        //        StepName = s.StepName,
        //        AuditId = s.StepType == (int)AuditType.用户审批 ? s.StepValue : null,
        //        StepType = s.StepType,
        //        StepValue = s.StepValue,
        //        OrderId = s.OrderId,
        //        Enable = 1,
        //        CreateDate = DateTime.Now,
        //    }).ToList()
        //};

    }

}

