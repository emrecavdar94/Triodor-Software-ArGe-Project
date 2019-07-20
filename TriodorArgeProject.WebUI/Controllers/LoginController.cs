using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TriodorArgeProject.Entities.EntityClasses;
using TriodorArgeProject.Service.Managers;

namespace TriodorArgeProject.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private UserManager userManager = new UserManager();
        private PersonnelManager personnelManager = new PersonnelManager();
        private  Users user;
        private Personnels personnel;
        public ActionResult Index()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(string username,string password)
        {
            if (username==null || password==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personnel = personnelManager.Find(x=>x.Users.Username==username&&x.Users.Password==password);
            if (personnel==null)
            {
                ViewBag.LOGINMESSAGE = "Kullanıcı bulunamadı.";
                return View();
            }
            else
            {
                Session["user"] = personnel;
                return RedirectToAction("Dashboard", "Dash");
            }
        }
    }
}