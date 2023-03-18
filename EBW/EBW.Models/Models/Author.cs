using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models
{
    public class Author : IndetifiedModel
    {
        [Required, DisplayName("Author name"), MaxLength(30), MinLength(2)]
        public string FullName { get; set; }
    }
}
