using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Community3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using System.Web.Mvc;
using Community3.Helpers;
using System.Data.Entity;
using System.IO;

namespace Community3.Hubs
{
    public class ChatHub : Hub
    {

        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<AppUser> UserManager { get; set; }

        public ChatHub()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<AppUser>(new UserStore<AppUser>(this.ApplicationDbContext));
        }

        static List<AppUser> Users = new List<AppUser>();

        public void Send(string id, string message, int chatId)
        {
            var sender = UserManager.FindById(id);
            var userName = sender.FullName;
            var chat = ApplicationDbContext.ChatRooms
                .Where(_ => _.ChatRoomId == chatId).FirstOrDefault();

            var recipient = chat.AppUsers.Where(_ => _.Id != id).FirstOrDefault();

            using (ApplicationDbContext)
            {
                var newMessage = new Message();
                newMessage.CreationTime = DateTime.Now;
                newMessage.Text = message;
                newMessage.Sender = sender;
                newMessage.SenderId = sender.Id;
                newMessage.Recipient = recipient;
                newMessage.RecipientId = recipient.Id;
                chat.Messages.Add(newMessage);
                ApplicationDbContext.SaveChanges();
            }
            Clients.All.addMessage(userName, message);
        }


        //public void Send(string name, string message)
        //{
        //    Clients.All.addMessage(name, message);
        //}

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;


            //if (!Users.Any(_ => _.Id == id))
            //{
                Users.Add(UserManager.FindById(id));

                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(id, userName, Users);

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            //}
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.Id == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}