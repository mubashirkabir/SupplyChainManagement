using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class WarehouseStock 
    {
        [Key]
        [Display(Name = "Enter product id")]
        public string ProductID { get; set; }

        [Required]
        [Display(Name = "Enter product Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Enter Shelf Number")]
        public string ShelveNo { get; set; }

        [Required]
        public int Status { get; set; }

        [ForeignKey("ProductID")]
        public WarehouseProduct WarehouseProduct { get; set; }

        public IEnumerator<WarehouseStock> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

}