using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using SupplyChainManagement.Models;
using SupplyChainManagement.ViewModels;

namespace SupplyChainManagement.Controllers.Api
{
    public class ProductionsController : ApiController
    {
        private ApplicationDbContext _context;

        public ProductionsController()
        {
            _context = new ApplicationDbContext();
        }

        [Route("Api/Productions/{id}")]
        [HttpDelete]
        //PUT /api/Productions/1
        public void ProductionsDelete(string id)
        {
            var productionOrder = _context.ProductionOrders.SingleOrDefault(o => o.Id == id);
            List<ProductionRawMaterial> orderraw = _context.ProductionRawMaterial.ToList();
            var orderrawInDb = _context.ProductionRawMaterial.Where(or => or.OId == id).ToList();

            if (productionOrder == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _context.ProductionRawMaterial.RemoveRange(orderrawInDb);
            _context.ProductionOrders.Remove(productionOrder);
            _context.SaveChanges();


        }
    }
}