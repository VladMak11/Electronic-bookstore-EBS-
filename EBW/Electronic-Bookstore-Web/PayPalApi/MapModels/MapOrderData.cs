using PayPal.Api;

namespace Electronic_Bookstore_Web
{ 
    public class MapOrderData
    {
        public ItemList ItemList { get; set; }
        public Details Details { get; set; }
        public Amount Amount { get; set; }
        public List<Transaction> TransactionList { get; set; }
      
        
        
    }
}
