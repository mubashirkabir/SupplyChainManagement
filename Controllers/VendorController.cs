using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SupplyChainManagement.Models;
using SupplyChainManagement.ViewModels;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;




namespace SupplyChainManagement.Controllers
{
    public class VendorController : Controller
    {



        private ApplicationDbContext _context;


        public VendorController()
        {

            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();

        }
        // GET: Vendor
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                if (Convert.ToInt32(Session["userrole"].ToString()) != 3)
                {
                    Response.Redirect("~/Home/Index");
                }
            }
            else
            {
                Response.Redirect("~/Home/Index");

            }
            
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

        public ActionResult AddVendor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertVendor(VendorDetail vendorDetail)
        {
            vendorDetail.UserName = Request.Form["uname"];
            _context.VendorDetails.Add(vendorDetail);
            _context.SaveChanges();
            var uD = new UsersDetail();
            uD.UserName = Request.Form["uname"];
            uD.UserPassword = encryptPassword(Request.Form["upass"]);
            uD.FullName = vendorDetail.Name;
            uD.PhoneNumber = vendorDetail.PhoneNumber;
            uD.RoleId = 6;
            _context.UsersDetails.Add(uD);
            _context.SaveChanges();
            return Redirect("~/Vendor/Index");
        }

        [HttpPost]
        public ActionResult UpdateVendor(VendorDetail vendorDetail)
        {
            var vendorInDb = _context.VendorDetails.SingleOrDefault(v => v.VId.Equals(vendorDetail.VId));
            vendorInDb.UserName = vendorDetail.UserName;
            vendorInDb.Address = vendorDetail.Address;
            vendorInDb.Name = vendorDetail.Address;
            vendorInDb.Email = vendorDetail.Address;
            vendorInDb.PhoneNumber = vendorDetail.PhoneNumber;
            _context.SaveChanges();
            var uD = _context.UsersDetails.SingleOrDefault(u => u.UserName.Equals(vendorDetail.UserName));
            uD.UserPassword = encryptPassword(Request.Form["upass"]);
            _context.SaveChanges();
            return Redirect("~/Vendor/Index");
        }

        public static string encryptPassword(string x)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider Md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] dataInByte = System.Text.Encoding.ASCII.GetBytes(x);
            dataInByte = Md5.ComputeHash(dataInByte);
            String md5Hash = System.Text.Encoding.ASCII.GetString(dataInByte);

            return md5Hash;
        }

        [Route("Vendor/EditVendor/{id}")]
        public ActionResult EditVendor(string id)
        {
            var vendorInDb = _context.VendorDetails.SingleOrDefault(v => v.VId == id);
            return View(vendorInDb);
        }

        public ActionResult AddOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddOrder(VendorOrderDetail vendorOrderDetail)
        {
            string hour = DateTime.Now.ToString("HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string min = DateTime.Now.ToString("mm", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string month = DateTime.Now.ToString("MM", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string year = DateTime.Now.ToString("yy", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string OID = "OV" + hour + min + month + year;
            var Order = new VendorOrderDetail();
            Order.Id = OID;
            Order.PId = vendorOrderDetail.PId;
            Order.PQuantity = vendorOrderDetail.PQuantity;
            Order.Price = vendorOrderDetail.Price;
            Order.VendorId = vendorOrderDetail.VendorId;
            Order.Notification = 0;
            _context.VendorOrderDetails.Add(Order);
            _context.SaveChanges();
            return Redirect("~/Vendor/Index");
        }



        public ActionResult WarehouseOrder()
        {
            var orderInDb = _context.WarehouseVendorRequests.ToList();
            return View(orderInDb);
        }


        [Route("Vendor/SendMail/{id}")]
        public ActionResult SendMail(string id)
        {
            var vendorInDb = _context.VendorDetails.SingleOrDefault(v => v.VId == id);
            return View(vendorInDb);
        }

        //[HttpPost]
        //public ActionResult Index(VendorDetail model)
        //{
        //    var em = model.Email;
        //    var name = model.Name;

        //}

        public ActionResult SendMailBtn(VendorDetail v)
        {
            string email = Request.Form["emailId"];
            string message = Request.Form["message"];
            Execute(email, message).Wait(1000);
            //How to add notify.js or sweetAlert.js here or any other notification

            return Redirect("~/Vendor/Index");


        }


        //[HttpGet]
        //public ActionResult SendData(VendorDetail model)
        //{
        //    var name = model.Name;
        //    var email = model.Email;
        //    return View();
        //}


        static async Task Execute(String email, String Message)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("061.umair@gmail.com", "vendor employee");

            var to = new EmailAddress(email); // need email, name from model.
            var subject = "From Office Interior";
            var plainTextContent = "content";
            var htmlContent = Message; //msg here from SendMail View.
            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                plainTextContent,
                htmlContent);

            var response = await client.SendEmailAsync(msg);
            //swal({
            //title: "Good job!",
            //    text: "You clicked the button!",
            //    icon: "success",
            //    button: "Aww yiss!",
            //    });
        }




    }
}