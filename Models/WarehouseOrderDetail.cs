using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class WarehouseOrderDetail
    {
        [Key]
        public int SNo { get; set; }

        [Required]
        [Display(Name = "Enter Order ID")]
        public string OrderID { get; set; }

        [Required]
        [Display(Name = "Enter Sender Name")]
        public string Sender { get; set; }

        [Required]
        [Display(Name = "Enter Receiver Name")]
        public string Receiver { get; set; }


    }
}