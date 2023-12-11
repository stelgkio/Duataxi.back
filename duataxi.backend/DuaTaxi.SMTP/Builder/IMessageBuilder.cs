using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.SMTP.Builder
{
    public interface IMessageBuilder
    {
        IMessageBuilder WithSender(string senderEmail);
        IMessageBuilder WithReceiver(string receiverEmail);
        IMessageBuilder WithSubject(string subject);
        IMessageBuilder WithSubject(string template, params object[] @params);
        IMessageBuilder WithBody(string body);
        IMessageBuilder WithBody(string template, params object[] @params);
        MimeMessage Build();
    }
}
