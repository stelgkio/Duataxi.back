using DuaTaxi.Common.Handlers;
using DuaTaxi.Common.MailKit;
using DuaTaxi.Common.RabbitMq;
using DuaTaxi.Common.Types;
using DuaTaxi.SMTP.Builder;
using DuaTaxi.SMTP.Entities.Models;
using DuaTaxi.SMTP.Messages.Commands;
using DuaTaxi.SMTP.Repository;
using DuaTaxi.SMTP.Services.Mailer2;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuaTaxi.SMTP.Handler.ContactForms
{
    public class ContactFormHandler : ICommandHandler<Contact>
    {
        private IContactFormRepository _contactFormRepository;
        private IBusPublisher _busPublisher;
        private readonly ILogger<Contact> _logger;
        private readonly MailKitOptions _options;
        private readonly IMailService _mailService;

        public ContactFormHandler(IContactFormRepository contactFormRepository, IBusPublisher busPublisher, ILogger<Contact> logger , MailKitOptions options, IMailService mailService)
        {
            _contactFormRepository = contactFormRepository;
            _busPublisher = busPublisher;
            _logger = logger;
            _options = options;
            _mailService = mailService;
        }

        public async  Task HandleAsync(Contact command, ICorrelationContext context)
        {
            try {

            await _contactFormRepository.AddSync(new ContactForm(command.Id,command.Name,command.Email,command.Subject,command.Message));


                var message = MessageBuilder
               .Create()
               .WithReceiver(command.Email)
               .WithSender(_options.Email)
               .WithSubject(command.Subject)
               .WithBody(command.Message)
               .Build();

                await _mailService.SendAsync(message);

            } catch (Exception ex) {

                _logger.LogError("ContactFrom error : ", ex.Message);
                //_busPublisher.PublishAsync  an xreiazetai na kanw kapoio saga gia na steilw mnm oti den phge kala HHHHHHH//// na dokimasw pali
                throw new DuaTaxiException(ex.Message,ex.StackTrace);
            }
        }
    }
}
