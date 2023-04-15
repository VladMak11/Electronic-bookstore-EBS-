using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EBW.Models
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public OrderUserInfo OrderUserInfo { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? PoshtaList { get; set; } = new List<SelectListItem>
        {
             new SelectListItem { Text = "Nova Posta", Value = "Nova Poshta" },
             new SelectListItem { Text = "UKR Posta", Value = "UKR Poshta" }
        };
    }
}
