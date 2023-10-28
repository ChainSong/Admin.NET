using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Enumerate
{
    public enum ASNStatusEnum
    {
        /// <summary>
        /// ASN 取消 -1
        /// </summary>
        取消 = -1,

        /// <summary>
        /// ASN 新增 1
        /// </summary>
        新增 = 1,

        /// <summary>
        /// ASN 转入库单 10
        /// </summary>
        转入库单 = 10,

    

        /// <summary>
        /// ASN 完成 99
        /// </summary>
        完成 = 99,
    }
}
