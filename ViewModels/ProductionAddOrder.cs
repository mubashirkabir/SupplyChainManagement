using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SupplyChainManagement.Models;

namespace SupplyChainManagement.ViewModels
{
    public class ProductionAddOrder
    {
        [Required]
        public ProductionOrder productionOrder { get; set; }
        
        [Required]
        public ProductionRawMaterial productionRawMaterial { get; set; }
    }
}