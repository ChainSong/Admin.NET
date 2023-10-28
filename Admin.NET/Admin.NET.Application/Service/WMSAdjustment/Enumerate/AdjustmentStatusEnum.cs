using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Enumerate
{
    public enum AdjustmentStatusEnum
    {
        /// <summary>
        /// WMSAdjustment 取消 -1
        /// </summary>
        取消 = -1,

        /// <summary>
        /// WMSAdjustment 新增 1
        /// </summary>
        新增 = 1,

        /// <summary>
        /// WMSAdjustment 完成 99
        /// </summary>
        完成 = 99,
    }
}
