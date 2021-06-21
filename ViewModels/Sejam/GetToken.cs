using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class GetToken
    {
        public GetToken(string jsonData)
        {
            jsonData = jsonData.Replace("\"", "'");
            JObject jObject = JObject.Parse(jsonData);
            JToken token = jObject["data"];
            accessToken = (string)token["accessToken"];
            ttl = (string)token["ttl"];
        }
        public string accessToken { get; set; }
        public string ttl { get; set; }

    }
}
