using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Web.Routing;
using EmerCar.Models;
using System.Data.SqlClient;
using System.Web.Http;

namespace EmerCar.Controllers.API
{
    public class ResetController : ApiController
    {

        public string ResetPass(string email)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException(400, "Incorrect input");
            }
            if (email == null)
            {
                throw new HttpException(400, "Email can't be empty");
            }
            //string message = "Please Check your mail";
            DB_A33B8A_emercarEntities _context = new DB_A33B8A_emercarEntities();
            User user = _context.Users.SingleOrDefault(m => m.Email == email);
            EmerCar.Models.SendActivation.SendResetEmail(user);
            throw new HttpResponseException(HttpStatusCode.OK);
        }
    }
}
