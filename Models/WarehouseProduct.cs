using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class WarehouseProduct
    {
        [Key]
        [Required(ErrorMessage = "Please enter product ID")]
        [MaxLength(5)]
        [Display(Name = "Enter product id")]
        public string PId { get; set; }

        [Required(ErrorMessage = "Please enter product name")]
        [Display(Name = "Enter product name")]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter product price")]
        [Display(Name = "Enter product price")]
        
        public float Price { get; set; }

        [Required(ErrorMessage = "Please select product type")]
        [Display(Name = "Enter product type")]
        public string Type { get; set; }

        [Display(Name = "Enter Category")] 
        public string Category { get; set; }
        
        [Required]
        public int Status { get; set; }

        public string Description { get; set; }

    }
}