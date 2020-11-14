using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SupplyChainManagement.Models
{
    public class WarehouseOrderType
    {
        [Required]
        [Key]
        [Display(Name = "Enter Order ID")]
        public string OId { get; set; }

        [Required]
        [Display(Name = "Enter order type")]
        public int type { get; set; }

        [Required]
        public int Status { get; set; }
    }

    public enum type
    {
        Receiving,
        Sending
    }

}