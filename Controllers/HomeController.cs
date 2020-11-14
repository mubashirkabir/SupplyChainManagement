using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Schema;
using SupplyChainManagement.Models;
using SupplyChainManagement.ViewModels;

namespace FinalYearProject.Controllers
{
    public class HomeController : Controller
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

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserLogin user)
        {

            var userInDB = _context.UsersDetails.Single(u => u.UserName == user.UserName.ToLower());

            if (encryptPassword(user.UserPassword) == userInDB.UserPassword)
            {
                Session["username"] = userInDB.UserName.ToLower();
                Session["userrole"] = userInDB.RoleId.ToString();
                Session["fullname"] = userInDB.FullName.ToUpper();

                if (userInDB.RoleId == 1)
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                else if (userInDB.RoleId == 2)
                {
                    return Redirect("~/Warehouse/");
                }
                else if (userInDB.RoleId == 3)
                {
                    return Redirect("~/Vendor/");
                }
                else if (userInDB.RoleId == 4)
                {
                    return Redirect("~/Production/");
                }
                else if (userInDB.RoleId == 5)
                {
                    return Redirect("~/EmployeeManagement/");
                }
                else if (userInDB.RoleId == 6)
                {
                    return Redirect("~/VendorView/Index/" + user.UserName);
                }
                return View();
            }
            else
            {
                return View();
            }
        }

        public ActionResult Dashboard()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["userrole"].ToString()) != 1)
                {
                    Response.Redirect("~/Home/Index");
                }

            }
            else
            {
                Response.Redirect("~/Home/Index");

            }
            List<WarehouseOrder> orderList = _context.WarehouseOrders.ToList();
            List<WarehouseCompletedOrders> completedOrdersList = _context.WarehouseCompletedOrderses.ToList();
            List<WarehouseOrderDetail> orderDetailsList = _context.WarehouseOrderDetails.ToList();
            int[] noOfOrders = new int[12];
            int[] noOfCOrders = new int[12];
            string newId = "";
            string prevId = "";

            for (int i = 0; i < orderList.Count; i++)
            {
                newId = orderList[i].OId;
                if (newId != prevId)
                {
                    if (orderList[i].Date.Month == 1)
                    {
                        noOfOrders[0]++;
                    }
                    else if (orderList[i].Date.Month == 2)
                    {
                        noOfOrders[1]++;
                    }
                    else if (orderList[i].Date.Month == 3)
                    {
                        noOfOrders[2]++;
                    }
                    else if (orderList[i].Date.Month == 4)
                    {
                        noOfOrders[3]++;
                    }
                    else if (orderList[i].Date.Month == 5)
                    {
                        noOfOrders[4]++;
                    }
                    else if (orderList[i].Date.Month == 6)
                    {
                        noOfOrders[5]++;
                    }
                    else if (orderList[i].Date.Month == 7)
                    {
                        noOfOrders[6]++;
                    }
                    else if (orderList[i].Date.Month == 8)
                    {
                        noOfOrders[7]++;
                    }
                    else if (orderList[i].Date.Month == 9)
                    {
                        noOfOrders[8]++;
                    }
                    else if (orderList[i].Date.Month == 10)
                    {
                        noOfOrders[9]++;
                    }
                    else if (orderList[i].Date.Month == 11)
                    {
                        noOfOrders[10]++;
                    }
                    else if (orderList[i].Date.Month == 12)
                    {
                        noOfOrders[11]++;
                    }
                    else
                    {

                    }

                    prevId = orderList[i].OId;
                }
            }

            newId = "";
            prevId = "";
            for (int i = 0; i < completedOrdersList.Count; i++)
            {
                newId = completedOrdersList[i].OId;
                if (newId != prevId)
                {
                    if (completedOrdersList[i].Date.Month == 1)
                    {
                        noOfCOrders[0]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 2)
                    {
                        noOfCOrders[1]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 3)
                    {
                        noOfCOrders[2]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 4)
                    {
                        noOfCOrders[3]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 5)
                    {
                        noOfCOrders[4]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 6)
                    {
                        noOfCOrders[5]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 7)
                    {
                        noOfCOrders[6]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 8)
                    {
                        noOfCOrders[7]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 9)
                    {
                        noOfCOrders[8]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 10)
                    {
                        noOfCOrders[9]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 11)
                    {
                        noOfCOrders[10]++;
                    }
                    else if (completedOrdersList[i].Date.Month == 12)
                    {
                        noOfCOrders[11]++;
                    }
                    else
                    {

                    }
                    prevId = completedOrdersList[i].OId;
                }
            }
            string[] strOArray = Array.ConvertAll(noOfOrders, ele => ele.ToString());
            string[] strCOArray = Array.ConvertAll(noOfCOrders, ele => ele.ToString());
            ViewBag.NoOfOrder = strOArray;
            ViewBag.NoOfCOrder = strCOArray;


            //Sales in months
            float[] Sales = new float[12];
            var OrderMonthSales = (from co in completedOrdersList
                                   join od in orderDetailsList on co.OId equals od.OrderID
                                   where od.Sender.Equals("WAREHOUSE") && od.Receiver.Equals("SALES")
                                   select new WarehouseOrder()
                                   {
                                       OId = co.OId,
                                       Date = co.Date,
                                       TotalPrice = co.TotalPrice
                                   }).ToList();

            for (int i = 0; i < OrderMonthSales.Count; i++)
            {
                if (OrderMonthSales[i].Date.Month == 1)
                {
                    Sales[0] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 2)
                {
                    Sales[1] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 3)
                {
                    Sales[2] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 4)
                {
                    Sales[3] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 5)
                {
                    Sales[4] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 6)
                {
                    Sales[5] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 7)
                {
                    Sales[6] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 8)
                {
                    Sales[7] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 9)
                {
                    Sales[8] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 10)
                {
                    Sales[9] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 11)
                {
                    Sales[10] += OrderMonthSales[i].TotalPrice;
                }
                else if (OrderMonthSales[i].Date.Month == 12)
                {
                    Sales[11] += OrderMonthSales[i].TotalPrice;
                }
                else
                {

                }
            }
            string[] strOfTotalSales = Array.ConvertAll(Sales, ele => ele.ToString());
            ViewBag.TotalSales = strOfTotalSales;

            IDictionary<string, int> saledProducts = new Dictionary<string, int>();
            var warehouseOrders = _context.WarehouseOrders.ToList();
            for (int i = 0; i < warehouseOrders.Count; i++)
            {
                string ProductIDinList = warehouseOrders[i].PId;
                int quantityPID = warehouseOrders[i].PQuantity;
                bool ifFound = false;
                for (int j = 0; j < saledProducts.Count; j++)
                {
                    if (saledProducts.Keys.ElementAt(j).Equals(ProductIDinList))
                    {
                        ifFound = true;
                        break;
                    }
                }

                if (ifFound == true)
                {
                    saledProducts[ProductIDinList] += quantityPID;
                }
                else
                {
                    saledProducts.Add(ProductIDinList, quantityPID);
                }
            }


            var OrderedHotSales = saledProducts.OrderBy(key => key.Value).ToList();
            string [] HotProductID = new string[5];
            int [] HotProductQuantity = new int[5];
            int count = 0;
            if (OrderedHotSales.Count >= 5)
            {
                for (int i = OrderedHotSales.Count-1; i > OrderedHotSales.Count-6; i--)
                {
                    HotProductID[count] = OrderedHotSales[i].Key;
                    HotProductQuantity[count] = OrderedHotSales[i].Value;
                    count++;
                }
            }
            else
            {
                for (int i = OrderedHotSales.Count-1; i >= 0; i--)
                {
                    HotProductID[count] = OrderedHotSales[i].Key;
                    HotProductQuantity[count] = OrderedHotSales[i].Value;
                    count++;
                }
            }

            string[] HotProductName = new string[5];

            for (int i = 0; i < 5; i++)
            {
                string temp = HotProductID[i];
                if (temp != null)
                {
                    var ProductInDb = _context.WarehouseProducts.SingleOrDefault(p => p.PId == temp);
                    HotProductName[i] = ProductInDb.Name;
                }
            }

            ViewBag.HotProductName = HotProductName;
            ViewBag.HotProduct = HotProductID;
            ViewBag.HotProductQuantity = HotProductQuantity;
            return View();
        }

        //All User controller
        public ActionResult UserAll()
        {
            List<UsersDetail> usersInDb = _context.UsersDetails.ToList();
            return View(usersInDb);
        }


        public ActionResult UserAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserAdd(UsersDetail usersDetail)
        {
            var user = new UsersDetail();

            user.UserName = usersDetail.UserName.ToLower();
            user.FullName = usersDetail.FullName;
            user.PhoneNumber = usersDetail.PhoneNumber;
            user.UserPassword = encryptPassword(usersDetail.UserPassword);
            user.RoleId = usersDetail.RoleId;

            try
            {
                _context.UsersDetails.Add(user);
                _context.SaveChanges();
            }
            catch
            {
                return View();
            }
            return Redirect("~/home/UserAll");

        }



        [Route("home/UserEdit/{id}")]
        public ActionResult UserEdit(String Id)
        {
            int x = Int32.Parse(Id);
            var userInDb = _context.UsersDetails.SingleOrDefault(u => u.Id == x);
            return View(userInDb);
        }


        public ActionResult Update(UsersDetail usersDetail)
        {
            var UserInDb = _context.UsersDetails.SingleOrDefault(u => u.Id == usersDetail.Id);

            UserInDb.UserName = usersDetail.UserName;
            UserInDb.FullName = usersDetail.FullName;
            UserInDb.PhoneNumber = usersDetail.PhoneNumber;
            UserInDb.RoleId = Int32.Parse(Request.Form["RoleId"]);
            UserInDb.UserPassword = encryptPassword(Request.Form["UserPassword"]);
            _context.SaveChanges();
            return Redirect("~/home/UserAll");
        }

        public ActionResult SearchUser()
        {
            return View();
        }

        public static string encryptPassword(string x)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider Md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] dataInByte = System.Text.Encoding.ASCII.GetBytes(x);
            dataInByte = Md5.ComputeHash(dataInByte);
            String md5Hash = System.Text.Encoding.ASCII.GetString(dataInByte);

            return md5Hash;
        }


        // Reports
        public ActionResult WarehouseReports()
        {
            return View();
        }

        public ActionResult VendorReports()
        {
            return View();
        }

        public ActionResult ProductionReports()
        {
            return View();
        }

        //Warehouse Navigate
        public ActionResult WarehouseAllProducts()
        {
            var products = _context.WarehouseProducts.ToList();
            var productlist = (from p in products
                select new WarehouseProduct()
                {
                    PId = p.PId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Category = p.Category,
                    Status = p.Status,
                    Type = p.Type

                }).ToList();
            return View(productlist);
        }

        public ActionResult WarehouseInventory()
        {
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
        }

        public ActionResult WarehouseOrders()
        {
            List<WarehouseOrder> orders = _context.WarehouseOrders.ToList();
            List<WarehouseOrderDetail> details = _context.WarehouseOrderDetails.ToList();
            List<WarehouseOrderType> type = _context.WarehouseOrderTypes.ToList();
            var OrderList = (from o in orders
                join d in details on o.OId equals d.OrderID
                join t in type on o.OId equals t.OId
                select new WarehouseOrderJoin()
                {
                    WarehouseOrderj = o,
                    WarehouseOrderDetailj = d,
                    WarehouseOrderTypej = t
                }).ToList();

            return View(OrderList);
        }


        //Production Management

        public ActionResult ProductionAllOrders()
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
            return View(productionInDb);
        }

        public ActionResult ProductionAddOrder()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult AddProductonOrder(ProductionOrder Item)
        {
            string hour = DateTime.Now.ToString("HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string min = DateTime.Now.ToString("mm", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string month = DateTime.Now.ToString("MM", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string year = DateTime.Now.ToString("yy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string OID = "OP" + hour + min + month + year;
            var addOrder = new ProductionOrder();
            addOrder.Id = OID;
            addOrder.Title = Item.Title;
            addOrder.ExpectedDate = Item.ExpectedDate;
            addOrder.StartDate = Item.StartDate;
            addOrder.PId = Item.PId;
            addOrder.PQuantity = Int32.Parse(Request.Form["PQuantity"]);
            addOrder.Status = "InProgress";
            addOrder.Notification = 0;
            _context.ProductionOrders.Add(addOrder);
            _context.SaveChanges();
            return Redirect("~/Home/ProductionAllOrders");
        }


        //Vendor Management
        public ActionResult VendorAll()
        {
            var vendorList = _context.VendorDetails.ToList();
            return View(vendorList);
            
        }

        public ActionResult VendorOrders()
        {
            List<VendorDetail> vendorlist = _context.VendorDetails.ToList();
            List<VendorOrder> vendororder = _context.VendorOrders.ToList();
            List<VendorOrderDetail> orderDetails = _context.VendorOrderDetails.ToList();
            var finallist = (from vl in vendorlist
                join vo in vendororder on vl.VId equals vo.Id
                join od in orderDetails on vo.OId equals od.Id
                select new VendorOrderViewModel
                {
                    VendorDetail = vl,
                    VendorOrder = vo,
                    VendorOrderDetail = od
                }).ToList();

            return View(finallist);
        }

        public ActionResult VendorAddOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VendorAddOrder(WarehouseVendorRequest warehouseVendorRequest)
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
    }
}