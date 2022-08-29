using HamAfarin.Classes.Dto;
using System.Collections.Specialized;

namespace HamAfarin.Classes.Interface
{
    public interface IBankService
    {
        BankRequestDto Request(BankInvoice bankInvoice);
        //BankPaymentDto DeserializeBankRequest(NameValueCollection requestQueryString);
        //(bool IsSuccess, string Result) CheckTransaction(string transactionReferenceID);
        //long GetTransactionAmount(string transactionResult);
        //(bool IsSuccess, string Result) RefundPayment(BankPaymentDto dto);
        //(bool IsSuccess, string Result) ConfirmPayment(BankPaymentDto dto);
    }
}
