using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EBW.Models
{
    public class InputProfile
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The last name field is required.")]
        [Display(Name = "Last Name"), MinLength(2), MaxLength(25)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The first name field is required.")]
        [Display(Name = "First Name"), MinLength(2), MaxLength(25)]
        public string FirstName { get; set; }

        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Invalid phone number format. The phone number must be in the format +380XXXXXXXXX.")]
        [Required(ErrorMessage = "The phone number field is required.")]
        [Display(Name = "Phone number"), MaxLength(13)]
        public string PhoneNumber { get; set; }
        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name = "Branch Office")]
        public string? BranchOffice { get; set; }
        public InputProfile()
        {

        }
        public InputProfile(string email, string lastName, string firstName, string phoneNumber, string? city = null, string? branchOffice = null)
        {
            Email = email;
            LastName = lastName;
            FirstName = firstName;
            PhoneNumber = phoneNumber;
            City = city;
            BranchOffice = branchOffice;
        }
    }
}
