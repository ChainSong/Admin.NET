using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.MAUI2C
{
    public class MessageModel
    {
        public string Msg { get; set; }
        public string MagType { get; set; }
        public string MagDate { get; set; }
        public string Sender { get; set; }
        public bool IsString { get; set; }=false;
        public bool IsDataTable { get; set; }=false;
        public Color BackgroundColor { get; set; }
    }
}
