using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SupplyChainManagement.Models;

namespace SupplyChainManagement.Controllers.Api
{
    public class VendorsController : ApiController
    {
        private ApplicationDbContext _context;

        public VendorsController()
        {
            _context = new ApplicationDbContext();
        }

        [Route("Api/Vendors/{id}")]
        [HttpDelete]
        //PUT /api/Vendors/1
        public void ProductionsDelete(string id)
        {
            var vendor = _context.VendorDetails.SingleOrDefault(v => v.VId == id);
            _context.VendorDetails.Remove(vendor);
            _context.SaveChanges();


        }
    }
}
