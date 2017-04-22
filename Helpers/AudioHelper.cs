using Community3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Community3.Helpers
{
    public class AudioHelper
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        public AudioHelper()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
        }

        List<string> extensions = new List<string>() { ".mp3", ".AAC", ".wav" };

        public Audio GetAudioFromFile(HttpPostedFileBase file)
        {
            var fileName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);
            fileName += extension;

            if (extensions.Contains(extension.ToLower()) || extensions.Contains(extension.ToUpper()))
            {
                file.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~/Audios/" + fileName));

                Audio audio = new Audio();

                    audio.Path = "~/Audios/" + fileName;
                    audio.Name = fileName;
                    audio.Label = file.FileName;
                    return audio;
            }
            else
            {
                return null;
            }
        }

        public void DeleteAudioById(int id)
        {
            var audio = ApplicationDbContext.Audios.Where(a => a.AudioId == id).FirstOrDefault();
            var path = System.Web.HttpContext.Current.Server.MapPath(audio.Path);
            System.IO.File.Delete(path);
            using (ApplicationDbContext)
            {
                ApplicationDbContext.Audios.Remove(audio);
                ApplicationDbContext.SaveChanges();
            }
        }
    }
}