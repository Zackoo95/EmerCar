using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmerCar.Models;

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
        public IEnumerable<CarModel> CarModels (int id)
        {
            string sql = "select Car_ModelId,Car_Model from dbo.Car_Model where Car_TypeId = {0}";
            IEnumerable<CarModel> carModel = _context.Database.SqlQuery<CarModel>(sql,id).ToList();
            return carModel;

        }
    }
}
