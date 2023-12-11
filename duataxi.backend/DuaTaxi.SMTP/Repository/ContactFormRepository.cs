using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuaTaxi.Common.Mongo;
using DuaTaxi.SMTP.Entities.Models;
using DuaTaxi.SMTP.Messages.Commands;

namespace DuaTaxi.SMTP.Repository
{
    public class ContactFormRepository : IContactFormRepository
    {
        private IMongoRepository<ContactForm> _repository;

        public ContactFormRepository(IMongoRepository<ContactForm> repository)
        {
            _repository = repository;                                                  
        }

        public async Task AddSync(ContactForm contact)
            => await _repository.AddAsync(contact);


        public async Task DeleteAsync(string Id)
            => await _repository.DeleteAsync(Id);


        public async Task<ContactForm> GetByIdAsync(string Id)
            => await _repository.GetAsync(Id);


        public async Task UpdateAsync(ContactForm contact)
        => await _repository.UpdateAsync(contact);
    }
}
