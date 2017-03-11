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

        public ActionResult Groups()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.Groups = ApplicationDbContext.Groups.ToList();

            return View(user);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UserProfile()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var userImageId = user.PhotoId;
            var userPhoto = ApplicationDbContext.Images.Where(i => i.ImageId == userImageId).SingleOrDefault();
            user.Photo = userPhoto;
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
                using (ApplicationDbContext)
                {
                    Image photo = new Image();
                    photo.Path = "~/Images/" + fileName;
                    photo.Name = fileName;

                    ApplicationDbContext.Images.Add(photo);
                    ApplicationDbContext.SaveChanges();

                    var image = ApplicationDbContext.Images.Where(i => i.Name == fileName).FirstOrDefault();
                    user.PhotoId = image.ImageId;
                    user.Photo = image;
                    var result = await UserManager.UpdateAsync(user);
                }
                ViewBag.Message = "Фото загружено";
            }
            else
            {
                return View("WrongFileExtensionError");
            }
            
            return RedirectToAction("UserProfile");
        }
    }
}