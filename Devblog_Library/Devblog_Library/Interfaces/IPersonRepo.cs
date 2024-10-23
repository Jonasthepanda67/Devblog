using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Interfaces
{
    public interface IPersonRepo
    {
        public List<Person> UserAccounts { get; }

        public Person CreatePerson(string firstName, string lastName, int age, string mail, string city, string phoneNumber, string password);

        public Person GetPersonById(Guid id);

        public List<Person> LoadListOfPeople();

        public void DeletePerson(Person person);

        public void UpdatePerson(Person person, string NewFirstName, string NewLastName, string NewFullName, int NewAge, string NewMail, string NewCity, string NewPhoneNumber, string NewPassword);
    }
}