using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class WarehouseVendorRequest
    {
        [Required] 
        [Key]
        [Display(Name = "Please Enter Product ID")]
        public string OID { get; set; }

        [Required]
        [Display(Name = "Enter product ID")]
        public string Pid { get; set; }

        [Required]
        [Display(Name = "Enter product Quantity")]
        public int Quantity { get; set; }
    }
}