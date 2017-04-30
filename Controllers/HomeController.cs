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
using Community3.Helpers;

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

        public ActionResult Dialogues()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            return View(currentUser);
        }

        public ActionResult ChatRoom(string id)
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            var interlocutor = UserManager.FindById(id);
            ViewBag.currentUser = currentUser;
            ViewBag.interlocutor = interlocutor;
            foreach (var chat in ApplicationDbContext.ChatRooms)
            {
                if (chat.AppUsers.Contains(currentUser) && chat.AppUsers.Contains(interlocutor))
                {
                    return View(chat);
                }
            }
            var chatRoom = new ChatRoom();
            using (ApplicationDbContext)
            {
                chatRoom.AppUsers.Add(currentUser);
                chatRoom.AppUsers.Add(interlocutor);
                ApplicationDbContext.ChatRooms.Add(chatRoom);
                ApplicationDbContext.SaveChanges();
            }

            return View(chatRoom);
        }

        public ActionResult ShowNewMessage()
        {
            var userId = Request.Form.GetValues("userId")[0];
            var message = Request.Form.GetValues("message")[0];
            var chatId = Convert.ToInt32(Request.Form.GetValues("chatId")[0]);

            var sender = UserManager.FindById(userId);
            var chat = ApplicationDbContext.ChatRooms
                .Where(_ => _.ChatRoomId == chatId).FirstOrDefault();

            var recipient = chat.AppUsers.Where(_ => _.Id != userId).FirstOrDefault();

            var newMessage = new Message();
            newMessage.CreationTime = DateTime.Now;
            newMessage.Text = message;
            newMessage.Sender = sender;
            newMessage.SenderId = sender.Id;
            newMessage.Recipient = recipient;
            newMessage.RecipientId = recipient.Id;

            return PartialView("_Message", newMessage);
        }

        public ActionResult UserProfile(string id)
        {
            var user = UserManager.FindById(id);
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            if (user != currentUser)
            {
                ViewBag.Access = "Denied";
            }
            return View(user);
        }

        //public ActionResult ShowUserProfile(string id)
        //{

        //    ViewBag.Access = "Denied";
        //    var user = ApplicationDbContext.Users.Where(u => u.Id == id).FirstOrDefault();
        //    return View("UserProfile", user);
        //}

        public ActionResult EditProfile()
        {
            var model = new UserProfileEditModel();
            model.Gender = UserManager.FindById(User.Identity.GetUserId()).Gender;
            return View(model);
        }

        //[HttpPost]
        //public ActionResult EditProfile(Gender? gender, DateTime? birthday, string location = "", string description = "")
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId());
        //    using (ApplicationDbContext context = new ApplicationDbContext())
        //    {
        //        user.Gender = gender;
        //        user.Location = location;
        //        user.Birthday = birthday;
        //        user.Description = description;
        //        UserManager.Update(user);
        //        context.SaveChanges();
        //    }

        //    return RedirectToAction("UserProfile", new { id = user.Id });
        //}

        [HttpPost]
        public ActionResult EditProfile(UserProfileEditModel model)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                user.Gender = model.Gender;
                user.Location = model.Location;
                user.Birthday = model.Birthday;
                user.Description = model.Description;
                UserManager.Update(user);
                context.SaveChanges();
            }

            return RedirectToAction("UserProfile", new { id = user.Id });
        }

        public ActionResult ChangePhoto()
        {

            var user = UserManager.FindById(User.Identity.GetUserId());
            return View(user);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ChangePhoto(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return View("WrongFileExtensionError");
            }

            var user = UserManager.FindById(User.Identity.GetUserId());
            var oldPhotoId = user.PhotoId;
            var helper = new ImageHelper();
            var image = helper.GetImageFromFile(file);

            if (image != null)
            {
                using (ApplicationDbContext)
                {
                    user.PhotoId = image.ImageId;
                    user.Photo = image;
                    user.Images.Add(image);
                    ApplicationDbContext.SaveChanges();
                    var result = await UserManager.UpdateAsync(user);
                    if (oldPhotoId != null)
                    {
                        var imageHelper = new ImageHelper();
                        imageHelper.DeleteImageById(oldPhotoId.Value);
                    }
                }
            }
            else


            {
                return View("WrongFileExtensionError");
            }

            return RedirectToAction("UserProfile", new { id = user.Id });

        }
    }
}