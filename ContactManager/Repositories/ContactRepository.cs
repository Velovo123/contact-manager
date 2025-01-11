using ContactManager.Data;
using ContactManager.Models;
using ContactManager.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactDbContext _context;

        public ContactRepository(ContactDbContext context)
        {
            _context = context;
        }
       

        async Task<List<Contact>> IContactRepository.GetAllContactsAsync()
        {
            return await _context.Contacts.ToListAsync();
        }

        async Task<Contact> IContactRepository.GetContactByIdAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task AddContactAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }
    }
}
