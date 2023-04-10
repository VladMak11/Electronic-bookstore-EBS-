﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EBW.Models
{
    public class InputModelforRegistration
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The last name field is required.")]
        [Display(Name = "Last Name"), MinLength(2),MaxLength(25)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The first name field is required.")]
        [Display(Name = "First Name"), MinLength(2), MaxLength(25)]
        public string FirstName { get; set; }

        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Invalid phone number format. The phone number must be in the format +380XXXXXXXXX.")]
        [Required(ErrorMessage = "The phone number field is required.")]
        [Display(Name = "Phone number"), MaxLength(13)]
        public string PhoneNumber { get; set; }
    }
}
