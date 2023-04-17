using Electronic_Bookstore_Web.PayPalService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;

namespace Electronic_Bookstore_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PayPalController : Controller
    {
        private readonly IConfiguration _config;
        private Payment payment;

        public PayPalController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Payment()
        {
            // Get the API context and create a payment
            var apiContext = PayPalConfiguration.GetAPIContext(_config.GetValue<string>("PayPal:ClientId"), _config.GetValue<string>("PayPal:ClientSecret"), _config.GetValue<string>("PayPal:Mode"));

            var guid = Convert.ToString((new Random()).Next(100000));
            var createdPayment = CreatePayment(apiContext, guid);

            // Save the payment ID to the user's session so we can execute the payment later
            HttpContext.Session.SetString("paymentId", createdPayment.id);

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

            // Redirect the user to PayPal
            return Redirect(paypalRedirectUrl);
        }

        public IActionResult Cancel()
        {
            return View();
        }

        public IActionResult Success()
        {
            var apiContext = PayPalConfiguration.GetAPIContext(_config.GetValue<string>("PayPal:ClientId"), _config.GetValue<string>("PayPal:ClientSecret"), _config.GetValue<string>("PayPal:Mode"));

            // Get the payment ID from the user's session
            var paymentId = HttpContext.Session.GetString("paymentId");

            // Execute the payment
            var executedPayment = ExecutePayment(apiContext, paymentId, Request.Query["PayerID"]);

            if (executedPayment.state.ToLower() != "approved")
            {
                return RedirectToAction("Cancel");
            }

            // Payment was successful, so do something here, like update your database or show a success page
            return RedirectToAction("Index");
        }

        private Payment CreatePayment(APIContext apiContext, string guid)
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
                var returnUrl = Url.Action("Success", "PayPal", null, Request.Scheme);
                var cancelUrl = Url.Action("Cancel", "PayPal", null, Request.Scheme);

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
        private Payment ExecutePayment(APIContext apiContext, string paymentId, string payerId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }
    }
}
