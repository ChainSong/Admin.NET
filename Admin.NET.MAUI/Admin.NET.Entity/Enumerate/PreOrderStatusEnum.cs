using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Entity
{
    public enum PreOrderStatusEnum
    {
        取消 = -1,

        新增 = 1,

        部分转出库 = 50,

        转出库 = 60,

        完成 = 99
    }
}
