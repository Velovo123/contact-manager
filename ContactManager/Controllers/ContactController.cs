using ContactManager.Filters;
using ContactManager.Models;
using ContactManager.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Controllers
{

    [Route("Contact")]
    public class ContactController : Controller
    {

        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return View(contacts);
        }

        [HttpPost("UploadCsv")]
        [FileValidation]
        public async Task<IActionResult> UploadCsv(IFormFile csvFile)
        {
            await _contactService.AddContactsAsync(csvFile);
            TempData["Success"] = "CSV file processed successfully.";

            return RedirectToAction("Index");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id,[FromBody]Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid contact data." });
            }
            await _contactService.UpdateContactAsync(contact);
            return Ok(new { message = "Contact updated successfully." });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            await _contactService.DeleteContactAsync(id); 
            return Ok(new { message = "Contact deleted successfully." });
        }
    }
}
