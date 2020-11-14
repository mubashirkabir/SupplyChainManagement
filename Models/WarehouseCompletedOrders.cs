using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SupplyChainManagement.Models
{
    public class WarehouseCompletedOrders
    {
        [Key]
        public int SNo { get; set; }

        [Required]
       public string OId { get; set; }

        [Required]
        public string PId { get; set; }

        [Required]
        public int PQuantity { get; set; }

        [Required]
        public float TotalPrice { get; set; }

        [Required] 
        public int Notification { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }

    }
}