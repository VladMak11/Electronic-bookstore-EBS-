using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Electronic_Bookstore_Web.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
