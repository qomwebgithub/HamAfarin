using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class AdminSendSmsviewModel
    {
        public string MobileNumber { get; set; }
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}
