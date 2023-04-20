using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models.ViewModels
{
    public class OrderVM
    {
        public OrderUserInfo OrderUserInfo { get; set; }
        public IEnumerable<OrderDetailsProduct> OrderDetail { get; set; }
    }
}
