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

        public int Age { get; set; }
        public string Mail { get; set; }
        public string City { get; set; }
        public int PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsAuthor { get; set; }

        public Person(string firstName, string lastName, int age, string mail, string city, int phoneNumber, string password, bool isAuthor)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Mail = mail;
            City = city;
            PhoneNumber = phoneNumber;
            Password = password;
            IsAuthor = isAuthor;
        }
    }
}