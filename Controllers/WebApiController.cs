using Community3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Community3.Helpers;

namespace Community3.Controllers
{
    public class WebApiController : ApiController
    {
        [HttpPut]
        public void RenameAudio([FromBody]Audio audio)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(audio).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteAudio(int id)
        {
            var helper = new AudioHelper();
            helper.DeleteAudioById(id);
        }
    }
}
