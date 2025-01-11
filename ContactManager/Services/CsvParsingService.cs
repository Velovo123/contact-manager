using ContactManager.Models;
using ContactManager.Services.IServices;
using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;

namespace ContactManager.Services
{
    public class CsvParsingService : ICsvParsingService
    {
        public async Task<List<Contact>> ParseCsvFileAsync(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
                throw new ArgumentException("CSV file is empty or invalid.");

            var contacts = new List<Contact>();

            using (var stream = new StreamReader(csvFile.OpenReadStream()))
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    PrepareHeaderForMatch = args => args.Header.ToLower(),
                };

                using (var csvReader = new CsvReader(stream, csvConfig))
                {
                    csvReader.Context.RegisterClassMap<ContactMap>();
                    try
                    {
                        await foreach (var contact in csvReader.GetRecordsAsync<Contact>())
                        {
                            contacts.Add(contact);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Error parsing CSV file.", ex);
                    }
                }
            }

            return contacts;
        }
    }

    public class ContactMap : ClassMap<Contact>
    {
        public ContactMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.DateOfBirth).Name("DateOfBirth");
            Map(m => m.Married).Name("Married");
            Map(m => m.Phone).Name("Phone");
            Map(m => m.Salary).Name("Salary");

            Map(m => m.Id).Ignore();
        }
    }
}
