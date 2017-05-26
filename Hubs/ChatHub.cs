using Community3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;

namespace Community3.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string id, string message, int chatId)
        {
            using (var context = new ApplicationDbContext())
            {
                var manager = new UserManager<AppUser>(new UserStore<AppUser>(context));
                var sender = manager.FindById(id);
                var userName = sender.FullName;
                var chat = context.ChatRooms
                    .Where(_ => _.ChatRoomId == chatId).FirstOrDefault();

                var recipient = chat.AppUsers.Where(_ => _.Id != id).FirstOrDefault();

                var newMessage = new Message();
                newMessage.CreationTime = DateTime.Now;
                newMessage.Text = message;
                newMessage.Sender = sender;
                newMessage.SenderId = sender.Id;
                newMessage.Recipient = recipient;
                newMessage.RecipientId = recipient.Id;
                chat.Messages.Add(newMessage);
                context.SaveChanges();
            }
            Clients.All.addMessage(id, message, chatId);
        }
    }
}