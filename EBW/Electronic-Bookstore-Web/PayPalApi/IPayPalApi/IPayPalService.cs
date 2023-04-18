using PayPal.Api;

namespace Electronic_Bookstore_Web.PayPalApi
{
    public interface IPayPalService
    {
        Task<string> PaymentAsync(HttpContext http);
        Task<Payment> ExecutePaymentAsync(HttpContext http);
       
    }
}
