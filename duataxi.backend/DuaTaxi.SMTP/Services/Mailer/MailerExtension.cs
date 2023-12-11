using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.SMTP.Services.Mailer
{
    public static class MailerExtension
    {

        //public static async Task SendContactEmail(this IMailerService mailer, string email_from, string Name, string Text)
        //{
        //    var ContactReciever = ConfigurationManager.AppSettings["Contact:Reciever"];
        //    var subject = "NO REPLY:Instatest, μήνυμα από τη σελίδα Contact";
        //    var body = RenderDynamicTemplate("ContactHtml.html");

        //    var baseUrl = ConfigurationManager.AppSettings["Site:BaseUrl"];
        //    body = body.Replace("!FIRST_NAME!", Name);
        //    body = body.Replace("!EMAIL_ADDRESS!", email_from);
        //    body = body.Replace("!MESSAGE!", Text);

        //    await mailer.SendAsync(subject, body, email_from, ContactReciever);
        //}

        //private static string RenderDynamicTemplate(string filename)
        //{
        //    var viewPath = HttpContext.Current.Server.MapPath(@"~\Content\EmailTemplates\" + filename);

        //    var template = System.IO.File.ReadAllText(viewPath);

        //    return template;
        //}
    }
}
