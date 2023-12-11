using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.SMTP.Services.Mailer
{
    public class MailService : IMailerService
    {
        public void Send(string subject, string body, params string[] recipients)
    {
        Send(subject, body, String.Join(",", recipients));
    }

    public void Send(string subject, string body, string recipient)
    {
        //var mail = new MailMessage(ConfigurationManager.AppSettings["SMTP:Sender"], recipient, subject, body);

        //string mandrilSubAccount = ConfigurationManager.AppSettings["SMTP:Subaccount"];
        //if (!String.IsNullOrEmpty(mandrilSubAccount))
        //    mail.Headers.Add("X-MC-Subaccount", mandrilSubAccount);

        //mail.IsBodyHtml = true;

        //string bcc = ConfigurationManager.AppSettings["SMTP:Bcc"];
        //if (!String.IsNullOrEmpty(bcc))
        //    mail.Bcc.Add(bcc);

        //using (var client = CreateClient()) {
        //    try {
        //        client.Send(mail);
        //    } catch (Exception ex) {
        //        var e = new Exception($"While sending email to {recipient}.", ex);

        //        e.Data.Add("Subject", subject);
        //        e.Data.Add("Recipient", recipient);

        //        throw e;
        //    }
        //}
    }

    public async Task SendAsync(string subject, string body, string emailFrom, params string[] recipients)
    {
        await SendAsync(subject, body, emailFrom, String.Join(",", recipients));
    }

    public async Task SendAsync(string subject, string body, string emailFrom, string recipient)
    {

        //var mail = new MailMessage(ConfigurationManager.AppSettings["SMTP:Sender"] == "" ? emailFrom : ConfigurationManager.AppSettings["SMTP:Sender"], recipient, subject, body);

        //string mandrilSubAccount = ConfigurationManager.AppSettings["SMTP:Subaccount"];
        //if (!String.IsNullOrEmpty(mandrilSubAccount))
        //    mail.Headers.Add("X-MC-Subaccount", mandrilSubAccount);

        //mail.IsBodyHtml = true;

        //string bcc = ConfigurationManager.AppSettings["SMTP:Bcc"];
        //if (!String.IsNullOrEmpty(bcc))
        //    mail.Bcc.Add(bcc);

        //using (var client = CreateClient()) {
        //    try {
        //        await client.SendMailAsync(mail);
        //    } catch (Exception ex) {
        //        var e = new Exception($"While sending email to {recipient}.", ex);

        //        e.Data.Add("Subject", subject);
        //        e.Data.Add("Recipient", recipient);

        //        throw e;
        //    }
        //}
    }

    //private static SmtpClient CreateClient()
    //{
    //    //var client = new SmtpClient {
    //    //    Port = int.Parse(ConfigurationManager.AppSettings["SMTP:Port"]),
    //    //    Host = ConfigurationManager.AppSettings["SMTP:Host"],
    //    //    EnableSsl = true,
    //    //    DeliveryMethod = SmtpDeliveryMethod.Network,
    //    //    UseDefaultCredentials = false,
    //    //    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTP:Username"], ConfigurationManager.AppSettings["SMTP:Password"])
    //    //};

    //    //return client;
    //}


}
}
