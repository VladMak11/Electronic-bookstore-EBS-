using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models
{
    public class OrderUserInfo : IndetifiedModel
    {
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? Carrier { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string BranchOffice { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public DateTime ShippingDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }


    }
}
