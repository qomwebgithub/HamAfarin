using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BankingAccountViewModel
    {
        public string sheba { get; set; }

        public static implicit operator BankingAccountViewModel(JToken jsonData)
        {
            return new BankingAccountViewModel()
            {
                sheba = (string)jsonData["sheba"]
            };
        }
    }
}
