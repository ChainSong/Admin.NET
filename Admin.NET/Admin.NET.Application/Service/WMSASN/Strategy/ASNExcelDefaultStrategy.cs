
using Admin.NET.Application.CommonCore.EnumCommon;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Interface;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Strategy
{
    public class ASNExcelDefaultStrategy : IASNExcelInterface
    {
        //注入数据库实例
        //public ISqlSugarClient _db { get; set; }

        //注入权限仓储
        public UserManager _userManager { get; set; }
        ////注入ASN仓储

        //public SqlSugarRepository<WMSASN> _repASN { get; set; }
        ////注入仓库关系仓储

        //public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        //注入客户关系仓储
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public ASNExcelDefaultStrategy(
        )
        {

        }

        //默认方法不做任何处理
        public Response<DataTable> Strategy(dynamic request)
        {
            Response<DataTable> response = new Response<DataTable>();

            var headerTableColumn = GetColumns("WMS_ASN");
            var detailTableColumn = GetColumns("WMS_ASNDetail");


            //循环datatable
            for (int i = 0; i < request.Columns.Count; i++)
            {
                //获取datatable的标头
                var s = request.Columns[i].ColumnName;
                var Column = headerTableColumn.Where(a => a.DisplayName == s).FirstOrDefault();
                if (Column == null)
                {
                    Column = detailTableColumn.Where(a => a.DisplayName == s).FirstOrDefault();
                }

                if (Column == null)
                {
                    continue;
                }
                //判断标头与key是否相等
                if (s.Equals(Column.DisplayName))
                {
                    //相等替换掉原来的表头
                    request.Columns[i].ColumnName = Column.DbColumnName;
                }


                //var config = new MapperConfiguration(cfg => cfg.CreateMap<DataTable, WMS_ASN>() );
                //var mapper = new Mapper(config);
                //var asnData = mapper.Map<List<WMS_ASN>>(request);


            }
            response.Data = request;
            response.Code = StatusCode.Success;
            //throw new NotImplementedException();
            return response;

        }
        private List<TableColumns> GetColumns(string TableName)
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
                  IsImportColumn = a.IsImportColumn
              }).ToList();
        }
    }
}
