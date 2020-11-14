using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Security;
using Microsoft.SqlServer.Server;
using SupplyChainManagement.Migrations;
using SupplyChainManagement.Models;
using SupplyChainManagement.ViewModels;

namespace SupplyChainManagement.Controllers.Api
{
    public class WarehousesController : ApiController
    {
        private ApplicationDbContext _context;

        public WarehousesController()
        {
            _context = new ApplicationDbContext();
        }

        [Route("Api/Warehouses")]
        [HttpGet]
        // Get /Api/Warehouses
        public IEnumerable<WarehouseProduct> Warehouses()
        {

            var products = _context.WarehouseProducts.ToList();
            return products;
        }

        [Route("Api/Warehouses/{Pid}")]
        [HttpGet]
        //Get /Api/Warehouses/{pid}
        public IEnumerable<WarehouseProduct> Warehouses(string Pid)
        {

            List<WarehouseProduct> warehouseproductList = _context.WarehouseProducts.ToList();
            var OrderList = (from o in warehouseproductList
                             where o.PId.Contains(Pid)
                             select new WarehouseProduct()
                             {
                                 PId = o.PId,
                                 Name = o.Name,
                                 Type = o.Type,
                                 Price = o.Price,
                                 Description = o.Description
                             }).ToList();

            if (OrderList == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return OrderList;
        }

        [Route("Api/Warehouses/{id}")]
        [HttpDelete]
        //PUT /api/Warehouses/1
        public void DeleteProduct(string id)
        {
            var product = _context.WarehouseProducts.SingleOrDefault(p => p.PId == id);
            var stock = _context.WarehouseStocks.SingleOrDefault(s => s.ProductID == id);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            product.Status = 1;
            if (stock != null)
            {
                stock.Status = 1;
            }

            _context.SaveChanges();

        }

        [Route("Api/WarehousesOrder/{id}")]
        [HttpDelete]
        //PUT /api/Warehouses/1
        public void DeleteOrder(string id)
        {

            var ordertype = _context.WarehouseOrderTypes.SingleOrDefault(o => o.OId == id);
            if (ordertype == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }


            if (ordertype != null)
            {
                ordertype.Status = 1;
            }

            _context.SaveChanges();

        }

        [Route("Api/WarehousesVOrder/{id}")]
        [HttpDelete]
        //PUT /api/Warehouses/1
        public void DeleteVOrder(string id)
        {
            var orders = _context.WarehouseVendorRequests.SingleOrDefault(o => o.OID == id);
            if (orders == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }


            if (orders != null)
            {
                _context.WarehouseVendorRequests.Remove(orders);
            }

            _context.SaveChanges();

        }

        [Route("Api/QuantityCheck/{id}/{q}")]
        [HttpGet]
        //PUT /api/QuantityCheck/1
        public string QuantityCheck(string id, string q)
        {
            var stock = _context.WarehouseStocks.SingleOrDefault(p => p.ProductID == id);
            var product = _context.WarehouseProducts.SingleOrDefault(p => p.PId == id);
            if (stock != null)
            {
                if (stock.Quantity >= Int32.Parse(q))
                {
                    return "Quantity Available";
                }
                else
                {
                    return "Max quantity available is: " + stock.Quantity;
                }
            }
            else if (product != null)
            {
                return "No stock available for this product";
            }
            else
            {
                return "Enter valid product ID";
            }




        }


        [Route("Api/SearchOrder/{id}")]
        [HttpGet]
        //Put /api/searchorder/1
        public IEnumerable<WarehouseOrder> SearchOrder(String id)
        {
            List<WarehouseOrder> warehouseOrders = _context.WarehouseOrders.ToList();
            List<WarehouseOrderType> warehouseOrderTypes = _context.WarehouseOrderTypes.ToList();
            List<WarehouseOrderDetail> warehouseOrderDetails = _context.WarehouseOrderDetails.ToList();

            var OrderList = (from o in warehouseOrders
                             join wd in warehouseOrderDetails on o.OId equals wd.OrderID
                             where o.OId.Contains(id.ToUpper()) || o.PId.Contains(id.ToUpper()) || wd.Receiver.Contains(id.ToUpper()) || wd.Sender.Contains(id.ToUpper())
                             select new WarehouseOrder()
                             {
                                 OId = o.OId,
                                 Date = o.Date

                             }).ToList();

            if (OrderList == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return OrderList;
        }

        [Route("Api/ProductSuggestion/{id}")]
        [HttpGet]
        //Put /api/searchorder/1
        public IEnumerable<WarehouseProduct> ProductSuggestion(String id)
        {


            List<WarehouseProduct> warehouseProducts =
                _context.WarehouseProducts.Where(p => p.PId.Contains(id)).ToList();

            if (warehouseProducts == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return warehouseProducts;
        }

        [Route("Api/Warehouse/Completed/{id}/{shelfNo}")]
        [HttpDelete]
        //PUT /api/Warehouses/Completed/1
        public void Completed(string id, string shelfNo)
        {
            string date = DateTime.Now.ToString("d", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            var warehouseOrderType = _context.WarehouseOrderTypes.SingleOrDefault(ot => ot.OId == id);
            List<WarehouseOrder> orders = _context.WarehouseOrders.Where(o => o.OId == id).ToList();
            bool added = false;
            if (warehouseOrderType.type == 1)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    bool found = false;
                    try
                    {
                        var PID = orders[i].PId;
                        int q = orders[i].PQuantity;
                        var ProductInDb =
                            _context.WarehouseStocks.SingleOrDefault(pid => pid.ProductID.Equals(PID));
                        ProductInDb.Quantity = ProductInDb.Quantity + q;
                        found = true;
                        added = true;
                        _context.SaveChanges();

                    }
                    catch (Exception e)
                    {

                    }
                    if (found == false)
                    {
                        try
                        {
                            var ProductAdd = new WarehouseStock();
                            ProductAdd.ProductID = orders[i].PId;
                            ProductAdd.ShelveNo = shelfNo;
                            ProductAdd.Quantity = orders[i].PQuantity;
                            _context.WarehouseStocks.Add(ProductAdd);
                            added = true;
                            _context.SaveChanges();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    if (added == true)
                    {
                        var completedOrder = new WarehouseCompletedOrders();
                        completedOrder.OId = id;
                        completedOrder.PId = orders[i].PId;
                        completedOrder.PQuantity = orders[i].PQuantity;
                        completedOrder.TotalPrice = orders[i].TotalPrice;
                        completedOrder.Date = DateTime.Parse(date);
                        _context.WarehouseCompletedOrderses.Add(completedOrder);
                        _context.WarehouseOrders.Remove(orders[i]);
                        _context.SaveChanges();
                    }
                }

            }
            if (warehouseOrderType.type == 2)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    try
                    {
                        var PID = orders[i].PId;
                        int q = orders[i].PQuantity;
                        var ProductInDb =
                            _context.WarehouseStocks.SingleOrDefault(pid => pid.ProductID.Equals(PID));

                        ProductInDb.Quantity = ProductInDb.Quantity - q;
                        _context.SaveChanges();
                        added = true;
                    }
                    catch (Exception e)
                    {
                    }
                    if (added == true)
                    {
                        var completedOrder = new WarehouseCompletedOrders();
                        completedOrder.OId = id;
                        completedOrder.PId = orders[i].PId;
                        completedOrder.PQuantity = orders[i].PQuantity;
                        completedOrder.TotalPrice = orders[i].TotalPrice;
                        completedOrder.Date = DateTime.Parse(date);
                        completedOrder.Notification = 0;
                        _context.WarehouseCompletedOrderses.Add(completedOrder);
                        _context.WarehouseOrders.Remove(orders[i]);
                        _context.SaveChanges();
                    }
                }

            }
            _context.SaveChanges();

        }

    }
}