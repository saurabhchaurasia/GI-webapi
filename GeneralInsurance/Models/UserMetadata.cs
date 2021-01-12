using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralInsurance.Models
{
    public class UserMetadata
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Nullable<int> ContactNo { get; set; }

        [Required]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [MinLength(8)]
        public string DrivingLiscence { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }

    [MetadataType(typeof(UserMetadata))]
    public partial class USER { }
}