using Community3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Community3.Controllers
{
    public class HomeController : Controller
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<AppUser> UserManager { get; set; }

        public HomeController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<AppUser>(new UserStore<AppUser>(this.ApplicationDbContext));
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UserProfile()
        {

            var user = UserManager.FindById(User.Identity.GetUserId());
            return View(user);
        }
    }
}