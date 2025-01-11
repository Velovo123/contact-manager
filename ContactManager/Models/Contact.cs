using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public bool Married { get; set; }

        [Required, Phone]
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
