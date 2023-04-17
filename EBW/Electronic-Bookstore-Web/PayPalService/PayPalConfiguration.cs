using PayPal.Api;

namespace Electronic_Bookstore_Web.PayPalService
{
    public class PayPalConfiguration
    {
        public PayPalConfiguration() { }
        public static Dictionary<string,string> GetConfig(string mode)
        {
            //getting prop from the ...json?
            return new Dictionary<string, string>() { { "mode", mode } };
        }

        private static string GetAccessToken(string ClienId, string ClientSecret, string mode)
        {
            //getting access token from PayPal
            string accessToken = new OAuthTokenCredential(ClienId, ClientSecret, new Dictionary<string, string>()
            {
               { "mode", mode }
            }).GetAccessToken();

            return accessToken;
        }
        public static APIContext GetAPIContext(string ClienId, string ClientSecret,string mode)
        {
            //return apicontext obj by invoking it with tha accesstoken
            APIContext aPIContext = new APIContext(GetAccessToken(ClienId, ClientSecret, mode));
            aPIContext.Config = GetConfig(mode);
            return aPIContext;
        }
    }
}
