using Community3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Community3.Helpers;
using System.Data.Entity;
using System.IO;

namespace Community3.Controllers
{
    [Authorize(Roles = "user")]
    [HandleError(ExceptionType = typeof(Exception), View = "Error")]
    public class GroupController : Controller
    {

        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<AppUser> UserManager { get; set; }

        public GroupController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<AppUser>(new UserStore<AppUser>(this.ApplicationDbContext));
        }

        [HttpGet]
        public ActionResult ShowGroupPage(int id)
        {
            ViewBag.userId = User.Identity.GetUserId();
            var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
            if(group == null)
            {
                return View("NullGroupReferenceError");
            }
            ViewBag.currentUser = UserManager.FindById(User.Identity.GetUserId());
            return View(group);
        }

        public ActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroup(GroupCreationModel model)
        {
            if (ModelState.IsValid)
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

                return RedirectToRoute("Default", new { controller = "Group", action = "ShowGroupPage", id = grp.GroupId });
            }

            return View();
        }

        [HttpGet]
        public ActionResult ManageGroup(int id)
        {
            var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
            if (group == null)
            {
                return View("NullGroupReferenceError");
            }
            return View(group);
        }

        [ValidateAntiForgeryToken]
        public ActionResult EditGroupInfo(int id, string name, string description = "")
        {
            var grp = new Group();
            using (ApplicationDbContext)
            {
                var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
                if (name != null)
                {
                    group.Name = name;
                }
                group.Description = description;
                group.Owner = UserManager.FindById(User.Identity.GetUserId());
                ApplicationDbContext.SaveChanges();
                grp = group;
            }
            ViewBag.Info = "Готово!";
            return View("ManageGroup", grp);
        }

        [HttpPost]
        public ActionResult AddAudioToAlbum(HttpPostedFileBase file, int id)
        {
            if (file == null)
            {
                return View("Error");
            }

            var grp = new Group();
            var helper = new AudioHelper();
            var audio = helper.GetAudioFromFile(file);

            if (audio != null)
            {
                using (ApplicationDbContext)
                {
                    var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
                    if(group == null)
                    {
                        return View("NullGroupReferenceError");
                    }
                    group.Audios.Add(audio);
                    ApplicationDbContext.SaveChanges();
                    grp = group;
                }

            }
            else
            {
                return View("WrongAudioFileExtensionError");
            }

            ViewBag.Audio = "Готово!";
            return View("ManageGroup", grp);
        }

        [HttpGet]
        public ActionResult ShowAudio(int id)
        {
            ViewBag.userId = User.Identity.GetUserId();
            var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
            if(group == null)
            {
                return View("NullGroupReferenceError");
            }
            return View(group);
        }

        [HttpGet]
        public ActionResult RemoveAudio(int groupId, int audioId)
        {
            var audioHelper = new AudioHelper();
            audioHelper.DeleteAudioById(audioId);

            return RedirectToRoute("Default", new { controller = "Group", action = "ShowAudio", id = groupId });
        }

        [HttpPost]
        public ActionResult AddPhotoToAlbum(HttpPostedFileBase file, int id)
        {
            if (file == null)
            {
                return View("Error");
            }

            var grp = new Group();
            var helper = new ImageHelper();
            var image = helper.GetImageFromFile(file);

            if (image != null)
            {
                using (ApplicationDbContext)
                {
                    var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
                    group.Images.Add(image);
                    ApplicationDbContext.SaveChanges();
                    grp = group;
                }

            }
            else
            {
                return View("WrongImageFileExtensionError");
            }

            ViewBag.Image = "Готово!";
            return View("ManageGroup", grp);
        }

        [HttpGet]
        public ActionResult ShowImages(int id)
        {
            ViewBag.userId = User.Identity.GetUserId();
            var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
            if(group == null)
            {
                return View("NullGroupReferenceError");
            }
            return View(group);
        }

        [HttpGet]
        public ActionResult RemoveImage(int groupId, int imageId)
        {
            var imageHelper = new ImageHelper();
            imageHelper.DeleteImageById(imageId);

            return RedirectToRoute("Default", new { controller = "Group", action = "ShowImages", id = groupId });
        }

        [HttpGet]
        public ActionResult Subscribe(int id)
        {
            ViewBag.userId = User.Identity.GetUserId();
            var user = UserManager.FindById(User.Identity.GetUserId());
            var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
            if (group == null)
            {
                return View("NullGroupReferenceError");
            }

            var grp = new Group();
            using (ApplicationDbContext)
            {
                user.Groups.Add(group);
                group.AppUsers.Add(user);
                ApplicationDbContext.SaveChanges();
                grp = group;
            }
            return RedirectToRoute("Default", new { controller = "Group", action = "ShowGroupPage", id = id });
        }

        [HttpGet]
        public ActionResult Unsubscribe(int id)
        {
            ViewBag.userId = User.Identity.GetUserId();
            var user = UserManager.FindById(User.Identity.GetUserId());
            var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
            if (group == null)
            {
                return View("NullGroupReferenceError");
            }

            var grp = new Group();
            using (ApplicationDbContext)
            {
                user.Groups.Remove(group);
                group.AppUsers.Remove(user);
                ApplicationDbContext.SaveChanges();
                UserManager.Update(user);
                grp = group;
            }
            return RedirectToRoute("Default", new { controller = "Group", action = "ShowGroupPage", id = id });
        }

        [HttpGet]
        public ActionResult ShowParticipants(int id)
        {
            ViewBag.userId = User.Identity.GetUserId();
            var group = ApplicationDbContext.Groups.Where(g => g.GroupId == id).FirstOrDefault();
            if (group == null)
            {
                return View("NullGroupReferenceError");
            }

            return View(group);
        }

        [HttpGet]
        public ActionResult BlockUser(int groupId, string userId)
        {
            var group = ApplicationDbContext.Groups.Where(g => g.GroupId == groupId).FirstOrDefault();
            var user = UserManager.FindById(userId);

            using (ApplicationDbContext)
            {
                group.AppUsers.Remove(user);
                group.BlockUsers.Add(user);
                ApplicationDbContext.SaveChanges();
            }

            return RedirectToAction("ShowParticipants", new { id = group.GroupId });
        }

        [HttpGet]
        public ActionResult AddPost(int id)
        {
            ViewBag.groupId = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddPost(int groupId, Post post)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext)
                {
                    post.CreationDate = DateTime.Now;
                    ApplicationDbContext.Posts.Add(post);
                    ApplicationDbContext.SaveChanges();
                }
                ViewBag.groupId = groupId;
                ViewBag.postId = post.PostId;
                return View("AddMultimediaToPost");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddImageToPostAjax()
        {
            var postId = Convert.ToInt32(Request.Form.GetValues("postId")[0]);
            var post = ApplicationDbContext.Posts.Where(_ => _.PostId == postId).FirstOrDefault();
            var file = Request.Files[0];
            var helper = new ImageHelper();
            var image = helper.GetImageFromFile(file);
            var images = new List<Image>();
            images.Add(image);
            using (ApplicationDbContext)
            {
                post.Images.Add(image);
                ApplicationDbContext.SaveChanges();
            }

            return PartialView("_Images", images);
        }

        [HttpPost]
        public ActionResult AddAudioToPostAjax()
        {
            var postId = Convert.ToInt32(Request.Form.GetValues("postId")[0]);
            var post = ApplicationDbContext.Posts.Where(_ => _.PostId == postId).FirstOrDefault();
            var file = Request.Files[0];
            var helper = new AudioHelper();
            var audio = helper.GetAudioFromFile(file);
            var audios = new List<Audio>();
            audios.Add(audio);
            using (ApplicationDbContext)
            {
                post.Audios.Add(audio);
                ApplicationDbContext.SaveChanges();
            }

            return PartialView("_Audios", audios);
        }

        [HttpPost]
        public ActionResult CreatePost(string currentUserId, int? groupId, int postId)
        {
            if (groupId != null)
            {
                using (ApplicationDbContext)
                {
                    var post = ApplicationDbContext.Posts.Where(p => p.PostId == postId).FirstOrDefault();
                    post.GroupId = groupId;
                    post.CreationDate = DateTime.Now;
                    var group = ApplicationDbContext.Groups.Where(g => g.GroupId == groupId).FirstOrDefault();
                    group.Posts.Add(post);
                    ApplicationDbContext.SaveChanges();
                }
                return RedirectToAction("ShowGroupPage", new { id = groupId });
            }

            using (ApplicationDbContext)
            {
                var post = ApplicationDbContext.Posts.Where(p => p.PostId == postId).FirstOrDefault();
                post.AppUserId = currentUserId;
                post.CreationDate = DateTime.Now;
                var user = UserManager.FindById(currentUserId);
                user.Posts.Add(post);
                ApplicationDbContext.SaveChanges();
            }
            return RedirectToAction("News", "Home", new { id = currentUserId });
        }

        [HttpPost]
        public ActionResult AddLikeAjax()
        {

            var postId = Convert.ToInt32(Request.Form.GetValues("postId")[0]);
            var post = ApplicationDbContext.Posts.Where(_ => _.PostId == postId).FirstOrDefault();
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            foreach (var item in post.Likes)
            {
                if (item.AppUserId == currentUser.Id)
                {
                    return PartialView("_Likes", post.Likes);
                }
            }
            var like = new Like();
            like.AppUser = UserManager.FindById(User.Identity.GetUserId());
            like.Post = post;
            using (ApplicationDbContext)
            {
                post.Likes.Add(like);
                post.AppUsersLiked.Add(currentUser);
                ApplicationDbContext.SaveChanges();
            }

            return PartialView("_Likes", post.Likes);
        }

        [HttpPost]
        public void RemovePostAjax()
        {
            var postId = Convert.ToInt32(Request.Form.GetValues("postId")[0]);

            var postHelper = new PostHelper();
            postHelper.DeletePostById(postId);
        }

        [HttpGet]
        public ActionResult RemoveGroup(int id)
        {
            var groupHelper = new GroupHelper();
            groupHelper.DeleteGroupById(id);

            return RedirectToAction("Groups", "Home");
        }
    }
}