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

namespace EmerCar.Models
{
    public class SendActivation
    {
        public static void SendActivationEmail(User user)
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

        public static void SendResetEmail(User user)
        {
            Guid activationCode = Guid.NewGuid();
            DB_A33B8A_emercarEntities usersEntities = new DB_A33B8A_emercarEntities();
            PasswordReset userindb = new PasswordReset();
            userindb = usersEntities.PasswordResets.SingleOrDefault(c => c.ID == user.UserID);
            if (userindb.ID != 0)
            {
                usersEntities.PasswordResets.Remove(userindb);
                usersEntities.SaveChanges();
            }
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
    }
}