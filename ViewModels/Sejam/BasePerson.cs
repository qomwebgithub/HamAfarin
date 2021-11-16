using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BasePerson
    {
        public BasePerson(string jsonData)
        {
            JObject jObject = JObject.Parse(jsonData);
            JToken jUser = jObject["data"];
            mobile = (string)jUser["mobile"];
            email = (string)jUser["email"];
            uniqueIdentifier = (string)jUser["uniqueIdentifier"];
            type = (string)jUser["type"];
            privatePerson = jUser["privatePerson"];
            accounts = jUser["accounts"][0];
            tradingCodes = jUser["tradingCodes"][0];
            province = (string)jUser["addresses"][0]["province"]["name"];
            city = (string)jUser["addresses"][0]["city"]["name"];
        }
        public string mobile { get; set; }
        public string email { get; set; }
        public string uniqueIdentifier { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public PrivatePersonViewModel privatePerson { get; set; }
        public BankingAccountViewModel accounts { get; set; }
        public TradingCodeViewModel tradingCodes { get; set; }
        public LegalPersonViewModel legalPerson { get; set; }
        public List<AddressViewModel> addresses { get; set; }
        //public List<TradingCodeViewModel> tradingCodes { get; set; }
        public AgentViewModel agent { get; set; }
        public JobInfoViewModel jobInfo { get; set; }
        public FinancialInfoViewModel financialInfo { get; set; }
        public List<LegalPersonShareholderViewModel> legalPersonShareholders { get; set; }
        public List<LegalPersonStakeholderViewModel> legalPersonStakeholders { get; set; }

    }
}
