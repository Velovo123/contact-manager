using ContactManager.Models;
using ContactManager.Repositories.IRepositories;
using ContactManager.Services.IServices;

namespace ContactManager.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;
        private readonly ICsvParsingService _csvParsingService;

        public ContactService(IContactRepository repository, ICsvParsingService csvParsingService)
        {
            _repository = repository;
            _csvParsingService = csvParsingService;
        }

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            return await _repository.GetAllContactsAsync();
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            return await _repository.GetContactByIdAsync(id);
        }

        public async Task AddContactsAsync(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
                throw new ArgumentException("CSV file is empty or invalid.");

            var contacts = await _csvParsingService.ParseCsvFileAsync(csvFile);

            foreach (var contact in contacts)
            {
                if (string.IsNullOrWhiteSpace(contact.Name) || contact.Salary < 0)
                {
                    continue;
                }

                await _repository.AddContactAsync(contact);
            }
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
