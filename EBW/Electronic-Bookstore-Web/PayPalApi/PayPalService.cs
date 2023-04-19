using Electronic_Bookstore_Web.PayPalService;
using Microsoft.AspNetCore.Http;
using PayPal.Api;
using System.Security.Policy;

namespace Electronic_Bookstore_Web.PayPalApi
{
    public class PayPalService : IPayPalService
    {
        private readonly IConfiguration _config;
        private Payment payment;

        public PayPalService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> PaymentAsync(HttpContext http)
        {
            // Get the API context and create a payment
            var apiContext = PayPalConfiguration.GetAPIContext(_config.GetValue<string>("PayPal:ClientId"), _config.GetValue<string>("PayPal:ClientSecret"), _config.GetValue<string>("PayPal:Mode"));

            var guid = Convert.ToString((new Random()).Next(100000));
            var createdPayment =  await CreatePaymentAsync(apiContext, guid, http);

            // Save the payment ID to the user's session so we can execute the payment later
            http.Session.SetString("paymentId", createdPayment.id);

            // Get the approval URL from the payment links
            var links = createdPayment.links.GetEnumerator();
            string paypalRedirectUrl = null;
            while (links.MoveNext())
            {
                var link = links.Current;
                if (link.rel.ToLower().Trim().Equals("approval_url"))
                {
                    paypalRedirectUrl = link.href;
                }
            }
            return paypalRedirectUrl;
        }

        public async Task<Payment> ExecutePaymentAsync(HttpContext http)
        {
            var apiContext = PayPalConfiguration.GetAPIContext(_config.GetValue<string>("PayPal:ClientId"), _config.GetValue<string>("PayPal:ClientSecret"), _config.GetValue<string>("PayPal:Mode"));

            // Get the payment ID from the user's session
            var paymentId = http.Session.GetString("paymentId");

            // Execute the payment
            var paymentExecution = new PaymentExecution() { payer_id = http.Request.Query["PayerID"] };
            payment = new Payment() { id = paymentId };

            var executedPayment =  payment.Execute(apiContext, paymentExecution);
             return executedPayment;

            //if (executedPayment.state.ToLower() != "approved")
            //{
            //    return RedirectToAction("Cancel");
            //}

            //// Payment was successful, so do something here, like update your database or show a success page
            //return RedirectToAction("Index");
        }
        private async Task<Payment> CreatePaymentAsync(APIContext apiContext, string guid, HttpContext http)
        {
            // Set up the payment details, including the total amount, currency, and item details
            var itemList = new ItemList()
            {
                items = new List<Item>()
                    {
                        new Item()
                        {
                            name = "Example Item",
                            currency = "USD",
                            price = "10",
                            quantity = "1",
                            sku = "sku"
                        }
                    }
            };

            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = "10"
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = "10",
                details = details
            };

            var transaction = new Transaction()
            {
                description = "Example Transaction",
                invoice_number = guid,
                amount = amount,
                item_list = itemList
            };

            var transactions = new List<Transaction>()
                {
                    transaction
                };

            // Set up the redirect URLs for the user, including where to go on success or cancel
            var baseUrl = $"{http.Request.Scheme}://{http.Request.Host}";
            var returnUrl = $"{baseUrl}/Customer/Cart/Success";
            var cancelUrl = $"{baseUrl}/Customer/Cart/Cancel";

            var payment = new Payment()
            {
                intent = "sale",
                payer = new Payer() { payment_method = "paypal" },
                transactions = transactions,
                redirect_urls = new RedirectUrls()
                {
                    cancel_url = cancelUrl,
                    return_url = returnUrl
                }
            };

            // Create the payment using the API context
            return payment.Create(apiContext);
        }
    }
}
