using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EBW.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }
        [Required, DisplayName("Cover Type name"), MaxLength(20)]
        public string Name { get; set; }
    }
}
