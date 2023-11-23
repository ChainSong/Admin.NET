
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
using Admin.NET.Application.Dtos.Enum;

namespace Admin.NET.Application.Strategy
{
    public class PreOrderExcelDefaultStrategy : IPreOrderExcelInterface
    {
        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
        public ISqlSugarClient _db { get; set; }
        public UserManager _userManager { get; set; }


        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }




        public PreOrderExcelDefaultStrategy(

        )
        {
        }

        //默认方法不做任何处理
        public Response<DataTable> Strategy(dynamic request)
        {
            Response<DataTable> response = new Response<DataTable>();

            var headerTableColumn = GetColumns("WMS_PreOrder");
            var detailTableColumn = GetColumns("WMS_PreOrderDetail");


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

            }
            //var config = new MapperConfiguration(cfg => cfg.CreateMap<DataTable, WMS_ASN>() );
            //var mapper = new Mapper(config);
            //var asnData = mapper.Map<List<WMS_ASN>>(request);

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