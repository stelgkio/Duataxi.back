using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.SMTP.Messages.Commands
{
    public class Contact : IIdentifiable, ICommand
    {
        public string Id { get; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }
     
        public string Message { get; set; }

        

        [JsonConstructor]
        public Contact(string Id, string Name, string Email, string Subject, string Message)
        {

            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.Subject = Subject;
            this.Message = Message;
        }
    }
}
