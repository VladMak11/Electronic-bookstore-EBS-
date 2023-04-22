using PayPal.Api;

namespace Electronic_Bookstore_Web.PayPalApi
{
    public interface IPayPalService
    {
        Task<string> PaymentAsync(HttpContext http, MapOrderData mapOrderData);
        Task<Payment> ExecutePaymentAsync(HttpContext http);
       
    }
}
