using System;

namespace HamAfarin.Classes.Dto
{
    public class BankRequestDto
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string RedirectUrl { get; set; }
        public string Result { get; set; }
    }
}