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
    public class GroupController : Controller
    {

        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<AppUser> UserManager { get; set; }

        public GroupController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<AppUser>(new UserStore<AppUser>(this.ApplicationDbContext));
        }

        // GET: Group
        public ActionResult ShowGroupPage(int id)
        {
            var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
            return View(group);
        }

        public ActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroup(GroupCreationModel model)
        {
            var grp = new Group();
            using (ApplicationDbContext)
            {
                var group = new Group();
                group.Name = model.Name;
                group.Description = model.Description;
                group.OwnerId = UserManager.FindById(User.Identity.GetUserId()).Id;
                group.Owner = UserManager.FindById(User.Identity.GetUserId());
                group.CreationDate = DateTime.Now;
                ApplicationDbContext.Groups.Add(group);
                ApplicationDbContext.SaveChanges();
                grp = group;
            }

            return View("ShowGroupPage", grp);
        }
    }
}