using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Enumerate
{
    public enum OrderStatusEnum
    {

        取消 = -1,
        新增 = 1,
        分配中 = 5,
        库存不足 = 10,
        部分分配 = 15,
        已分配 = 20,
        拣货中 = 25,
        已拣货 = 30,
        复检 = 40,
        包装中 = 50,
        已包装 = 60,
        交接 = 80,
        完成 = 99,
    }
}





