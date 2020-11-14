using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class UsersDetail
    {
        [Required]
        [Key]
        public int Id { get; set; }

        
        [Required]
        [StringLength(20)]
        [Display(Name = "Enter UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Enter Password")]
        public string UserPassword { get; set; }

        [Required]
        [Display(Name = "Enter Full Name")]
        public string FullName { get; set; }

        [Phone]
        [Display(Name = "Enter Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Enter Roll Id")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public UserRoles UserRoles { get; set; }
    }
}