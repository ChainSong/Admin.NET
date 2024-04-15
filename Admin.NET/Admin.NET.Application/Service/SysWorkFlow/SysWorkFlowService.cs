using Admin.NET.Application.Const;
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
    public SysWorkFlowService(SqlSugarRepository<SysWorkFlow> rep, UserManager userManager, SqlSugarRepository<SysPos> repPos, SqlSugarRepository<SysRole> repRole, SqlSugarRepository<SysUser> repUser)
    {
        _rep = rep;
        _userManager = userManager;
        _repPos = repPos;
        _repRole = repRole;
        _repUser = repUser;
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
            await _rep.Context.UpdateNav(MainData).Include(a => a.SysWorkFlowSteps).ExecuteCommandAsync();
        }
        else
        {
            await _rep.Context.InsertNav(MainData).Include(a => a.SysWorkFlowSteps).ExecuteCommandAsync();

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





}

