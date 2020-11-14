using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using SupplyChainManagement.Models;

namespace SupplyChainManagement.ViewModels
{
    public class WarehouseCompletedOrderJoin
    {
        public WarehouseCompletedOrders WarehouseCompletedOrdersj { get; set; }
        public WarehouseOrderType WarehouseOrderTypej { get; set; }
        public WarehouseOrderDetail WarehouseOrderDetailj { get; set; }
    }
}