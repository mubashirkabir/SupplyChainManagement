using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class ProductionOrder
    {
        [Key]
        [Required(ErrorMessage = "Please enter Production order ID")]
        [MaxLength(12)]
        [Display(Name = "Enter Production order id")]
        public string Id { get; set; }

        public string Title { get; set; }

        [Required]
        [Display(Name = "Enter Start Date, Format: DD.MM.YYYY")]
        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}")]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "Enter Expected End Date")]
        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}")]
        public string ExpectedDate { get; set; }

        [Required]
        public string PId { get; set; }
        
        [Required]
        public int PQuantity { get; set; }

        [Required]
        public string Status { get; set; }

        [Required] 
        public int Notification { get; set; }

        [ForeignKey("PId")]
        public WarehouseProduct WarehouseProduct { get; set; }

    }
}