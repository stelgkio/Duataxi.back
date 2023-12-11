using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.SMTP.Services.Mailer
{
    public interface IMailerService
    {
        void Send(string subject, string body, params string[] recipients);

        void Send(string subject, string body, string recipient);

        Task SendAsync(string subject, string body, string emailFrom, params string[] recipients);

        Task SendAsync(string subject, string body, string emailFrom, string recipient);
    }
}
