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

namespace Community3.Helpers
{
    public class UserHelpers
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<AppUser> UserManager { get; set; }

        public UserHelpers()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<AppUser>(new UserStore<AppUser>(this.ApplicationDbContext));
        }

        public string GetUserFullName(string userId)
        {
            var user = UserManager.FindById(userId);
            if (user != null)
            {
                return user.Name + " " + user.Surname;
            }
            else
            {
                return "";
            }
        }
    }
}