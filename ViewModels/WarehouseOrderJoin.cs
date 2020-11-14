using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SupplyChainManagement.Models;

namespace SupplyChainManagement.ViewModels
{
    public class WarehouseOrderJoin
    {
        public WarehouseOrder WarehouseOrderj { get; set; }
        public WarehouseOrderType WarehouseOrderTypej { get; set; }
        public WarehouseOrderDetail WarehouseOrderDetailj { get; set; }
    }
}