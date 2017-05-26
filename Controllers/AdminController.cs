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

namespace Community3.Controllers
{
    [Authorize(Roles = "admin")]
    [HandleError(ExceptionType = typeof(Exception), View = "Error")]
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult AdminPage()
        {
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(new ApplicationDbContext()));

            var users = userManager.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public ActionResult IdSearchAjax()
        {
            var userId = Request.Form.GetValues("userId")[0];
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(new ApplicationDbContext()));
            var user = userManager.FindById(userId);
            if (user != null)
            {
                var usersList = new List<AppUser> { user };
                return PartialView("_UsersList", usersList);
            }

            return PartialView("_EmptyList");
        }

        [HttpPost]
        public ActionResult NameSearchAjax()
        {
            var userName = Request.Form.GetValues("userName")[0];
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(new ApplicationDbContext()));
            var users = userManager.Users.Where(_ => (_.Name + _.Surname).Contains(userName)).ToList();
            if (users.Count == 0 || userName.Length == 0)
            {
                return PartialView("_EmptyList");
            }

            return PartialView("_UsersList", users);
        }

        [HttpPost]
        public ActionResult EmailSearchAjax()
        {
            var userEmail = Request.Form.GetValues("userEmail")[0];
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(new ApplicationDbContext()));
            var users = userManager.Users.Where(_ => _.Email.Contains(userEmail)).ToList();
            if (users.Count == 0 || userEmail.Length == 0)
            {
                return PartialView("_EmptyList");
            }

            return PartialView("_UsersList", users);
        }

        [HttpPost]
        public ActionResult GroupIdSearchAjax()
        {
            int groupId;
            var result = Int32.TryParse(Request.Form.GetValues("groupId")[0], out groupId);
            if (!result)
            {
                return PartialView("_EmptyList");
            }
            try
            {
                var groupsList = new List<Group>();
                using (var context = new ApplicationDbContext())
                {
                    var group = context.Groups.Include(_ => _.Owner).Where(_ => _.GroupId == groupId).First(); ;
                    if (group == null)
                    {
                        return PartialView("_EmptyList");
                    }
                    groupsList.Add(group);
                }
                return PartialView("_GroupsList", groupsList);
            }
            catch
            {
                return PartialView("_EmptyList");
            }
        }

        [HttpPost]
        public ActionResult GroupNameSearchAjax()
        {
            var groupName = Request.Form.GetValues("groupName")[0];
            var groupsList = new List<Group>();
            using (var context = new ApplicationDbContext())
            {
                var groups = context.Groups.Include(_ => _.Owner).Where(_ => _.Name.Contains(groupName)).ToList();
                if (groups.Count == 0 || groupName.Length == 0)
                {
                    return PartialView("_EmptyList");
                }
                groupsList = groups;
            }
            return PartialView("_GroupsList", groupsList);
        }

        [HttpPost]
        public ActionResult GroupOwnerSearchAjax()
        {
            var groupOwnerId = Request.Form.GetValues("ownerId")[0];
            var groupsList = new List<Group>();
            using (var context = new ApplicationDbContext())
            {
                var groups = context.Groups.Include(_ => _.Owner).Where(_ => _.OwnerId == groupOwnerId).ToList();
                if (groups.Count == 0 || groupOwnerId.Length == 0)
                {
                    return PartialView("_EmptyList");
                }
                groupsList = groups;
            }
            return PartialView("_GroupsList", groupsList);
        }

        [HttpGet]
        public ActionResult ManageUser(string id)
        {
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(new ApplicationDbContext()));
            var user = userManager.FindById(id);
            return View(user);
        }

        [HttpGet]
        public ActionResult RemoveUser(string id)
        {
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(new ApplicationDbContext()));
            var images = new List<Image>();
            var audios = new List<Audio>();
            var posts = new List<Post>();
            var groups = new List<Group>();
            var chatrooms = new List<ChatRoom>();
            var candidates = new List<AppUser>();
            var friends = new List<AppUser>();
            var ownerGroups = new List<Group>();

            var imageHelper = new ImageHelper();
            var audioHelper = new AudioHelper();
            var postHelper = new PostHelper();
            var groupHelper = new GroupHelper();
            var chatHelper = new ChatHelper();

            var user = userManager.FindById(id);

            using (var context = new ApplicationDbContext())
            {

                ownerGroups = context.Groups.Where(_ => _.OwnerId == id).ToList();

                audios = user.Audios.ToList();
                images = user.Images.ToList();
                posts = user.Posts.ToList();
                groups = user.Groups.ToList();
                candidates = user.Candidates.ToList();
                friends = user.Friends.ToList();
                chatrooms = user.ChatRooms.ToList();

                user.Images.Clear();
                user.Audios.Clear();
                user.Posts.Clear();
                user.Groups.Clear();
                user.Candidates.Clear();
                user.Friends.Clear();
                user.ChatRooms.Clear();

                context.SaveChanges();
            }

            if (images.Count > 0)
            {
                foreach (var image in images)
                {
                    imageHelper.DeleteImageById(image.ImageId);
                }
            }

            if (audios.Count > 0)
            {
                foreach (var audio in audios)
                {
                    audioHelper.DeleteAudioById(audio.AudioId);
                }
            }

            if (posts.Count > 0)
            {
                foreach (var post in posts)
                {
                    postHelper.DeletePostById(post.PostId);
                }
            }

            if (ownerGroups.Count > 0)
            {
                foreach (var group in ownerGroups)
                {
                    groupHelper.DeleteGroupById(group.GroupId);
                }
            }

            if (friends.Count > 0)
            {
                foreach (var friend in friends)
                {
                    FinishFriendship(friend.Id, user.Id);
                }
            }

            if (chatrooms.Count > 0)
            {
                foreach (var chat in chatrooms)
                {
                    chatHelper.DeleteChatById(chat.ChatRoomId);
                }
            }

            if (groups.Count > 0)
            {
                foreach (var group in groups)
                {
                    RemoveUserFromGroup(user.Id, group.GroupId);
                }
            }

            foreach (var appUser in userManager.Users)
            {
                    var userToDelete = appUser.Candidates.Where(_ => _.Id == user.Id).FirstOrDefault();

                    if (userToDelete != null)
                    {
                        RemoveUserFromCandidates(appUser.Id, userToDelete.Id);
                    }           
            }


            DeleteUser(user.Id);
            return RedirectToAction("AdminPage"); ;
        }

        public void DeleteUser(string userId)
        {
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(new ApplicationDbContext()));
            var user = userManager.FindById(userId);
            userManager.Delete(user);
        }

        public void FinishFriendship(string userId1, string userId2)
        {
            using (var context = new ApplicationDbContext())
            {
                var userManager = new ApplicationUserManager(new UserStore<AppUser>(context));
                var user1 = userManager.FindById(userId1);
                var user2 = userManager.FindById(userId2);
                user1.Friends.Remove(user2);
                user2.Friends.Remove(user1);
                userManager.Update(user1);
                userManager.Update(user2);
                context.SaveChanges();
            }
        }

        public void RemoveUserFromGroup(string userId, int groupId)
        {
            using (var context = new ApplicationDbContext())
            {
                var userManager = new ApplicationUserManager(new UserStore<AppUser>(context));
                var user = userManager.FindById(userId);
                var group = context.Groups.Where(_ => _.GroupId == groupId).First();
                group.AppUsers.Remove(user);
                context.SaveChanges();
            }
        }

        public void RemoveUserFromCandidates(string userId, string userToDeleteId)
        {
            using (var context = new ApplicationDbContext())
            {
                var userManager = new ApplicationUserManager(new UserStore<AppUser>(context));
                var user = userManager.FindById(userId);
                var userToDelete = userManager.FindById(userToDeleteId);
                user.Candidates.Remove(userToDelete);
                userManager.Update(user);
                context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult BlockUnblockUser(string id)
        {

            var userManager = new ApplicationUserManager(new UserStore<AppUser>(new ApplicationDbContext()));
            if (userManager.IsInRole(id, "blocked"))
            {
                userManager.RemoveFromRole(id, "blocked");
                userManager.AddToRole(id, "user");
            }
            else
            {
                userManager.RemoveFromRole(id, "user");
                userManager.AddToRole(id, "blocked");
            }

            return RedirectToAction("UserProfile", "Home", new { id = id });
        }

        [HttpGet]
        public ActionResult AddUserAdminRole(string id)
        {
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(new ApplicationDbContext()));

            if (userManager.IsInRole(id, "admin"))
            {
                userManager.RemoveFromRole(id, "admin");
            }
            else
            {
                userManager.AddToRole(id, "admin");
            }

            return RedirectToAction("AdminPage");
        }

        [HttpGet]
        public ActionResult AddUserModerRole(string id)
        {
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(new ApplicationDbContext()));

            if (userManager.IsInRole(id, "moder"))
            {
                userManager.RemoveFromRole(id, "moder");
            }
            else
            {
                userManager.AddToRole(id, "moder");
            }

            return RedirectToAction("AdminPage");
        }
    }
}