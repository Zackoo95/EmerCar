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
    public class LoginController : ApiController
    {
        DB_A33B8A_emercarEntities _context = new DB_A33B8A_emercarEntities();
        [System.Web.Http.HttpPost]
        public LoginModel Log([FromBody] Login model)
        {

            if (model == null)
            {
                throw new HttpException(400, "Incorrect input");
            }
            if (model.email == null)
            {
                throw new HttpException(400, "Email can't be empty");
            }
            else if (model.password == null)
            {
                throw new HttpException(400, "Password can't be empty");
            }


            User user = _context.Users.SingleOrDefault(u => u.Email == model.email);
            Number number = _context.Numbers.SingleOrDefault(n => n.User_ID == user.UserID);
            UserActivation active = _context.UserActivations.SingleOrDefault(a => a.User_ID == user.UserID);

            
            if (active != null)
            {
                throw new HttpException(400, "Please activate your account first");
               
            }



            //Hashing the password
            var keyNew = user.Code;
            var password = Hash.EncodePassword(model.password, keyNew);
            
            if (user.Pass == password)
            {

                string token = model.firebase_token;
                var sql = "Update dbo.Users SET FireBaseToken = {0} WHERE UserID = {1}";
                _context.Database.ExecuteSqlCommand(sql, token, user.UserID);
                _context.SaveChanges();


                var user1 = _context.Users.SingleOrDefault(u => u.Email == model.email);


                LoginModel logged = new LoginModel();
                logged.name = user1.UserName;
                logged.id = user1.UserID;
                logged.is_verified = user1.IsVerified;
                logged.firebase_token = model.firebase_token;
                logged.email = user1.Email;
                logged.mobile_no = user1.User_Number;
                //user.ProfilePicture = logged.profile_photo;
                logged.emergency_no1 = number.Number1;
                logged.emergency_no2 = number.Number2;
                logged.emergency_no3 = number.Number3;
                logged.emergency_no4 = number.Number4;
                logged.emergency_no5 = number.Number5;
                logged.car_model_id = (int)user1.Car_ModelId;
                // logged.car_make_id = user.Car_Model.Car_TypeId;
                logged.car_no = user1.Car_ID;

                return logged;

            }

            else
            {
                throw new HttpException(400, "Password doesn't match this account");

            }


        }

    }
}
