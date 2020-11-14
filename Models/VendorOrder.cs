using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class VendorOrder
    {
        [Key]
        [Display(Name = "Enter vendor ID")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Enter vendor-order ID")]
        public string OId { get; set; }

        [ForeignKey("Id")]
        public VendorDetail VendorDetails { get; set; }

        [ForeignKey("OId")]
        public VendorOrder VendorOrders { get; set; }
    }
}