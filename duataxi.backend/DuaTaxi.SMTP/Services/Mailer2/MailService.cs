using DuaTaxi.Common.MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace DuaTaxi.SMTP.Services.Mailer2
{
    public class MailService : IMailService
    {
        private readonly MailKitOptions _options;

        public MailService(MailKitOptions options)
        {
            _options = options;
        }

        public async Task SendAsync(MimeMessage msg)
        {
            using (var client = new SmtpClient()) {

                //MimeMessage message = new MimeMessage();

                //MailboxAddress from = new MailboxAddress("Admin",
                //"info@duataxi.com");
                //message.From.Add(from);

                //MailboxAddress to = new MailboxAddress(msg.,
                //"user@example.com");
                //message.To.Add(to);

                //message.Subject = "This is email subject";

                //BodyBuilder bodyBuilder = new BodyBuilder();
                //bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
                //bodyBuilder.TextBody = "Hello World!";

                client.Connect(_options.SmtpHost, _options.Port, true);
                client.Authenticate(_options.Username, _options.Password);

                await client.SendAsync(msg);
                client.Disconnect(true);
            }
        }
    }
}
