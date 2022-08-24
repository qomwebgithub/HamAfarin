using HamAfarin.Classes.Dto;

namespace HamAfarin.Classes.Interface
{
    public interface IBankService
    {
        string GetRedirectUrl(string token);
        string GetToken(string requestToken);
        (bool IsSuccess, string Result) RequestToken(BankDto dto);
    }
}
