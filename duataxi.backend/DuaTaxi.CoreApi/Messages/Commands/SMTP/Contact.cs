using DuaTaxi.Common.Messages;
using DuaTaxi.Common.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.CoreApi.Messages.Commands.SMTP
{
    [MessageNamespace("smtpApi")]
    public class Contact :  ICommand , IIdentifiable
    {

        public string Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject  { get; set; }

        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Message { get; set; }

        [JsonConstructor]
        public Contact(string Id, string Name , string Email , string Subject , string Message)
        {

            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.Subject = Subject;
            this.Message = Message;
        }
    }
}
