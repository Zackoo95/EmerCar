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

namespace EmerCar.Controllers
{
    public class RegController : ApiController
    {
        DB_A33B8A_emercarEntities _context = new DB_A33B8A_emercarEntities();
        //Post /Registration//Rege
        [System.Web.Http.HttpPost]
        public RegModel Rege(RegModel model)
        {
            if (model.name == null)
            {
                throw new HttpException(400, "UserName can't be empty");
            }
            else if (model.email == null)
            {
                throw new HttpException(400, "Email can't be empty");
            }
            else if (model.password == null)
            {
                throw new HttpException(400, "Password can't be empty");

            }
            else if (model.mobile_no == null)
            {
                throw new HttpException(400, "Number can't be empty");

            }
            else if (model.emergency_no1 == null)
            {
                throw new HttpException(400, "Emergancy Number can't be empty");
            }
            else if (model.car_no == null)
            {
                throw new HttpException(400, "CarPlate can't be empty");
            }
            // To convert the user uploaded Photo as Byte Array before save to DB
          

            var keyNew = Hash.GeneratePassword(10);
            model.Code = keyNew;
            var password = Hash.EncodePassword(model.password, keyNew);
            model.password = password;
            //model.profile_photo = imageData;
            User user = new User();
            Number number = new Number();
            string message = string.Empty;
            string email = _context.Database.SqlQuery<string>("Select Email from dbo.Users where Email=@mail", new SqlParameter("@mail", user.Email)).ToString();
            string plate = _context.Database.SqlQuery<string>("Select Car_ID from dbo.Users where Car_ID=@Car", new SqlParameter("@car", user.Car_ID)).ToString();
            if (email == user.Email)
            {
                throw new HttpException(400, "Email is already Used");
            }
            else if (plate == user.Car_ID)
            {
                throw new HttpException(400, "CarPlate is already Used");
            }
            else
            {
                user.UserName = model.name;
                user.Email = model.email;
                user.Pass = model.password;
                user.Car_ID = model.car_no;
                user.Car_ModelId = model.car_model_id;
                user.User_Number = model.mobile_no;
                user.FireBaseToken = model.firebase_token;
                user.Code = model.Code;
                _context.Users.Add(user);
                _context.SaveChanges();
                var UserInDb = _context.Users.SingleOrDefault(c => c.Email == model.email);
                number.User_ID = UserInDb.UserID;
                number.Number1 = model.emergency_no1;
                number.Number2 = model.emergency_no2;
                number.Number3 = model.emergency_no3;
                number.Number4 = model.emergency_no4;
                number.Number5 = model.emergency_no5;

                _context.Numbers.Add(number);
                _context.SaveChanges();
                message = "Registration successful.\\nUser Id: " + user.UserID.ToString();
                EmerCar.Models.SendActivation.SendActivationEmail(user);
                model.password = "";
                return model;
            }
        }

      
    }
}
