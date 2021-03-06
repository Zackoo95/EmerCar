﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using EmerCar.Models;
using System.Collections;

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
        //[HttpGet]
        public IEnumerable<Cartype> GetCarType()
        {
            var types = _context.Database.SqlQuery<Cartype>("select Car_TypeId,Car_Type from dbo.Car_Type").ToList();
            return types;
        }
    }
}
