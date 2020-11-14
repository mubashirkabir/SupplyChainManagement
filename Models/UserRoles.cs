using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class UserRoles
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}