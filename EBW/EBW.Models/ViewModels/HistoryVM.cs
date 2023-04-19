using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models
{
    public class HistoryVM
    {
        public IEnumerable<OrderUserInfo> OrderUserInfoList { get; set; }
        public IEnumerable<OrderDetailsProduct> OrderDetailsProductList { get; set; }
    }
}
