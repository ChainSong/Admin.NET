using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Interface
{
    public interface IReceiptExcelInterface
    {
        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        public UserManager _userManager { get; set; }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Response<DataTable> Export(List<long> request);
    }
}
