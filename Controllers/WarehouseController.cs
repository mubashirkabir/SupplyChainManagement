using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;
using SupplyChainManagement.Models;
using SupplyChainManagement.ViewModels;
using WebGrease.Css.Ast.Selectors;


namespace SupplyChainManagement.Controllers
{
    public class WarehouseController : Controller
    {
        private ApplicationDbContext _context;


        public WarehouseController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();

        }


        // GET: Warehouse
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["userrole"].ToString()) != 2)
                {
                    Response.Redirect("~/Home/Index");
                }

            }
            else
            {
                Response.Redirect("~/Home/Index");

            }
            //var products = _context.WarehouseStocks.ToList();
            //var stock = _context.WarehouseStocks;
            //var datasend = new WarehouseProductJoin
            //{
            //    WProducts = products,
            //    WStocks = stock
            //};
            //var products = _context.Wproducts;

            //DbSet<WarehouseStock> warehouseslist = _context.WarehouseStocks;
            //WarehouseProductJoin warehouseProductJoins = new WarehouseProductJoin();
            //List<WarehouseProductJoin> warehouseProductJoinslist = warehouseslist.Select(x => new WarehouseProductJoin
            //{
            //    PId = x.WarehouseProduct.PId, Name = x.WarehouseProduct.Name, Price = x.WarehouseProduct.Price,
            //    ShelveNo = x.ShelveNo, Quantity = x.Quantity, Type = x.WarehouseProduct.Type,
            //    Description = x.WarehouseProduct.Description
            //}).ToList();

            //warehouseProductJoins.WProducts = _context.WarehouseProducts.ToList();
            //warehouseProductJoins.WStocks = _context.WarehouseStocks.ToList();



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

            return View(StockList);
            //}
        }

        public ActionResult AllProducts()
        {
            var products = _context.WarehouseProducts.ToList();
            var productlist = (from p in products
                               where p.Status == 0
                               select new WarehouseProduct()
                               {
                                   PId = p.PId,
                                   Name = p.Name,
                                   Description = p.Description,
                                   Price = p.Price,
                                   Category = p.Category,
                                   Type = p.Type

                               }).ToList();
            return View(productlist);
        }


        public ActionResult AddProduct()
        {
            return View();

        }


        [HttpPost]
        public ActionResult AddProduct(WarehouseProductJoin warehouseProductJoin)
        {
            var products = new WarehouseProduct();
            var stock = new WarehouseStock();

            products.PId = warehouseProductJoin.Wproducts.PId.ToUpper();
            products.Name = warehouseProductJoin.Wproducts.Name;
            products.Price = warehouseProductJoin.Wproducts.Price;
            products.Type = Request.Form["PType"];
            products.Category = Request.Form["PCategory"];
            products.Description = warehouseProductJoin.Wproducts.Description;

            stock.ProductID = warehouseProductJoin.Wproducts.PId;
            stock.Quantity = warehouseProductJoin.WStock.Quantity;
            stock.ShelveNo = warehouseProductJoin.WStock.ShelveNo;

            try
            {
                _context.WarehouseProducts.Add(products);
                _context.WarehouseStocks.Add(stock);
                _context.SaveChanges();
            }
            catch
            {
                return View();
            }
            return Redirect("~/Warehouse/Index");
        }



        public ActionResult Orders()
        {
            List<WarehouseOrder> orders = _context.WarehouseOrders.ToList();
            List<WarehouseOrderDetail> details = _context.WarehouseOrderDetails.ToList();
            List<WarehouseOrderType> type = _context.WarehouseOrderTypes.ToList();
            var OrderList = (from o in orders
                             join d in details on o.OId equals d.OrderID
                             join t in type on o.OId equals t.OId
                             where t.Status == 0
                             orderby o.OId descending 
                             select new WarehouseOrderJoin()
                             {
                                 WarehouseOrderj = o,
                                 WarehouseOrderDetailj = d,
                                 WarehouseOrderTypej = t
                             }).ToList();

            return View(OrderList);
        }

        public ActionResult BoundIn()
        {
            List<WarehouseOrder> orders = _context.WarehouseOrders.ToList();
            List<WarehouseOrderDetail> details = _context.WarehouseOrderDetails.ToList();
            List<WarehouseOrderType> type = _context.WarehouseOrderTypes.ToList();
            var OrderList = (from o in orders
                             join d in details on o.OId equals d.OrderID
                             join t in type on o.OId equals t.OId
                             where t.type == 1
                             select new WarehouseOrderJoin()
                             {
                                 WarehouseOrderj = o,
                                 WarehouseOrderDetailj = d,
                                 WarehouseOrderTypej = t
                             }).ToList();

            return View(OrderList);
        }

        public ActionResult BoundOut()
        {

            List<WarehouseOrder> orders = _context.WarehouseOrders.ToList();
            List<WarehouseOrderDetail> details = _context.WarehouseOrderDetails.ToList();
            List<WarehouseOrderType> type = _context.WarehouseOrderTypes.ToList();
            var OrderList = (from o in orders
                             join d in details on o.OId equals d.OrderID
                             join t in type on o.OId equals t.OId
                             where t.type == 2
                             select new WarehouseOrderJoin()
                             {
                                 WarehouseOrderj = o,
                                 WarehouseOrderDetailj = d,
                                 WarehouseOrderTypej = t
                             }).ToList();
            return View(OrderList);
        }

        public ActionResult SearchProduct()
        {
            return View();
        }

        [Route("warehouse/edit/{id}")]
        public ActionResult Edit(string id)
        {

            var product = _context.WarehouseProducts.SingleOrDefault(p => p.PId == id);
            var stock = _context.WarehouseStocks.SingleOrDefault(s => s.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var warehouseProductJoin = new WarehouseProductJoin();
            if (stock == null)
            {
                int quantity = 0;
                warehouseProductJoin.Wproducts = product;
                WarehouseStock warehouseStock = new WarehouseStock();
                warehouseStock.Quantity = quantity;
                warehouseStock.ShelveNo = "N/A";
                warehouseProductJoin.WStock = warehouseStock;
            }
            else
            {
                warehouseProductJoin.Wproducts = product;
                warehouseProductJoin.WStock = stock;
            }
            return View(warehouseProductJoin);

        }


        [HttpPost]
        public ActionResult Update(WarehouseProductJoin warehouseProductJoin)
        {

            var productInDb = _context.WarehouseProducts.Single(p => p.PId == warehouseProductJoin.Wproducts.PId);
            var stockInDb = _context.WarehouseStocks.Single(s => s.ProductID == warehouseProductJoin.Wproducts.PId);
            productInDb.PId = warehouseProductJoin.Wproducts.PId;
            productInDb.Name = warehouseProductJoin.Wproducts.Name;
            productInDb.Price = warehouseProductJoin.Wproducts.Price;
            productInDb.Type = Request.Form["PType"];
            productInDb.Category = Request.Form["PCategory"];
            productInDb.Description = warehouseProductJoin.Wproducts.Description;
            stockInDb.Quantity = warehouseProductJoin.WStock.Quantity;
            stockInDb.ShelveNo = warehouseProductJoin.WStock.ShelveNo;
            _context.SaveChanges();
            return Redirect("~/Warehouse/Index");

        }

        public ActionResult AddOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddOrder(WarehouseOrderJoin warehouseOrderJoin)
        {

            var OrderId = Request.Form["OrderID"];
            var date = Request.Form["date"];
            var orderType = Int32.Parse(Request.Form["OrderType"]);
            var orderSender = Request.Form["OrderSender"];
            var OrderReciever = Request.Form["OrderReciever"];
            bool condition = false;

            int NoOfProducts = Int32.Parse(Request.Form["length"]);
            double price = 0;

            for (int i = 0; i <= NoOfProducts; i++)
            {
                var product = Request.Form["PID[" + i + "]"];
                var quatity = Int32.Parse(Request.Form["PQ[" + i + "]"]);
                var stockInDb = _context.WarehouseStocks.SingleOrDefault(p => p.ProductID == product);
                var productInDb = _context.WarehouseProducts.SingleOrDefault(p => p.PId == product);
                if (stockInDb != null)
                {
                    if (stockInDb.Quantity >= quatity)
                    {
                        condition = true;
                    }
                    else
                    {
                        condition = false;
                        break;
                    }
                }
                else if (productInDb != null)
                {
                    condition = false;
                    break;
                }
                else
                {
                    condition = false;
                    break;
                }

            }

            if (condition == true)
            {
                for (int i = 0; i <= NoOfProducts; i++)
                {
                    var product = Request.Form["PID[" + i + "]"];
                    var quatity = Int32.Parse(Request.Form["PQ[" + i + "]"]);

                    if (product == null || product == "")
                    {

                    }
                    else
                    {
                        var Order = new WarehouseOrder();
                        var OrderDetail = new WarehouseOrderDetail();
                        var OrderType = new WarehouseOrderType();
                        var stockInDb = _context.WarehouseStocks.SingleOrDefault(p => p.ProductID == product);

                        try
                        {
                            //Calculating price for each product w.r.t to quantity
                            var productPrice =
                                _context.WarehouseProducts.SingleOrDefault(p => p.PId == product);
                            var tprice = quatity * productPrice.Price;

                            Order.TotalPrice = tprice;
                        }
                        catch (Exception e)
                        {
                            continue;
                        }


                        //Setting Id to all of them
                        Order.OId = OrderId;
                        Order.Notification = 0;
                        OrderDetail.OrderID = OrderId;
                        OrderType.OId = OrderId;

                        //Setting remaining instances
                        Order.PId = Request.Form["PID[" + i + "]"];
                        Order.PQuantity = Int32.Parse(Request.Form["PQ[" + i + "]"]);
                        Order.Date = DateTime.Parse(date);
                        OrderDetail.Receiver = OrderReciever;
                        OrderDetail.Sender = orderSender;


                        if (i == 0)
                        {
                            OrderType.type = orderType;
                            _context.WarehouseOrderDetails.Add(OrderDetail);
                            _context.WarehouseOrderTypes.Add(OrderType);
                        }

                        try
                        {

                            _context.WarehouseOrders.Add(Order);
                            _context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.ToString());
                        }
                    }
                }

            }
            else
            {
                return Redirect("~/Warehouse/OrderError");
            }

            return Redirect("~/Warehouse/Orders");
        }


        public ActionResult OrderError()
        {
            return View();
        }



        [Route("warehouse/editorder/{id}/{WOsNo}/{WODsNo}")]
        public ActionResult EditOrder(string id, string WOsNo, String WODsNo)
        {
            int x = Int32.Parse(WOsNo);
            int y = Int32.Parse(WODsNo);
            var orders = _context.WarehouseOrders.SingleOrDefault(o => o.SNo == x);
            var orderdetail = _context.WarehouseOrderDetails.SingleOrDefault(o => o.SNo == y);
            var ordertype = _context.WarehouseOrderTypes.SingleOrDefault(o => o.OId == id);

            if (orders == null)
            {
                return HttpNotFound();
            }
            var Warehouseorderjoin = new WarehouseOrderJoin();
            Warehouseorderjoin.WarehouseOrderj = orders;
            Warehouseorderjoin.WarehouseOrderDetailj = orderdetail;
            Warehouseorderjoin.WarehouseOrderTypej = ordertype;

            return View(Warehouseorderjoin);

        }

        [HttpPost]
        public ActionResult Updateorder(WarehouseOrderJoin warehouseOrderJoin)
        {
            int x = Int32.Parse(Request.Form["WOsNo"]);
            int y = Int32.Parse(Request.Form["WODsNo"]);
            var orders = _context.WarehouseOrders.SingleOrDefault(o => o.SNo == x);
            var orderdetail = _context.WarehouseOrderDetails.SingleOrDefault(o => o.SNo == y);
            var productPrice =
                _context.WarehouseProducts.SingleOrDefault(p => p.PId == warehouseOrderJoin.WarehouseOrderj.PId);
            var tprice = warehouseOrderJoin.WarehouseOrderj.PQuantity * productPrice.Price;

            orders.OId = warehouseOrderJoin.WarehouseOrderDetailj.OrderID;
            orderdetail.OrderID = warehouseOrderJoin.WarehouseOrderDetailj.OrderID;

            orders.PId = warehouseOrderJoin.WarehouseOrderj.PId;
            orders.Notification = 2;
            orders.PQuantity = warehouseOrderJoin.WarehouseOrderj.PQuantity;
            orders.TotalPrice = tprice;


            orderdetail.Sender = Request.Form["OrderSender"];
            orderdetail.Receiver = Request.Form["OrderReciever"];

            _context.SaveChanges();

            return Redirect("~/Warehouse/Orders");
        }

        public ActionResult WVendor()
        {
            var warehouseVendorRequest = _context.WarehouseVendorRequests.ToList();
            return View(warehouseVendorRequest);
        }

        public ActionResult AddVendorOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddVendorOrder(WarehouseVendorRequest warehouseVendorRequest)
        {
            WarehouseVendorRequest WVRequest = new WarehouseVendorRequest();
            WVRequest = warehouseVendorRequest;
            try
            {
                _context.WarehouseVendorRequests.Add(WVRequest);
                _context.SaveChanges();
            }
            catch
            {
                return View();
            }
            return Redirect("~/Warehouse/WVendor");
        }

        [Route("warehouse/EditVOrder/{id}")]
        public ActionResult EditVOrder(string id)
        {
            if (id == null)
            {
                return View("WVendor");
            }
            var orderInDb = _context.WarehouseVendorRequests.SingleOrDefault(o => o.OID == id);
            return View(orderInDb);
        }

        [HttpPost]
        public ActionResult EditVendorOrder(WarehouseVendorRequest warehouseVendorRequest)
        {
            var warehouseVendor = _context.WarehouseVendorRequests.Single(o => o.OID == warehouseVendorRequest.OID);
            warehouseVendor.OID = warehouseVendorRequest.OID;
            warehouseVendor.Pid = warehouseVendorRequest.Pid;
            warehouseVendor.Quantity = warehouseVendorRequest.Quantity;
            _context.SaveChanges();
            return Redirect("~/Warehouse/WVendor");
        }

        public ActionResult PrintOrderReceipt()
        {
            return View();
        }

        [Route("warehouse/OrderReceipt/{id}")]
        public ActionResult OrderReceipt(String id)
        {
            if (id == null)
            {
                return View("PrintOrderReceipt");
            }

            //List<WarehouseOrder> allOrders = _context.WarehouseOrders.ToList();
            //List<WarehouseOrderType> allOrderstype = _context.WarehouseOrderTypes.ToList();
            //List<WarehouseOrderDetail> allOrdersdetail = _context.WarehouseOrderDetails.ToList();

            //var ordersProducts = (from o in allOrders
            //    join ot in allOrderstype on o.OId equals ot.OId 
            //    join od in allOrdersdetail on o.OId equals od.OrderID
            //    select new WarehouseOrderJoin()
            //    {
            //        WarehouseOrderj = o,
            //        WarehouseOrderTypej = ot,
            //        WarehouseOrderDetailj = od
            //    }).ToList();

            List<WarehouseOrder> orders = _context.WarehouseOrders.ToList();
            WarehouseOrderDetail details = _context.WarehouseOrderDetails.SingleOrDefault(o => o.OrderID == id);
            WarehouseOrderType type = _context.WarehouseOrderTypes.SingleOrDefault(o => o.OId == id);
            var OrderList = (from o in orders
                             where o.OId.Contains(id)
                             select new WarehouseOrderJoin()
                             {
                                 WarehouseOrderj = o,
                                 WarehouseOrderDetailj = details,
                                 WarehouseOrderTypej = type
                             }).ToList();

            ViewBag.ID = id;
            return View(OrderList);
        }

        public ActionResult CompletedOrders()
        {
            List<WarehouseCompletedOrders> orders = _context.WarehouseCompletedOrderses.ToList();
            List<WarehouseOrderDetail> details = _context.WarehouseOrderDetails.ToList();
            List<WarehouseOrderType> type = _context.WarehouseOrderTypes.ToList();
            var OrderList = (from o in orders
                join d in details on o.OId equals d.OrderID
                join t in type on o.OId equals t.OId
                orderby o.Date descending
                select new WarehouseCompletedOrderJoin()
                {
                    WarehouseCompletedOrdersj = o,
                    WarehouseOrderDetailj = d,
                    WarehouseOrderTypej = t
                }).ToList();

            return View(OrderList);
        }


    }
}