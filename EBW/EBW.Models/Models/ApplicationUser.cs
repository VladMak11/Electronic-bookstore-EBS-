using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? City { get; set; }
        public string? BranchOffice { get; set; }
    }
}
