using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EBW.Models
{
    public class Product : IndetifiedModel
    {
        [Required(ErrorMessage = "The field can't be empty"), MaxLength(25), MinLength(3)]
        public string Title { get; set; }

        [MaxLength(4000)]
        public string? Description { get; set; }
        [Required(ErrorMessage = "The field can't be empty"), MaxLength(17), RegularExpression(@"^\d{17}$", ErrorMessage = "ISBN must be 17 characters")]
        public string ISBN { get; set; }
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "The field can't be empty")]
        public decimal ListPrice { get; set; }
        

        [ValidateNever]
        public string ImageUrl { get; set; }

        //-- FK for linked tables in the database --//

        //1
        [ValidateNever]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        //2
        [ValidateNever]
        public int CoverTypeId { get; set; }

        [ForeignKey("CoverTypeId")]
        [ValidateNever]
        public CoverType CoverType { get; set; }
        //3
        [ValidateNever]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        [ValidateNever]
        public Author Author { get; set; }
    }
}
