using Newtonsoft.Json;

namespace HamAfarin.Controllers.Api
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }

    public class ApiResult<TData> : ApiResult
        where TData : class
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TData Data { get; set; }
    }
}