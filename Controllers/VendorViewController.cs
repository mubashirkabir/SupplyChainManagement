using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SupplyChainManagement.Models;

namespace SupplyChainManagement.Controllers
{
    public class VendorViewController : Controller
    {

        private ApplicationDbContext _context;


        public VendorViewController()
        {

            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();

        }

        // GET: VendorView
        [Route("VendorView/Index/{id}")]
        public ActionResult Index(string id)
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["userrole"].ToString()) != 6)
                {
                    Response.Redirect("~/Home/Index");
                }

            }
            else
            {
                Response.Redirect("~/Home/Index");

            }
            var vendorInDb = _context.VendorDetails.SingleOrDefault(v => v.UserName.Equals(id));
            List<VendorOrderDetail> vendorOrderInDb = _context.VendorOrderDetails.Where(o => o.VendorId.Equals(vendorInDb.VId)).ToList();
            return View(vendorOrderInDb);
        }

        [Route("VendorView/MyDetail/{id}")]
        public ActionResult MyDetail(string id)
        {
            var vendorInDb = _context.VendorDetails.SingleOrDefault(v => v.UserName.Equals(id));
            return View(vendorInDb);
        }

    }
}