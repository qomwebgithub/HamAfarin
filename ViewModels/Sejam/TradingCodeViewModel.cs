using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class TradingCodeViewModel
    {
        public string code { get; set; }

        public static implicit operator TradingCodeViewModel(JToken jsonData)
        {
            return new TradingCodeViewModel()
            {
                code = (string)jsonData["code"]
            };
        }
    }
}
