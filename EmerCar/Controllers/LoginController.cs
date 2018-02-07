using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmerCar.Controllers
{
    public class LoginController : Controller
    {
        private DB_A33B8A_emercarEntities _context;
        public LoginController()
        {
            _context = new DB_A33B8A_emercarEntities();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Reg()
        {
            return RedirectToAction("Register", "Login");
        }
        public ActionResult Register()
        {

            return View();
        }
        public ActionResult Create(User user)
        {
            return RedirectToAction("Regestration", "API");
        }
        public ActionResult SignIn()
        {

            return View();
        }
        public ActionResult Enter(User user)
        {
            //SignIn API

            return RedirectToAction("Index", "Home");
        }
    }
}