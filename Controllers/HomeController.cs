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
using System.Net;

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

        [Authorize(Roles = "user, blocked")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            if (UserManager.IsInRole(userId, "blocked"))
            {
                ViewBag.blocked = "blocked";
            }
            //ViewBag.currentUser = user;
            return View();
        }

        [Authorize(Roles = "user")]
        public ActionResult News()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            var news = new List<Post>();
            var ownerGroupsList = ApplicationDbContext.Groups.Where(_ => _.Owner.Id == currentUser.Id).ToList();
            news = news.Concat(currentUser.Posts).ToList();
            foreach (var group in ownerGroupsList)
            {
                news = news.Concat(group.Posts).ToList();
            }

            foreach (var group in currentUser.Groups)
            {
                news = news.Concat(group.Posts).ToList();
            }

            foreach (var friend in currentUser.Friends)
            {
                news = news.Concat(friend.Posts).ToList();
            }
            news.Sort((x, y) => DateTime.Compare(x.CreationDate, y.CreationDate));
            news.Reverse();

            ViewBag.currentUser = currentUser;

            return View(news);
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        public ActionResult Photos()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            return View(currentUser);
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        public ActionResult ShowPhotos(string id)
        {
            var user = UserManager.FindById(id);
            return View(user);
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        public ActionResult AddImageToAlbum()
        {
            return View();
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public ActionResult AddImageToAlbum(HttpPostedFileBase file)
        {
            if (file != null)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var helper = new ImageHelper();
                var image = helper.GetImageFromFile(file);

                if (image != null)
                {
                    using (ApplicationDbContext)
                    {
                        user.Images.Add(image);
                        ApplicationDbContext.SaveChanges();
                    }

                    return RedirectToAction("Photos");
                }

                return View("Error");
            }

            return View("Error");
        }

        [Authorize(Roles = "user")]
        public ActionResult RemoveImage(string userId, int imageId)
        {
            var user = UserManager.FindById(userId);

            var imageHelper = new ImageHelper();
            imageHelper.DeleteImageById(imageId);

            return RedirectToAction("Photos", user);
        }

        [Authorize(Roles = "user")]
        public ActionResult Music()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            return View(currentUser);
        }

        [Authorize(Roles = "user")]
        public ActionResult ShowMusic(string id)
        {
            var user = UserManager.FindById(id);
            return View(user);
        }

        [Authorize(Roles = "user")]
        public ActionResult MusicWebApi()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return View(user);
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        public ActionResult AddAudioToAlbum()
        {
            return View();
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public ActionResult AddAudioToAlbum(HttpPostedFileBase file)
        {
            if (file != null)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var helper = new AudioHelper();
                var audio = helper.GetAudioFromFile(file);

                if (audio != null)
                {
                    using (ApplicationDbContext)
                    {
                        user.Audios.Add(audio);
                        ApplicationDbContext.SaveChanges();
                    }

                    return RedirectToAction("Music");
                }

                return View("Error");
            }

            return View("Error");
        }

        [Authorize(Roles = "user")]
        public ActionResult MusicSearchAjax()
        {
            var userId = Request.Form.GetValues("userId")[0];
            var searchString = Request.Form.GetValues("searchString")[0];
            var user = UserManager.FindById(userId);

            if (searchString.Length == 0)
            {
                return PartialView("_Audio", user.Audios);
            }

            var musicList = new List<Audio>();

            foreach (var audio in user.Audios)
            {
                if (audio.Label.ToLower().Contains(searchString.ToLower()))
                {
                    musicList.Add(audio);
                }
            }

            return PartialView("_Audio", musicList);
        }

        [Authorize(Roles = "user")]
        public ActionResult Groups()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.Groups = ApplicationDbContext.Groups.ToList();
            return View(user);
        }

        [Authorize(Roles = "user")]
        public ActionResult Friends()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            return View(currentUser);
        }

        public ActionResult UserSearchAjax()
        {
            var userName = Request.Form.GetValues("userName")[0];

            var users = UserManager.Users.Where(_ => (_.Name + _.Surname).Contains(userName)).ToList();
            if (users.Count == 0 || userName.Length == 0)
            {
                return PartialView("_EmptyList");
            }

            return PartialView("_Users", users);
        }


        [Authorize(Roles = "user")]
        public ActionResult Dialogues()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            return View(currentUser);
        }

        [Authorize(Roles = "user")]
        public void OfferFriendship()
        {
            using (ApplicationDbContext)
            {
                var userId = Request.Form.GetValues("userId")[0];
                var user = UserManager.FindById(userId);
                var currentUser = UserManager.FindById(User.Identity.GetUserId());
                user.Candidates.Add(currentUser);
                ApplicationDbContext.SaveChanges();
            }
        }

        [Authorize(Roles = "user")]
        public void ConfirmFriendshipAjax()
        {
            using (ApplicationDbContext)
            {
                var userId = Request.Form.GetValues("userId")[0];
                var user = UserManager.FindById(userId);
                var currentUser = UserManager.FindById(User.Identity.GetUserId());
                currentUser.Friends.Add(user);
                user.Friends.Add(currentUser);
                currentUser.Candidates.Remove(user);
                ApplicationDbContext.SaveChanges();
            }
        }

        [Authorize(Roles = "user")]
        public ActionResult ConfirmFriendship(string id)
        {
            using (ApplicationDbContext)
            {
                var user = UserManager.FindById(id);
                var currentUser = UserManager.FindById(User.Identity.GetUserId());
                currentUser.Friends.Add(user);
                user.Friends.Add(currentUser);
                currentUser.Candidates.Remove(user);
                ApplicationDbContext.SaveChanges();
            }

            return RedirectToAction("Friends", new { id = User.Identity.GetUserId() });
        }

        [Authorize(Roles = "user")]
        public ActionResult RefuseFriendship(string id)
        {
            using (ApplicationDbContext)
            {
                var user = UserManager.FindById(id);
                var currentUser = UserManager.FindById(User.Identity.GetUserId());
                currentUser.Candidates.Remove(user);
                ApplicationDbContext.SaveChanges();
            }

            return RedirectToAction("Friends", new { id = User.Identity.GetUserId() });
        }

        [Authorize(Roles = "user")]
        public void FinishFriendship()
        {
            using (var context = new ApplicationDbContext())
            {
                var userId = Request.Form.GetValues("userId")[0];
                var user = UserManager.FindById(userId);
                var currentUser = UserManager.FindById(User.Identity.GetUserId());
                currentUser.Friends.Remove(user);
                user.Friends.Remove(currentUser);
                UserManager.Update(user);
                UserManager.Update(currentUser);
                context.SaveChanges();
            }
        }

        [Authorize(Roles = "user")]
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

        [Authorize(Roles = "user")]
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

        [Authorize(Roles = "user")]
        public ActionResult UserProfile(string id)
        {
            var user = UserManager.FindById(id);
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            if (user != currentUser)
            {
                ViewBag.Access = "Denied";
            }

            if (UserManager.IsInRole(user.Id, "blocked"))
            {
                ViewBag.blocked = "blocked";
            }

            ViewBag.currentUser = currentUser;
            return View(user);
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        public ActionResult EditProfile()
        {
            var model = new UserProfileEditModel();
            model.Gender = UserManager.FindById(User.Identity.GetUserId()).Gender;
            return View(model);
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserProfileEditModel model)
        {
            if (ModelState.IsValid)
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
            else
            {
                return View(model);
            }
        }

        [Authorize(Roles = "user")]
        public ActionResult ChangePhoto()
        {

            var user = UserManager.FindById(User.Identity.GetUserId());
            return View(user);
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        public ActionResult AddNews()
        {
            return View();
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public ActionResult AddNews(Post post)
        {
            using (ApplicationDbContext)
            {
                post.CreationDate = DateTime.Now;
                ApplicationDbContext.Posts.Add(post);
                ApplicationDbContext.SaveChanges();
            }
            ViewBag.currentUserId = User.Identity.GetUserId();
            ViewBag.postId = post.PostId;
            return View("AddMultimediaToPost");
        }


    }
}