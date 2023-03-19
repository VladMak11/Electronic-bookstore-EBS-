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
        [Required, MaxLength(25), MinLength(3)]
        public string Title { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }
        [Required, MaxLength(17)]
        public string ISBN { get; set; }
        public decimal ListPrice { get; set; }
        [Required]
        public decimal Price { get; set; }

        [Required, MaxLength(256)]
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
