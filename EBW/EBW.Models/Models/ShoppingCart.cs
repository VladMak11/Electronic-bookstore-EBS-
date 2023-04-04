using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models
{
	public class ShoppingCart
	{
		public Product Product { get; set; }
		[Range(1, 10000, ErrorMessage = "Avaible value between 1 and 10000")]
		public int Count { get; set; } = 1;
	}
}
