﻿using Community3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Community3.Helpers
{
    public class PostHelper
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<AppUser> UserManager { get; set; }

        public PostHelper()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<AppUser>(new UserStore<AppUser>(this.ApplicationDbContext));
        }

        public void DeletePostById(int id)
        {
            var audios = new List<Audio>();
            var images = new List<Image>();

            using (var context = new ApplicationDbContext())
            {
                var post = context.Posts.Where(_ => _.PostId == id).FirstOrDefault();
                var likes = post.Likes.ToList();
                images = post.Images.ToList();
                audios = post.Audios.ToList();

                foreach (var like in likes)
                {
                    context.Likes.Remove(like);
                    context.SaveChanges();
                }
                
                post.Audios.Clear();
                post.Images.Clear();
                post.Likes.Clear();
                post.AppUsersLiked.Clear();

                context.Posts.Remove(post);
                context.SaveChanges();
            }

            foreach (var audio in audios)
            {
                var audioHelper = new AudioHelper();
                audioHelper.DeleteAudioById(audio.AudioId);
            }
            foreach (var image in images)
            {
                var imageHelper = new ImageHelper();
                imageHelper.DeleteImageById(image.ImageId);
            }
        }
    }
}