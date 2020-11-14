using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.ViewModels
{
    public class UserLogin
    {
        [Required]
        [StringLength(20)]

        public string UserName { get; set; }

        [Required]
        
        public string UserPassword { get; set; }
    }
}