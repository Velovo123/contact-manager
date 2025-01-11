using ContactManager.Models;
using System.Formats.Asn1;
using System.Globalization;

namespace ContactManager.Services.IServices
{
    public interface ICsvParsingService
    {
        Task<List<Contact>> ParseCsvFileAsync(IFormFile csvFile);
    }
}
