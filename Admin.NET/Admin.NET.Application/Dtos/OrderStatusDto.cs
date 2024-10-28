

using Admin.NET.Application.Dtos.Enum;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Dtos
{
    public class OrderStatusDto
    {

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 系统单号
        /// </summary>
        public string SystemOrder { get; set; } = "";
        /// <summary>
        /// 客户单号
        /// </summary>
        public string ExternOrder { get; set; } = "";
        /// <summary>
        /// 状态代码
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态描述（用于前端显示，如有需要 按需填写）
        /// </summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 用于反应反馈数据的错误代码
        /// </summary>
        public StatusCode StatusCode { get; set; }
        /// <summary>
        /// 用于反应反馈类型的错误描述（系统统一填写）
        /// </summary>
        public string StatusMsg
        {
            get
            {

                return StatusCode.ToString().ToLower();
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Msg { get; set; }


    }
}

