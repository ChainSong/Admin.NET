using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlaApplication.Domain
{
    public enum InstructionStatus { Pending = 1, Succeeded = 99, Failed = 3,OBPending=63 }
    public enum InstructionType
    {
        [Description("未知")]
        未知 = 0,
        [Description("入库单序列号回传HachDG")]
        入库单序列号回传HachDG = 1013,
        [Description("入库单回传HachDG")]
        入库单回传HachDG = 1011,
        [Description("出库单防伪码回传HachDG")]
        出库单防伪码回传HachDG = 1002,
        [Description("出库单序列号回传HachDG")]
        出库单序列号回传HachDG = 1003,
        [Description("出库装箱回传HachDG")]
        出库装箱回传HachDG = 1004,
        [Description("出库单回传HachDG")]
        出库单回传HachDG = 1001,
    }
    public static class InstructionTypeHelper
    {
        public static bool TryParse(string? raw, out InstructionType type)
        {
            type = InstructionType.未知;
            if (string.IsNullOrWhiteSpace(raw)) return false;
            var s = raw.Trim();

            if (Enum.TryParse<InstructionType>(s, true, out var parsed)) { type = parsed; return true; }
            if (int.TryParse(s, out var n) && Enum.IsDefined(typeof(InstructionType), n)) { type = (InstructionType)n; return true; }

            switch (s.ToUpperInvariant())
            {
                case "INBOUND":
                case "PO":
                case "PO_DELIVERY":
                    type = InstructionType.入库单回传HachDG; return true;
                case "OUTBOUND":
                case "SO":
                case "SHIP_CONFIRM":
                    type = InstructionType.出库单回传HachDG; return true;
            }
            return false;
        }
    }

}
