using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using SupplyChainManagement.Models;
using SupplyChainManagement.ViewModels;

namespace SupplyChainManagement.Controllers.Api
{
    public class HomeController : ApiController
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();

        }

        [System.Web.Http.Route("Api/UserDelete/{id}")]
        [System.Web.Http.HttpDelete]
        //PUT /api/Home/1
        public void UserDelete(string id)
        {
            int x;
            if (Int32.TryParse(id, out x))
            {
                x = Int32.Parse(id);
                try
                {
                    var userInDb = _context.UsersDetails.SingleOrDefault(u => u.Id == x);
                    if (userInDb == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                        return;
                    }
                    _context.UsersDetails.Remove(userInDb);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }


        [System.Web.Http.Route("Api/Home/SearchUser/{name}")]
        [System.Web.Http.HttpGet]
        //Put /api/home/searchuser/1
        public IEnumerable<UsersDetail> searchUser(String name)
        {
            List<UsersDetail> usersDetails = _context.UsersDetails.ToList();

            var userList = (from u in usersDetails
                            where u.FullName.Contains(name)
                            select new UsersDetail()
                            {
                                Id = u.Id,
                                UserName = u.UserName,
                                FullName = u.FullName,


                            }).ToList();

            if (userList == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return userList;
        }

        //WAREHOUSE REPORTING

        [System.Web.Http.Route("Api/Home/WarehouseReports/AllProducts")]
        [System.Web.Http.HttpGet]
        public IEnumerable<WarehouseProduct> AllProducts()
        {
            List<WarehouseProduct> productInDb = _context.WarehouseProducts.ToList();
            return productInDb;
        }

        [System.Web.Http.Route("Api/Home/WarehouseReports/Inventory")]
        [System.Web.Http.HttpGet]
        public IEnumerable<WarehouseProductJoin> Inventory()
        {
            List<WarehouseProduct> products = _context.WarehouseProducts.ToList();
            List<WarehouseStock> stocks = _context.WarehouseStocks.ToList();
            var StockList = (from s in stocks
                             join p in products on s.ProductID equals p.PId
                             select new WarehouseProductJoin()
                             {
                                 Wproducts = p,
                                 WStock = s

                             }).ToList();

            return StockList;
        }

        [System.Web.Http.Route("Api/Home/WarehouseReports/InboundOrder")]
        [System.Web.Http.HttpGet]
        public IEnumerable<WarehouseOrderJoin> InboundOrder()
        {
            List<WarehouseOrder> orders = _context.WarehouseOrders.ToList();
            List<WarehouseOrderDetail> details = _context.WarehouseOrderDetails.ToList();
            List<WarehouseOrderType> type = _context.WarehouseOrderTypes.ToList();
            var OrderList = (from o in orders
                             join d in details on o.OId equals d.OrderID
                             join t in type on o.OId equals t.OId
                             where t.type == 1
                             orderby o.OId descending
                             select new WarehouseOrderJoin()
                             {
                                 WarehouseOrderj = o,
                                 WarehouseOrderDetailj = d,
                                 WarehouseOrderTypej = t
                             }).ToList();

            return OrderList;
        }

        [System.Web.Http.Route("Api/Home/WarehouseReports/OutboundOrder")]
        [System.Web.Http.HttpGet]
        public IEnumerable<WarehouseOrderJoin> OutboundOrder()
        {
            List<WarehouseOrder> orders = _context.WarehouseOrders.ToList();
            List<WarehouseOrderDetail> details = _context.WarehouseOrderDetails.ToList();
            List<WarehouseOrderType> type = _context.WarehouseOrderTypes.ToList();
            var OrderList = (from o in orders
                             join d in details on o.OId equals d.OrderID
                             join t in type on o.OId equals t.OId
                             where t.type == 2
                             orderby o.OId descending
                             select new WarehouseOrderJoin()
                             {
                                 WarehouseOrderj = o,
                                 WarehouseOrderDetailj = d,
                                 WarehouseOrderTypej = t
                             }).ToList();

            return OrderList;
        }

        [System.Web.Http.Route("Api/Home/WarehouseReports/AllOrders/{startDate}/{endDate}")]
        [System.Web.Http.HttpGet]
        public IEnumerable<WarehouseOrderJoin> AllOrders(string startDate, string endDate)
        {
            List<WarehouseOrder> orders = _context.WarehouseOrders.ToList();
            List<WarehouseOrderDetail> details = _context.WarehouseOrderDetails.ToList();
            List<WarehouseOrderType> type = _context.WarehouseOrderTypes.ToList();
            var OrderList = (from o in orders
                             join d in details on o.OId equals d.OrderID
                             join t in type on o.OId equals t.OId
                             where o.Date >= DateTime.Parse(startDate) && o.Date <= DateTime.Parse(endDate)
                             orderby o.Date descending
                             select new WarehouseOrderJoin()
                             {
                                 WarehouseOrderj = o,
                                 WarehouseOrderDetailj = d,
                                 WarehouseOrderTypej = t
                             }).ToList();

            return OrderList;
        }

        [System.Web.Http.Route("Api/Home/WarehouseReports/cAllOrders/{startDate}/{endDate}")]
        [System.Web.Http.HttpGet]
        public IEnumerable<WarehouseCompletedOrderJoin> cAllOrders(string startDate, string endDate)
        {
            List<WarehouseCompletedOrders> orders = _context.WarehouseCompletedOrderses.ToList();
            List<WarehouseOrderDetail> details = _context.WarehouseOrderDetails.ToList();
            List<WarehouseOrderType> type = _context.WarehouseOrderTypes.ToList();
            var OrderList = (from o in orders
                             join d in details on o.OId equals d.OrderID
                             join t in type on o.OId equals t.OId
                             where o.Date >= DateTime.Parse(startDate) && o.Date <= DateTime.Parse(endDate)
                             orderby o.Date descending
                             select new WarehouseCompletedOrderJoin()
                             {
                                 WarehouseCompletedOrdersj = o,
                                 WarehouseOrderDetailj = d,
                                 WarehouseOrderTypej = t
                             }).ToList();

            return OrderList;
        }



        // VENDOR REPORTING

        [System.Web.Http.Route("Api/Home/VendorReports/AllVendors")]
        [System.Web.Http.HttpGet]
        public IEnumerable<VendorDetail> AllVendors()
        {
            List<VendorDetail> vendorInDb = _context.VendorDetails.ToList();
            return vendorInDb;
        }


        [System.Web.Http.Route("Api/Home/VendorReports/AllVendorsOrders")]
        [System.Web.Http.HttpGet]
        public IEnumerable<VendorOrderViewModel> AllVendorsOrders()
        {
            List<VendorDetail> vendorInDb = _context.VendorDetails.ToList();
            List<VendorOrderDetail> vendorOrders = _context.VendorOrderDetails.ToList();
            var vendorOrderInDb = (from v in vendorInDb
                                   join o in vendorOrders on v.VId equals o.VendorId
                                   orderby o.Id ascending
                                   select new VendorOrderViewModel()
                                   {
                                       VendorDetail = v,
                                       VendorOrderDetail = o
                                   }).ToList();
            return vendorOrderInDb;
        }


        //PRODUCTION REPORTING
        [System.Web.Http.Route("Api/Home/ProductionReports/AllProductionOrder")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ProductionAddOrder> AllProductionOrder()
        {
            List<ProductionOrder> productionOrderInDb = _context.ProductionOrders.ToList();
            List<ProductionRawMaterial> ProductionRawInDb = _context.ProductionRawMaterial.ToList();
            var productionInDb = (from po in productionOrderInDb
                                  join pr in ProductionRawInDb on po.Id equals pr.OId
                                  orderby po.Id ascending
                                  select new ProductionAddOrder()
                                  {
                                      productionOrder = po,
                                      productionRawMaterial = pr
                                  }).ToList();
            return productionInDb;
        }

        //Notification
        //
        //
        //FOR Vendor
        [System.Web.Http.Route("Api/Home/VendorNotification/Orders")]
        [System.Web.Http.HttpGet]
        public IEnumerable<VendorOrderViewModel> Orders()
        {

            List<VendorDetail> vendorInDb = _context.VendorDetails.ToList();
            List<VendorOrderDetail> vendorOrders = _context.VendorOrderDetails.ToList();
            var vendorOrderInDb = (from v in vendorInDb
                join o in vendorOrders on v.VId equals o.VendorId
                where o.Notification.Equals(0) 
                orderby o.Id ascending
                select new VendorOrderViewModel()
                {
                    VendorDetail = v,
                    VendorOrderDetail = o
                }).ToList();
            return vendorOrderInDb;
        }

        [System.Web.Http.Route("Api/Home/VendorNotification/OrdersDelete/{id}")]
        [System.Web.Http.HttpGet]
        public void OrdersDelete(string id)
        {
            var orderDetail = _context.VendorOrderDetails.SingleOrDefault(o => o.Id == id);
            orderDetail.Notification = 1;
            _context.SaveChanges();

        }

        //FOR Production
        [System.Web.Http.Route("Api/Home/ProductionNotification/POrders")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ProductionOrder> POrders()
        {

            List<ProductionOrder> vendorInDb =
                _context.ProductionOrders.Where(po => po.Status.Equals("InProgress")).ToList();
            return vendorInDb;
        }

        [System.Web.Http.Route("Api/Home/ProductionNotification/PCOrders")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ProductionOrder> PCOrders()
        {

            List<ProductionOrder> vendorInDb =
                _context.ProductionOrders.Where(po => po.Status.Equals("Completed")).ToList();
            return vendorInDb;
        }


        //FOR WarehouseOrders
        [System.Web.Http.Route("Api/Home/WarehouseNotification/CompletedOrders")]
        [System.Web.Http.HttpGet]
        public IEnumerable<WarehouseCompletedOrders> CompletedOrders()
        {

            List<WarehouseCompletedOrders> warehouseCompletedOrderses =
                _context.WarehouseCompletedOrderses.Where(o => o.Notification.Equals(0)).OrderBy(oid => oid.OId).ToList();
            return warehouseCompletedOrderses;
        }

        [System.Web.Http.Route("Api/Home/WarehouseNotification/CompletedOrders/{id}")]
        [System.Web.Http.HttpDelete]
        public void CompletedOrders(string id)
        {
            List<WarehouseCompletedOrders> warehouseCompletedOrderses =
                _context.WarehouseCompletedOrderses.Where(o => o.OId.Equals(id)).ToList();
            for (int i = 0; i < warehouseCompletedOrderses.Count; i++)
            {
                warehouseCompletedOrderses[i].Notification = 1;
                _context.SaveChanges();
            }
        }

        [System.Web.Http.Route("Api/Home/WarehouseNotification/CompletedWOrders")]
        [System.Web.Http.HttpGet]
        public IEnumerable<WarehouseOrder> CompletedWOrders()
        {

            List<WarehouseOrder> warehouseOrder =
                _context.WarehouseOrders.Where(o => o.Notification.Equals(0) || o.Notification.Equals(2)).OrderBy(oid => oid.OId).ToList();
            return warehouseOrder;
        }

        [System.Web.Http.Route("Api/Home/WarehouseNotification/CompletedWOrders/{id}")]
        [System.Web.Http.HttpDelete]
        public void CompletedWOrders(string id)
        {
            List<WarehouseOrder> warehouseOrder =
                _context.WarehouseOrders.Where(o => o.OId.Equals(id)).ToList();
            for (int i = 0; i < warehouseOrder.Count; i++)
            {
                warehouseOrder[i].Notification = 1;
                _context.SaveChanges();
            }
        }

        [System.Web.Http.Route("Api/Home/WarehouseNotification/LowStockWarning")]
        [System.Web.Http.HttpGet]
        public IEnumerable<WarehouseProductJoin> LowStockWarning() {
            List<WarehouseProduct> products = _context.WarehouseProducts.ToList();
            List<WarehouseStock> stocks = _context.WarehouseStocks.ToList();
            var StockList = (from s in stocks
                             join p in products on s.ProductID equals p.PId
                             where s.Status == 0
                             select new WarehouseProductJoin()
                             {
                                 Wproducts = p,
                                 WStock = s

                             }).ToList();
            List <WarehouseProductJoin> warehouseProductJoins = new List<WarehouseProductJoin> { };
            for (int i = 0; i < StockList.Count; i++)
            {
                if (StockList[i].WStock.Quantity < 10) {
                    warehouseProductJoins.Add(StockList[i]);
                }
            }
            return warehouseProductJoins;
        }

        //Warehouse CRUD Operation

        //PDelete
        [System.Web.Http.Route("Api/Home/Warehouse/ProductDelete/{id}")]
        [System.Web.Http.HttpDelete]
        public void ProductDelete(string id)
        {
            try
            {
                var productInDb = _context.WarehouseProducts.SingleOrDefault(p => p.PId == id);
                _context.WarehouseProducts.Remove(productInDb);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return; 
            }
        }
        
        [System.Web.Http.Route("Api/Home/Warehouse/ProductSDelete/{id}")]
        [System.Web.Http.HttpDelete]
        public void ProductSDelete(string id)
        {
            try
            {
                var productInDb = _context.WarehouseProducts.SingleOrDefault(p => p.PId == id);
                var stockInDb = _context.WarehouseStocks.SingleOrDefault(s => s.ProductID == id);
                _context.WarehouseStocks.Remove(stockInDb);
                _context.WarehouseProducts.Remove(productInDb);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return;
            }
        }

        //Recover Deleted Product
        [System.Web.Http.Route("Api/Home/Warehouse/ProductUnDelete/{id}")]
        [System.Web.Http.HttpDelete]
        public void ProductUnDelete(string id)
        {
            try
            {
                var productInDb = _context.WarehouseProducts.SingleOrDefault(p => p.PId == id);
                productInDb.Status = 0;
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return;
            }
        }
       
        //Warehouse Order Delete
        [System.Web.Http.Route("Api/Home/Warehouse/OrderDelete/{id}")]
        [System.Web.Http.HttpDelete]
        public void OrderDelete(string id)
        {
            try
            {
                List<WarehouseOrder> warehouseOrders = _context.WarehouseOrders.Where(o => o.OId.Equals(id)).ToList();
                var orderdetail = _context.WarehouseOrderDetails.SingleOrDefault(o => o.OrderID.Equals(id));
                var ordertype = _context.WarehouseOrderTypes.SingleOrDefault(o => o.OId.Equals(id));
                _context.WarehouseOrderDetails.Remove(orderdetail);
                _context.WarehouseOrderTypes.Remove(ordertype);
                for (int i = 0; i < warehouseOrders.Count; i++)
                {
                    _context.WarehouseOrders.Remove(warehouseOrders[i]);

                }
                
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return;
            }
        }

        [System.Web.Http.Route("Api/Home/Warehouse/OrderUnDelete/{id}")]
        [System.Web.Http.HttpDelete]
        public void OrderUnDelete(string id)
        {
            try
            {
               
                var ordertype = _context.WarehouseOrderTypes.SingleOrDefault(o => o.OId.Equals(id));
                ordertype.Status = 0;
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return;
            }
        }

        //Delete Production Order
        [System.Web.Http.Route("Api/Home/ProductionsDeleteOrder/{id}")]
        [System.Web.Http.HttpDelete]
        public void ProductionsDeleteOrder(string id)
        {
            try
            {
                var orderInDb = _context.ProductionOrders.SingleOrDefault(o => o.Id.Equals(id));
                _context.ProductionOrders.Remove(orderInDb);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
