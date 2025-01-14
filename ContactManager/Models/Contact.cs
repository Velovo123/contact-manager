using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public bool Married { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; }

        [Range(0, double.MaxValue)]
        [Precision(18, 2)]
        public decimal Salary { get; set; }


        public Contact(string name, DateTime dateOfBirth, bool married, string phone, decimal salary)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Married = married;
            Phone = phone;
            Salary = salary;
        }

        public Contact()
        {
            
        }
    }
}
