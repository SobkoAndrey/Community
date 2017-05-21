using Community3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Community3.Helpers
{
    public class GroupHelper
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<AppUser> UserManager { get; set; }

        public GroupHelper()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<AppUser>(new UserStore<AppUser>(this.ApplicationDbContext));
        }

        public void DeleteGroupById(int id)
        {

            var audios = new List<Audio>();
            var images = new List<Image>();
            var posts = new List<Post>();

            var audioHelper = new AudioHelper();
            var imageHelper = new ImageHelper();
            var postHelper = new PostHelper();

            using (var context = new ApplicationDbContext())
            {
                var group = context.Groups.Where(_ => _.GroupId == id).FirstOrDefault();

                audios = group.Audios.ToList();
                images = group.Images.ToList();
                posts = group.Posts.ToList();

                group.Images.Clear();
                group.Audios.Clear();
                group.Posts.Clear();
                group.AppUsers.Clear();
                group.BlockUsers.Clear();

                context.Groups.Remove(group);
                context.SaveChanges();
            }

            foreach (var audio in audios)
            {
                audioHelper.DeleteAudioById(audio.AudioId);
            }
            foreach (var image in images)
            {
                imageHelper.DeleteImageById(image.ImageId);
            }
            foreach (var post in posts)
            {
                postHelper.DeletePostById(post.PostId);
            }

        }
    }
}