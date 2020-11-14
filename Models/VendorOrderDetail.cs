using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class VendorOrderDetail
    {
        [Key]
        [Required(ErrorMessage = "Please enter vendor ID")]
        [MaxLength(12)]        
        public string Id { get; set; }

        [Required]
        [Display(Name = "Enter vendor ID")]
        public string VendorId { get; set; }

        [Required]
        [Display(Name = "Enter Product ID")]
        public string PId { get; set; }

        [Required]
        [Display(Name = "Enter Product Quantity")]
        public int PQuantity { get; set; }

        [Required]
        [Display(Name = "Enter Unit Price")]
        public float Price { get; set; }

        [Required]
        public int Notification { get; set; }

        [ForeignKey("VendorId")]
        public VendorDetail VendorDetails { get; set; }

        [ForeignKey("PId")]
        public WarehouseProduct WarehouseProduct { get; set; }
    }
}