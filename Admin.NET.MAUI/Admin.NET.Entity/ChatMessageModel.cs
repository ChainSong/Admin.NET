using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Entity
{
    public class ChatMessageModel
    {
        public string Mag { get; set; }
        public string MagType { get; set; }
        public string MagDate { get; set; }
        public string Sender { get; set; }
        public Color BackgroundColor { get; set; }
    }
}
