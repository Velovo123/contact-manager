using ContactManager.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactManager.Services.IServices
{
    public interface IContactService
    {
        Task<List<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(int id);
        Task AddContactsAsync(IFormFile csvFile); 
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(int id);
    }
}
