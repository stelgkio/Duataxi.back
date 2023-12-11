using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.SMTP.Services.Mailer2
{
   public interface IMailService
    {
        Task SendAsync(MimeMessage message);
    }
}
