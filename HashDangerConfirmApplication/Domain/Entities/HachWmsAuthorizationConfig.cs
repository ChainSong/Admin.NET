using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlaApplication.Models.Entities
{
    [SugarTable("hach_confirm_wms_authorization_config", "哈希回传接口身份认证配置表")]
    public class HachConfirmWmsAuthorizationConfig:EntityBase
    {
        /// <summary>
        /// 雪花id
        /// </summary>
        [SugarColumn(ColumnDescription = "雪花id", IsIdentity = true, IsPrimaryKey = true)]
        public override long Id { get; set; }
        [SugarColumn(ColumnDescription = "用户id")]
        public string? AppId { get; set; }
        [SugarColumn(ColumnDescription = "秘钥")]
        public string AppSecret { get; set; }
        [SugarColumn(ColumnDescription = "类型")]
        public string? Type { get; set; }
        [SugarColumn(ColumnDescription = "接口名称")]
        public string InterfaceName { get; set; }
        [SugarColumn(ColumnDescription = "接口地址")]
        public string Url { get; set; }
        [SugarColumn(ColumnDescription = "客户名称")]
        public string CustomerName { get; set; }
        [SugarColumn(ColumnDescription = "客户ID")]
        public long? CustomerId { get; set; }
        [SugarColumn(ColumnDescription = "仓库Id")]
        public long? WarehouseId { get; set; }
        [SugarColumn(ColumnDescription = "仓库名称")]
        public string WarehouseName { get; set; }
        [SugarColumn(ColumnDescription = "状态")]
        public bool Status { get; set; } = true;
        [SugarColumn(ColumnDescription = "")]
        public string? Token { get; set; }
        [SugarColumn(ColumnDescription = "")]
        public string? AuthorizationScheme { get; set; }
    }
}
