
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.ApplicationCore.Strategy;
using Admin.NET.Common;
using Admin.NET.Core.Entity;
using Newtonsoft.Json;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class PreOrderForOrderFactory
    {
        public static IPreOrderForOrderInterface PreOrderForOrder(string workFlow)
        {

            switch (workFlow)
            {
                case "Hach":
                    return new PreOrderForOrderHachStrategy();
                default:
                    return new PreOrderForOrderDefaultStrategy();
            }
        }

    }
}
