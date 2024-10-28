using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.MAUI2C.AppApis
{
    public static class ChartApi
    {
        /// <summary>
        /// ASN 状态图表接口
        /// </summary>
        public const string _ASNStatusChart = "/api/wMSChart/ASNStatusChart";
        /// <summary>
        /// 预出库状态图表接口
        /// </summary>
        public const string _PreOrderStatusChart = "/api/wMSChart/PreOrderStatusChart";
        /// <summary>
        /// ASN 数量图表接口
        /// </summary>
        public const string _ASNNumChart = "/api/wMSChart/ASNNumChart";
        /// <summary>
        /// 预出库数量图表接口
        /// </summary>
        public const string _PreOrderNumChart = "/api/wMSChart/PreOrderNumChart";
    }
}
