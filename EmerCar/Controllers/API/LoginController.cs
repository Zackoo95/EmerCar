using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmerCar.Models;
using System.Web;

namespace EmerCar.Controllers.API
{
    public class LoginController : ApiController
    {
        DB_A33B8A_emercarEntities _context = new DB_A33B8A_emercarEntities();

        public LoginModel Log (Login model)
        {
            string emailCheck = model.email;

            var user = _context.Users.SingleOrDefault(u => u.Email == emailCheck);
            var number = _context.Numbers.SingleOrDefault(n => n.User_ID == user.UserID);
            var active = _context.UserActivations.SingleOrDefault(u => u.User_ID == user.UserID);

            if (model.email == null)
            {
                throw new HttpException(400, "Email can't be empty");
            }
            else if (model.password == null)
            {
                throw new HttpException(400, "Password can't be empty");
            }
            
            else if (user == null)
            {
                if (active != null)
                {
                    throw new HttpException(400, "Please activate your account first");
                }

                else
                { 
                throw new HttpException(400, "User isn't found");
                }
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

            }

            else
            {
                throw new HttpException(400, "Password doesn't match this account");

            }

            

            LoginModel logged = new LoginModel();
            logged.name = user.UserName;
            logged.id = user.UserID;
            logged.is_verified = user.IsVerified;
            logged.firebase_token = user.FireBaseToken;
            logged.email = user.Email;
            logged.mobile_no = user.User_Number;
            //user.ProfilePicture = logged.profile_photo;
            logged.emergency_no1 = number.Number1;
            logged.emergency_no2 = number.Number2;
            logged.emergency_no3 = number.Number3;
            logged.emergency_no4 = number.Number4;
            logged.emergency_no5 = number.Number5;
            logged.car_model_id = (int)user.Car_ModelId;
           // logged.car_make_id = user.Car_Model.Car_TypeId;
            logged.car_no = user.Car_ID;

            return logged;

        }

    }
}
