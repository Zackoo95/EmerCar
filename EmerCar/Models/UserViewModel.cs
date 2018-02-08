using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmerCar.Models
{
    public class UserViewModel
    {
        
        public IEnumerable<Car_Model> CarModel { get; set; }
        public User user { get; set; }
        public Number number { get; set; }
        public IEnumerable<Car_Type> CarType { get; set; }
        public Car_Type type { get; set; }
        public Park park { get; set; }
        public Accident accident { get; set; }
    }
}