using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Web.Routing;
using EmerCar.Models;
using System.Data.SqlClient;
using System.Web.Http;

namespace EmerCar.Controllers
{
    public class RegistrationController : Controller
    {

        DB_A33B8A_emercarEntities _context = new DB_A33B8A_emercarEntities();

        // GET: Registration



        public ActionResult Index()
        {
            var Carmodel = _context.Car_Model.ToList();
            var viewmodel = new RegModel();
            
            return View(viewmodel);
        }

        //Regestration
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
                throw new HttpException(400,"Email can't be empty");
            }
            else if (model.password == null)
            {
                throw new HttpException(400,"Password can't be empty");

            }
            else if (model.mobile_no == null)
            {
                throw new HttpException(400,"Number can't be empty");

            }
            else if (model.emergency_no1 == null)
            {
                throw new HttpException(400,"Emergancy Number can't be empty");
            }
            else if (model.car_no == null)
            {
                throw new HttpException(400,"CarPlate can't be empty");
            }
            // To convert the user uploaded Photo as Byte Array before save to DB
            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["ProfilePicture"];

                using (var binary = new System.IO.BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

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
                throw new HttpException(400,"Email is already Used");
            }
            else if (plate == user.Car_ID)
            {
                throw new HttpException(400,"CarPlate is already Used");
            }
            else
            {
                message = "Registration successful.\\nUser Id: " + user.UserID.ToString();
                user.UserName = model.name;
                user.Email = model.email;
                user.Pass = model.password;
                user.Car_ID = model.car_no;
                user.Car_ModelId = model.car_model_id;
                user.User_Number = model.mobile_no;
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
                SendActivationEmail(user);
                model.password = "";
                return model;
            }
        }


        //Checking the Avtivation Code    

        public ActionResult Activation()
        {

            ViewBag.Message = "Invalid Activation code.";
            if (RouteData.Values["id"] != null)
            {
                Guid activationCode = new Guid(RouteData.Values["id"].ToString());
                DB_A33B8A_emercarEntities usersEntities = new DB_A33B8A_emercarEntities();
                UserActivation userActivation = usersEntities.UserActivations.Where(p => p.ActivationCode == activationCode).FirstOrDefault();
                User user = _context.Users.Find(userActivation.User_ID);
                Number number = _context.Numbers.Find(userActivation.User_ID);
                if (userActivation != null)
                {
                    string sql = "Update dbo.Users set IsVerified = 1 where UserID = {0}";
                    _context.Database.ExecuteSqlCommand(sql,user.UserID);
                    usersEntities.UserActivations.Remove(userActivation);
                    usersEntities.SaveChanges();
                    ViewBag.Message = "Activation successful.";
                }

            }
            return View();
        }
        //Sending Confirmation mail


        public void SendActivationEmail(User user)
        {
            Guid activationCode = Guid.NewGuid();
            DB_A33B8A_emercarEntities usersEntities = new DB_A33B8A_emercarEntities();
            usersEntities.UserActivations.Add(new UserActivation
            {
                User_ID = user.UserID,
                ActivationCode = activationCode
            });
            usersEntities.SaveChanges();
            string from = "emercar.gp@gmail.com";
            using (MailMessage mm = new MailMessage(from, user.Email))
            {
                mm.Subject = "EmerCar Account Activation";
                string body = "Hello " + user.UserName + ",";
                body += "<br /><br />Please click the following link to activate your account";
                body += "<br /><a href = '" + string.Format("http://emercar-001-site2.atempurl.com/Registration/Activation/{0}", activationCode) + "'>Please Click Here</a>";
                body += "<br /><br />Thanks";
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                string pass = "emercar123";
                NetworkCredential NetworkCred = new NetworkCredential(from, pass);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }


        public ActionResult wait()
        {
            return View();
        }

    }


}
