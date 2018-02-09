using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmerCar.Controllers.API
{
    public class CarsController : ApiController
    {
        private DB_A33B8A_emercarEntities _context;
        public CarsController()
        {
            _context = new DB_A33B8A_emercarEntities();
        }

        //GET /api/cars
        [HttpGet]
        public IHttpActionResult CarType()
        {
            var list = _context.Car_Type;
            return Json(list); 
        }
    }
}
