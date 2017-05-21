namespace Community3.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Community3.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Community3.Models.ApplicationDbContext";
        }

        protected override void Seed(Community3.Models.ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<AppUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var user = new IdentityRole { Name = "user" };
            var admin = new IdentityRole { Name = "admin" };
            var moder = new IdentityRole { Name = "moder" };
            var inBlock = new IdentityRole { Name = "blocked" };
            
            roleManager.Create(user);
            roleManager.Create(admin);
            roleManager.Create(moder);
            roleManager.Create(inBlock);

            var mainAdmin = new AppUser { Name = "admin", UserName = "admin@admin.ru", Email = "admin@admin.ru", Gender = Gender.Empty, Surname = "admin" };
            string password = "111111";
            var result = userManager.Create(mainAdmin, password);

            if (result.Succeeded)
            {
                userManager.AddToRole(mainAdmin.Id, admin.Name);
                userManager.AddToRole(mainAdmin.Id, moder.Name);
                userManager.AddToRole(mainAdmin.Id, user.Name);
            }

            base.Seed(context);
        }
    }
}
