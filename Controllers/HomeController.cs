using Community3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
            var friend = ApplicationDbContext.Users.Where(u => u.Id == "7ee4f15b-a7bb-4196-aff2-96a640c87d46").FirstOrDefault();

            var user = UserManager.FindById(User.Identity.GetUserId());
            user.Friends.Add(friend);
            return View(user);
        }

        public ActionResult ShowUserProfile(string id)
        {

            ViewBag.Access = "Denied";

            var user = ApplicationDbContext.Users.Where(u => u.Id == id).FirstOrDefault();
            return View("UserProfile", user);
        }

        public ActionResult EditProfile()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(Gender? gender, DateTime? birthday, string location = "", string description = "")
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                user.Gender = gender;
                user.Location = location;
                user.Birthday = birthday;
                user.Description = description;
                UserManager.Update(user);
                context.SaveChanges();
            }

            return RedirectToAction("UserProfile");
        }

        public ActionResult ChangePhoto()
        {

            var user = UserManager.FindById(User.Identity.GetUserId());
            return View(user);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ChangePhoto(HttpPostedFileBase file)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var fileName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);
            fileName += extension;

            var extensions = new List<string>() { ".jpg", ".gif", ".png" };

            if (extensions.Contains(extension))
            {
                file.SaveAs(Server.MapPath("~/Images/" + fileName));
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    Image photo = new Image();
                    photo.Path = "~/Images/" + fileName;
                    photo.Name = fileName;
                    context.Images.Add(photo);
                    context.SaveChanges();

                    var image = context.Images.Where(i => i.Name == fileName).FirstOrDefault();
                    user.Photo = image;
                    var result = await UserManager.UpdateAsync(user);
                }
                ViewBag.Message = "Фото загружено";
            }
            else
            {
                ViewBag.Message = "Неправильный тип файла";
            }
            
            return RedirectToAction("UserProfile");
        }
    }
}