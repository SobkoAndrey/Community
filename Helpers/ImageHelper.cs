using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Community3.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;



namespace Community3.Helpers
{
    public class ImageHelper
    {
        //protected ApplicationDbContext ApplicationDbContext { get; set; }
        //protected UserManager<AppUser> UserManager { get; set; }

        //public ImageHelper()
        //{
        //    this.ApplicationDbContext = new ApplicationDbContext();
        //    this.UserManager = new UserManager<AppUser>(new UserStore<AppUser>(this.ApplicationDbContext));
        //}

        List<string> extensions = new List<string>() { ".jpg", ".gif", ".png" };

        public Image GetImageFromFile(HttpPostedFileBase file)
        {
            var fileName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);
            fileName += extension;

            if (extensions.Contains(extension.ToLower()) || extensions.Contains(extension.ToUpper()))
            {
                try
                {
                    file.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~/Images/" + fileName));
                }
                catch (Exception exception)
                {
                    return null;
                }

                Image image = new Image();

                image.Path = "~/Images/" + fileName;
                image.Name = fileName;
                image.Label = file.FileName;

                return image;
            }
            else
            {
                return null;
            }
        }

        public void DeleteImageById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var image = context.Images.Where(a => a.ImageId == id).FirstOrDefault();
                var path = System.Web.HttpContext.Current.Server.MapPath(image.Path);
                System.IO.File.Delete(path);
                context.Images.Remove(image);
                context.SaveChanges();
            }
        }
    }
}