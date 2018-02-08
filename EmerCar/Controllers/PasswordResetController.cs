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
    public class PasswordResetController : Controller
    {
        // GET: PasswordReset
        public ActionResult Index()
        {

            return View();
        }
        public string ResetPass(User model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpException(400, "Incorrect input");
            }
            if (model.Email == null)
            {
                throw new HttpException(400, "Email can't be empty");
            }
            string message = "Please Check your mail";
            DB_A33B8A_emercarEntities _context = new DB_A33B8A_emercarEntities();
            User user = _context.Users.SingleOrDefault(m => m.Email == model.Email);
            SendActivationEmail(user);
            return message;
        }
        public ActionResult Reset()
        {
            DB_A33B8A_emercarEntities _context = new DB_A33B8A_emercarEntities();

            ViewBag.Message = "Invalid Activation code.";
            if (RouteData.Values["id"] != null)
            {
                Guid activationCode = new Guid(RouteData.Values["id"].ToString());
                DB_A33B8A_emercarEntities usersEntities = new DB_A33B8A_emercarEntities();
                PasswordReset userActivation = usersEntities.PasswordResets.Where(p => p.Code == activationCode).FirstOrDefault();
                User user = usersEntities.Users.Where(p => p.UserID == userActivation.ID).FirstOrDefault();
                return View();
            }
            else
                throw new HttpException(400, "Incorrect input");
        }
                
        
        public void SendActivationEmail(User user)
        {
            Guid activationCode = Guid.NewGuid();
            DB_A33B8A_emercarEntities usersEntities = new DB_A33B8A_emercarEntities();
            usersEntities.PasswordResets.Add(new PasswordReset
            {
                ID = user.UserID,
                Code = activationCode
            });
            usersEntities.SaveChanges();
            string from = "emercar.gp@gmail.com";
            using (MailMessage mm = new MailMessage(from, user.Email))
            {
                mm.Subject = "EmerCar Password Reset";
                string body = "Hello " + user.UserName + ",";
                body += "<br /><br />Please Click on the following link to complete your operation";
                body += "<br /><a href = '" + string.Format("http://emercar-001-site2.atempurl.com/PasswordReset/Reset/{0}", activationCode) + "'>Please Click Here</a>";
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
        public ActionResult Page()
        {
            return View();
        }
        public string Confirm(ResetPassword forget)
        {
            DB_A33B8A_emercarEntities _context = new DB_A33B8A_emercarEntities();
            User user = _context.Users.Where(p => p.Email == forget.mail ).FirstOrDefault();
            PasswordReset userActivation = _context.PasswordResets.Where(p => p.ID == user.UserID).SingleOrDefault();
            var oldone = forget.oldpass;
            var newone = forget.newpass;
           oldone = Hash.EncodePassword(oldone, user.Code);
            if (oldone == user.Pass)
            {
                _context.PasswordResets.Remove(userActivation);
                user.Pass = Hash.EncodePassword(newone, user.Code);
                var sql = "Update dbo.Users SET Pass = {0} WHERE UserID = {1}";
                _context.Database.ExecuteSqlCommand(sql, newone, user.UserID);
                _context.SaveChanges();
                ViewBag.message = "Password Reset";
                return "Password Reset";
            }
            else
            throw new HttpException(400, "Incorrect Password");
        }
    }
}