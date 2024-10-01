using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Repositories
{
    public class PersonRepo : IPersonRepo
    {
        private static string _fileName = @"C:\Users\U427797\source\repos\Devblog-Portfolio\bin\Debug\net8.0\Author.txt";
        public List<Person> People { get; } = [];

        public Person CreatePerson(Person person)
        {
            using (StreamWriter sw = new StreamWriter(_fileName, append: true))
            {
                sw.WriteLine($"{person.Id},{person.FirstName},{person.LastName},{person.Age},{person.Mail},{person.City}.{person.PhoneNumber},{person.Password},false");
                People.Add(person);
            }

            return person;
        }

        public Person? GetPersonDetails(string id)
        {
            Guid.TryParse(id, out var personId);
            return People.Find(p => p.Id == personId);
        }

        //fix this method
        public void LoadListOfPeople()
        {
            Person person;
            string line;
            StreamReader file = new StreamReader(_fileName);
            while (!(file.EndOfStream))
            {
                line = file.ReadLine();
                if (line != string.Empty)
                {
                    string[] splittetStr = line.Split(",");
                    person = new Person(splittetStr[1], splittetStr[2], int.Parse(splittetStr[3]), splittetStr[4], splittetStr[5], int.Parse(splittetStr[6]), splittetStr[7], bool.Parse(splittetStr[8]));
                    People.Add(person);
                }
            }
            file.Close();
        }
    }
}