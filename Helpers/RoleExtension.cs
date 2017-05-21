using Community3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Community3.Helpers
{
    public static class RoleExtension
    {
        public static string GetName(this IdentityUserRole role)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>
                (new ApplicationDbContext()));

            var rol = roleManager.Roles.Where(_ => _.Id == role.RoleId).First();
            return rol.Name;
        }
    }
}