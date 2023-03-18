using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EBW.Models
{
    public class Category : IndetifiedModel
    {
        [Required, DisplayName("Category name"), MaxLength(20), MinLength(3)]
        public string Name { get; set; }
    }
}
