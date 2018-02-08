using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmerCar.Models
{
    public class ResetPassword
    {
        public string mail { get; set; }
        public string oldpass { get; set; }
        public string newpass { get; set; }

    }
}