using HamAfarin.Classes.Dto;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;

namespace HamAfarin.Classes.Interface
{
    public interface IBankService
    {
        Task<BankRequestDto> RequestAsync(BankInvoice bankInvoice);
        BankCallbackResult Fetch(HttpRequestBase httpRequest);
        Task<PaymentVerifyResult> VerifyAsync(BankCallbackResult callbackResult);
    }
}
