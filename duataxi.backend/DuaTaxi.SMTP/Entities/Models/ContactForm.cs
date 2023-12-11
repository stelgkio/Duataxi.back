using DuaTaxi.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.SMTP.Entities.Models
{
    public class ContactForm : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }


        public ContactForm(string Id, string Name, string Email, string Subject, string Message)
        {    
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.Subject = Subject;
            this.Message = Message;
        }
    }
}
