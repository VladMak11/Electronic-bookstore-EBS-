

//using EBW.Models.APIModels;
//using Newtonsoft.Json;
//using System.Data;
//using System.Text;

//namespace Electronic_Bookstore_Web
//{
//    public class NovaPoshtaService : INovaPoshtaService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly string _apiKey;
//        private readonly IConfiguration _config;

//        public NovaPoshtaService(IConfiguration config)
//        {
//            _config = config;
//            _apiKey = _config.GetSection("NovaPoshtaAPIkey").Value;
//            _httpClient = new HttpClient();
//        }

//        public async Task<City> GetCitiesAsync(SettlementsRequest request1)
//        {

//            var client = new HttpClient();
//            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.novaposhta.ua/v2.0/json/getCities");
//            string data = "{\r\n   \"apiKey\": \"{0}\",\r\n   \"modelName\": \"Address\",\r\n   \"calledMethod\": \"searchSettlements\",\r\n   \"methodProperties\": {\r\n\"CityName\" : \"{1}\",\r\n\"Limit\" : \"{2}\",\r\n\"Page\" : \"{3}\"\r\n   }\r\n}";



//            var content = new StringContent(string.Format(data, _apiKey, request1.CityName, request1.Limit ,request1.Page),Encoding.UTF8, "application/json");
//            request.Content = content;
//            var response = await client.SendAsync(request);
//            response.EnsureSuccessStatusCode();
//            var responseContent = await response.Content.ReadAsStringAsync();
//            var decodedata = System.Text.RegularExpressions.Regex.Unescape(responseContent);
//            StreamWriter write = new StreamWriter("data.txt");
//            write.Write(decodedata);
//            write.Close();

//                //string url = $"https://api.novaposhta.ua/v2.0/json/searchSettlements";
//                //HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
//                //var data = new
//                //{
//                //    apiKey = _apiKey,
//                //    modelName = "Address",
//                //    calledMethod = "searchSettlements",
//                //    methodProperties = new
//                //    {
//                //        CityName = request.CityName,
//                //        Limit = request.Limit,
//                //        Page = request.Page
//                //    }
//                //};

//            //requestMessage.Content = JsonContent.Create(data);
//            //var response = await _httpClient.SendAsync(requestMessage);
//            //response.EnsureSuccessStatusCode();
//            //var responseContent = await response.Content.ReadAsStringAsync();


//            //string url = "https://api.novaposhta.ua/v2.0/json/searchSettlements";
//            //var payload = new
//            //{
//            //    apiKey = _apiKey,
//            //    modelName = "Address",
//            //    calledMethod = "searchSettlements",
//            //    methodProperties = new
//            //    {
//            //        CityName = request.CityName,
//            //        Limit = request.Limit,
//            //        Page = request.Page
//            //    }
//            //};
//            //var response = await _httpClient.PostAsJsonAsync(url, payload);
//            //response.EnsureSuccessStatusCode();
//            //var responseContent = await response.Content.ReadAsStringAsync();
//                return new City();
            
//        }

//        public Task<List<Warehouse>> GetWarehousesAsync(string cityRef)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
