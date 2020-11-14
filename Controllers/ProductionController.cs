using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding.Binders;
using System.Web.Mvc;
using SupplyChainManagement.Models;
using SupplyChainManagement.ViewModels;

namespace SupplyChainManagement.Controllers
{
    public class ProductionController : Controller
    {
        private ApplicationDbContext _context;

        public ProductionController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();

        }

        // GET: Production
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["userrole"].ToString()) != 4)
                {
                    Response.Redirect("~/Home/Index");
                }

            }
            else
            {
                Response.Redirect("~/Home/Index");

            }

            var POrders = _context.ProductionOrders.ToList();

            return View(POrders);
        }

        public ActionResult addOrder()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult addOrder(ProductionAddOrder Item)
        {
            var order = new ProductionOrder();
            var raw = new ProductionRawMaterial();
            order = Item.productionOrder;
            _context.ProductionOrders.Add(order);
            List<string> listRaw = (Item.productionRawMaterial.PId).Split(',').ToList<string>();
            List<string> quantityRaw = (Item.productionRawMaterial.Quantity).Split(',').ToList<string>();
            for (var x = 0; x < listRaw.Count(); x++)
            {
                raw.OId = Item.productionOrder.Id;
                raw.PId = listRaw[x];
                raw.Quantity = quantityRaw[x];
                _context.ProductionRawMaterial.Add(raw);
                raw = new ProductionRawMaterial();
            }

            _context.SaveChanges();
            return Redirect("Index");
        }

        [System.Web.Mvc.Route("Production/Edit/{id}")]
        public ActionResult Edit(string id)
        {
            var orderInDb = new ProductionAddOrder();

            var order = _context.ProductionOrders.SingleOrDefault(o => o.Id == id);

            var orderrawInDb = _context.ProductionRawMaterial.Where(or => or.OId == id).ToList();
            List<ProductionRawMaterial> list = new List<ProductionRawMaterial>();
            list = orderrawInDb;

            string products = "";
            string quantity = "";

            foreach (var item in list)
            {
                products = products + item.PId + ",";
                quantity = quantity + item.Quantity + ",";
            }

            ProductionRawMaterial prm = new ProductionRawMaterial();
            prm.Quantity = quantity;
            prm.PId = products;
            prm.OId = id;
            prm.SNO = 0;
            orderInDb.productionOrder = order;
            orderInDb.productionRawMaterial = prm;

            return View(orderInDb);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult UpdateOrder(ProductionAddOrder production)
        {
            var orderrawInDb = _context.ProductionRawMaterial.Where(or => or.OId == production.productionOrder.Id).ToList();
            _context.ProductionRawMaterial.RemoveRange(orderrawInDb);
            var order = _context.ProductionOrders.SingleOrDefault(o => o.Id == production.productionOrder.Id);
            order.PId = production.productionOrder.PId;
            order.ExpectedDate = production.productionOrder.ExpectedDate;
            order.StartDate = production.productionOrder.StartDate;
            order.Status = production.productionOrder.Status;
            order.Title = production.productionOrder.Title;
            _context.SaveChanges();
            try
            {
                var raw = new ProductionRawMaterial();
                List<string> listRaw = (production.productionRawMaterial.PId).Split(',').ToList<string>();
                List<string> quantityRaw = (production.productionRawMaterial.Quantity).Split(',').ToList<string>();
                for (var x = 0; x < listRaw.Count(); x++)
                {
                    if (listRaw[x] != null)
                    {
                        raw.OId = production.productionOrder.Id;
                        raw.PId = listRaw[x];
                        raw.Quantity = quantityRaw[x];
                        _context.ProductionRawMaterial.Add(raw);
                        _context.SaveChanges();
                        raw = new ProductionRawMaterial();
                    }
                }
            }
            catch (Exception e)
            {
                return Redirect("~/Production/Index");
            }

            return Redirect("~/Production/Index");

        }

        public ActionResult CompleteOrders()
        {

            var POrders = _context.ProductionOrders.Where(o => o.Status.Equals("Completed")).ToList();
            return View(POrders);
        }

        public ActionResult InProgressOrders()
        {

            var POrders = _context.ProductionOrders.Where(o => o.Status.Equals("InProgress")).ToList();
            return View(POrders);
        }

    }

}