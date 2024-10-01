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
        public List<Person> People { get; }

        public Person CreatePerson(Person person);

        public Person GetPersonDetails(string id);

        public void LoadListOfPeople();
    }
}