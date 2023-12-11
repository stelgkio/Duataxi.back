using DuaTaxi.SMTP.Entities.Models;
using System;
using System.Threading.Tasks;

namespace DuaTaxi.SMTP.Repository
{
    public interface IContactFormRepository
    {
        Task AddSync(ContactForm contact);

        Task UpdateAsync(ContactForm contact);

        Task DeleteAsync(string Id);

        Task<ContactForm> GetByIdAsync(string Id);
    }
}
