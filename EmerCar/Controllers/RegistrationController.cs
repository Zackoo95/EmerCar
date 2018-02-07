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
            var viewmodel = new UserViewModel
            {
                CarModel = Carmodel
            };
            return View(viewmodel);
        }

        //Regestration
        //Post /Registration//Rege
        public string Rege([Bind(Exclude = "ProfilePicture")]UserViewModel model)
        {
            if (model.user.UserName == null)
            {
                throw new HttpException(400, "UserName can't be empty");
            }
            else if (model.user.Email == null)
            {
                throw new HttpException(400,"Email can't be empty");
            }
            else if (model.user.Pass == null)
            {
                throw new HttpException(400,"Password can't be empty");

            }
            else if (model.user.User_Number == 0)
            {
                throw new HttpException(400,"Number can't be empty");

            }
            else if (model.number.Number1 == 0)
            {
                throw new HttpException(400,"Emergancy Number can't be empty");
            }
            else if (model.user.Car_ID == null)
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
            model.user.Code = keyNew;
            var password = Hash.EncodePassword(model.user.Pass, keyNew);
            model.user.Pass = password;
            model.user.ProfilePicture = imageData;
            User user = model.user;
            Number number = model.number;
            number.User_ID = user.UserID;
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
                _context.Users.Add(user);
                _context.Numbers.Add(number);
                _context.SaveChanges();
                SendActivationEmail(user);
                return message;


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
                    usersEntities.UserActivations.Remove(userActivation);
                    //usersEntities.Numbers.Remove(number);
                    //usersEntities.Users.Remove(user);
                    usersEntities.SaveChanges();
                    //user.IsVerified = true;
                    //usersEntities.Users.Add(user);
                    //usersEntities.Numbers.Add(number);
                    //usersEntities.SaveChanges();
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
