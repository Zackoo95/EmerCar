using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmerCar.Controllers.API
{
    public class CarModelsController : ApiController
    {
        private DB_A33B8A_emercarEntities _context;

        public CarModelsController()
        {
            _context = new DB_A33B8A_emercarEntities();
        }

        //GET /api/carmodels
        [HttpGet]
        public IEnumerable<Car_Model> CarModels (int id)
        {
            
            IEnumerable<Car_Model> carModel = _context.Car_Model.Where(c => c.Car_TypeId == id).ToList();
            return carModel;

        }
    }
}
