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
    public class ChatHelper
    {
        public void DeleteChatById(int id)
        {
            var messages = new List<Message>();

            using (var context = new ApplicationDbContext())
            {
                var chat = context.ChatRooms.Where(_ => _.ChatRoomId == id).First();

                messages = chat.Messages.ToList();

                chat.AppUsers.Clear();
                chat.Messages.Clear();

                context.ChatRooms.Remove(chat);
                context.SaveChanges();

            }

            if (messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    DeleteMessageById(message.MessageId);
                }
            }
        }

        public void DeleteMessageById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var message = context.Messages.Where(_ => _.MessageId == id).First();

                message.Recipient = null;
                message.RecipientId = null;
                message.Sender = null;
                message.SenderId = null;

                context.Messages.Remove(message);
                context.SaveChanges();
            }
        }
    }
}