using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmerCar.Models
{
    public class LoginModel
    {
        public string email { get; set; }
        public string name { get; set; }
  
        public string mobile_no { get; set; }

        public int id { get; set; }
        public bool is_verified { get; set; }
        public Base64FormattingOptions profile_photo { get; set; }
        public string emergency_no1 { get; set; }
        public string emergency_no2 { get; set; }
        public string emergency_no3 { get; set; }
        public string emergency_no4 { get; set; }
        public string emergency_no5 { get; set; }
        public int car_model_id { get; set; }
        public string car_no { get; set; }
        public int? car_make_id { get; set; }
        public string firebase_token { get; set; }
    }
}