
using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Interface
{
    public interface IASNExcelInterface
    {

        //数据库实例
        //ISqlSugarClient _db { get; set; }
        //用户仓储
        UserManager _userManager { get; set; }
        ////asn仓储
        SqlSugarRepository<WMSASN> _repASN { get; set; }
        ////客户用户关系仓储
        //SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        ////仓库用户关系仓储
        //SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }

        //SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        //表字段仓储
        SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Response<DataTable, List<OrderStatusDto>> Import(dynamic request);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Response<DataTable> Export(WMSASNExcelInput request);



        public List<TableColumns> GetColumns(string TableName)
        {
            return _repTableColumns.AsQueryable()
               .Where(a => a.TableName == TableName &&
                 a.TenantId == _userManager.TenantId &&
                 a.IsImportColumn == 1
               )
              .Select(a => new TableColumns
              {
                  DisplayName = a.DisplayName,
                  //由于框架约定大于配置， 数据库的字段首字母小写
                  //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
                  DbColumnName = a.DbColumnName,
                  IsImportColumn = a.IsImportColumn,
                  tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
                  //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
                  //.Select()
              }).ToList();
        }
    }
}
