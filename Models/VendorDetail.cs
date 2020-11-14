using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class VendorDetail
    {
        [Required(ErrorMessage = "Please enter Vendor ID")]
        [Key]
        [MaxLength(12)]
        [Display(Name = "Enter vendor ID")]
        public string VId { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Enter vendor name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Enter Vendor's Email ID")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Enter Vendor's Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Enter UserName")]
        public string UserName { get; set; }

        public string Address { get; set; }
    }
}