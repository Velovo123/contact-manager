using ContactManager.Models;
using ContactManager.Services;
using CsvHelper;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using ContactManager.Tests.Helper;

public class CsvParsingServiceTests
{
    private readonly CsvParsingService _csvParsingService;

    public CsvParsingServiceTests()
    {
        _csvParsingService = new CsvParsingService();
    }

    [Fact]
    public async Task ParseCsvFileAsync_ValidCsv_ReturnsContacts()
    {
        // Arrange
        var csvContent = "Name,DateOfBirth,Married,Phone,Salary\n" +
                         "John Doe,1980-01-01,true,1234567890,50000\n" +
                         "Jane Smith,1990-05-10,false,0987654321,60000";
        var formFile = FormFileHelper.CreateMockFormFile(csvContent, "contacts.csv");

        // Act
        var result = await _csvParsingService.ParseCsvFileAsync(formFile);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var firstContact = result.First();
        Assert.Equal("John Doe", firstContact.Name);
        Assert.Equal(new System.DateTime(1980, 1, 1), firstContact.DateOfBirth);
        Assert.True(firstContact.Married);
        Assert.Equal("1234567890", firstContact.Phone);
        Assert.Equal(50000m, firstContact.Salary);
    }

    [Fact]
    public async Task ParseCsvFileAsync_InvalidCsv_ThrowsException()
    {
        // Arrange
        var csvContent = "InvalidHeader1,InvalidHeader2\n" +
                         "SomeData,SomeOtherData";
        var formFile = FormFileHelper.CreateMockFormFile(csvContent, "invalid.csv");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _csvParsingService.ParseCsvFileAsync(formFile));

        Assert.IsType<CsvHelper.HeaderValidationException>(exception.InnerException);
    }

    [Fact]
    public async Task ParseCsvFileAsync_EmptyCsv_ThrowsArgumentException()
    {
        // Arrange
        var formFile = FormFileHelper.CreateMockFormFile(string.Empty, "empty.csv");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _csvParsingService.ParseCsvFileAsync(formFile));
    }
}
