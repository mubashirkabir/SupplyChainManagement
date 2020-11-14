using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SupplyChainManagement.Models;

namespace SupplyChainManagement.ViewModels
{
    public class VendorOrderViewModel
    {
        public VendorDetail VendorDetail { get; set; }
        public VendorOrder VendorOrder { get; set; }
        public VendorOrderDetail VendorOrderDetail { get; set; }

    }
}