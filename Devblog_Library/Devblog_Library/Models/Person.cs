using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Models
{
    public class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => FirstName + " " + LastName;

        public string UserName { get; set; }
        public int Age { get; set; }
        public string Mail { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string UserType { get; set; } = "User";

        public Person(string firstName, string lastName, string userName, int age, string mail, string city, string phoneNumber, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Age = age;
            Mail = mail;
            City = city;
            PhoneNumber = phoneNumber;
            Password = password;
        }
    }
}