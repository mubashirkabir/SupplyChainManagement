using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SupplyChainManagement.Models;

namespace SupplyChainManagement.ViewModels
{
    public class WarehouseReports
    {
        public WarehouseOrder WarehouseOrder { get; set; }
        public WarehouseOrderDetail WarehouseOrderDetail { get; set; }
        public WarehouseOrderType WarehouseOrderType { get; set; }
        public WarehouseProduct WarehouseProduct { get; set; }
        public WarehouseStock WarehouseStock { get; set; }
    }
}