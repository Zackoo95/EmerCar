using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmerCar.Models
{
    public class Login
    {
        public string email { get; set; }
        
        public string password { get; set; }
        
        public string firebase_token { get; set; }
    }
}