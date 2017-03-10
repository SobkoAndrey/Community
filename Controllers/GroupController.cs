using Community3.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Community3.Controllers
{
    public class GroupController : Controller
    {

        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<AppUser> UserManager { get; set; }

        // GET: Group
        public ActionResult ShowGroupPage()
        {
            return View();
        }
    }
}