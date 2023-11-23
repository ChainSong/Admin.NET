using Admin.NET.Common.EnumCommon;
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application;

namespace MyProAdmin.NET.Applicationject.ReceiptReceivingCore.Strategy
{
    public class ReceiptReceivingReturnDefaultStrategy : IReceiptReceivingReturnInterface
    {

        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving { get; set; }

        public SqlSugarRepository<WMSLocation> _repLocation { get; set; }

        public ISqlSugarClient _db { get; set; }



        public ReceiptReceivingReturnDefaultStrategy()
        {
        }

        //默认方法不做任何处理
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<long> request)
        {
            //return Task.Run(() =>
            //{
            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>();
            response.Data = new List<OrderStatusDto>();
            //先判断状态是否正常 是否允许回退
            var receipt = _repReceipt.AsQueryable().Where(a => request.Contains(a.Id));
            receipt.ToList().ForEach(a =>
            {
                if (a.ReceiptStatus == (int)ReceiptStatusEnum.完成)
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = a.ExternReceiptNumber,
                        SystemOrder = a.ReceiptNumber,
                        Type = a.ReceiptType,
                        StatusCode = StatusCode.Warning,
                        Msg = "状态异常"
                    });
                }
            });
            if (response.Data.Count > 0)
            {
                return response;
            }
            //先处理上架=>入库单
            //先处理上架表的数据，然后修改入库单中的数据 
            await _repReceiptReceiving.DeleteAsync(a => request.Contains(a.ReceiptId));
            await _repReceipt.UpdateAsync(a => new WMSReceipt { ReceiptStatus = (int)ReceiptStatusEnum.新增 }, (a => request.Contains(a.Id)));
            //_wms_receiptRepository.GetAll().Where(a => request.Contains(a.Id)).ToList().ForEach(c =>
            //{
            //    c.ReceiptStatus = (int)ReceiptStatusEnum.新增;
            //    _wms_receiptRepository.Update(c);
            //});

            //.BatchUpdate(new WMS_Receipt { ReceiptStatus = (int)ReceiptStatusEnum.新增 });

            receipt.ToList().ForEach(a =>
            {

                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = a.ExternReceiptNumber,
                    SystemOrder = a.ReceiptNumber,
                    Type = a.ReceiptType,
                    StatusCode = StatusCode.Success,
                    Msg = "操作成功"
                });

            });
            //_wms_receiptRepository.Update(a => new WMS_Receipt {  ReceiptStatus== ReceiptStatusEnum.新增 } );
            response.Code = StatusCode.Success;
            //throw new NotImplementedException();
            return response;
            //});
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
        //          //由于框架约定大于配置， 数据库的字段首字母小写
        //          //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
        //          DbColumnName = a.DbColumnName,
        //          IsImportColumn = a.IsImportColumn
        //      }).ToList();
        //}
    }
}