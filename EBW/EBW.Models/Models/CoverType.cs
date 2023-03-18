﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EBW.Models
{
    public class CoverType : IndetifiedModel
    {
        [Required, DisplayName("Cover Type name"), MaxLength(20)]
        public string Name { get; set; }
    }
}
