using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class WarehouseOrder
    {
        [Key]
        public int SNo { get; set; }

        [Required(ErrorMessage = "Please enter order ID")]
        [MaxLength(12)]
        [Display(Name = "Enter order id")]
        public string OId { get; set; }

        [Required(ErrorMessage = "Please enter product ID")]
        [MaxLength(5)]
        [Display(Name = "Enter product id")]
        public string PId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        [Display(Name = "Enter Product Quantity")]
        public int PQuantity { get; set; }

        [Required]
        public float TotalPrice { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Enter Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }

        public int Notification { get; set; }

        [ForeignKey("PId")]
        public WarehouseProduct WarehouseProduct { get; set; }

      
    }
}