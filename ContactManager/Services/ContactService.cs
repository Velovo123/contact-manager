using ContactManager.Models;
using ContactManager.Repositories.IRepositories;
using ContactManager.Services.IServices;

namespace ContactManager.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            return await _repository.GetAllContactsAsync();
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            return await _repository.GetContactByIdAsync(id);
        }

        public async Task AddContactAsync(Contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.Name) || contact.Salary < 0)
            {
                throw new ArgumentException("Invalid contact data.");
            }

            await _repository.AddContactAsync(contact);
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.Name) || contact.Salary < 0)
            {
                throw new ArgumentException("Invalid contact data.");
            }

            await _repository.UpdateContactAsync(contact);
        }

        public async Task DeleteContactAsync(int id)
        {
            await _repository.DeleteContactAsync(id);
        }
    }
}
