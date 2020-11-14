using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class ProductionRawMaterial
    {
        [Key] 
        public int SNO { get; set; }

        [Required]
        [Display(Name = "Enter Order ID")]
        public string OId { get; set; }
        [Required]
        [Display(Name = "Enter Product id")]
        public string PId { get; set; }
        [Required]
        [Display(Name = "Enter Quantity id")]
        public string Quantity { get; set; }

        [ForeignKey("PId")]
        public WarehouseProduct WarehouseProduct { get; set; }
    }
}