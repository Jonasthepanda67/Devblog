using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Devblog_Library.Repositories
{
    public class PersonRepo : IPersonRepo
    {
        private readonly SqlConnection con;
        public List<Person> UserAccounts { get; } = [];

        public PersonRepo(IConfiguration configuration)
        {
            string conStr = configuration.GetConnectionString("MainConnection");
            con = new SqlConnection(conStr);
            UserAccounts = LoadListOfPeople();
        }

        public Person CreatePerson(string firstName, string lastName, int age, string mail, string city, int phoneNumber, string password)
        {
            Person person = new Person(firstName, lastName, age, mail, city, phoneNumber, password, false);

            SqlCommand cmd = new("sp_CreateUserAccount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = person.Id;
            cmd.Parameters.AddWithValue("@FName", firstName);
            cmd.Parameters.AddWithValue("@LName", lastName);
            cmd.Parameters.AddWithValue("@FullName", person.FullName);
            cmd.Parameters.AddWithValue("@Age", age);
            cmd.Parameters.AddWithValue("@Mail", mail);
            cmd.Parameters.AddWithValue("@City", city);
            cmd.Parameters.AddWithValue("@Number", phoneNumber);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@CreationDate", person.CreationDate);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                //Do stuff here
            }
            finally
            {
                con.Close();
            }

            return person;
        }

        public Person? GetPersonDetails(string id)
        {
            Guid.TryParse(id, out var personId);
            return UserAccounts.Find(p => p.Id == personId);
        }

        public List<Person> LoadListOfPeople()
        {
            List<Person> userAccounts = new List<Person>();

            string sqlCommand = "SELECT * FROM PersonTable";

            SqlCommand cmd = new SqlCommand(sqlCommand, con);

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Person person = new Person(
                            firstName: reader["FName"].ToString(),
                            lastName: reader["LName"].ToString(),
                            age: Convert.ToInt32(reader["Age"]),
                            mail: reader["Mail"].ToString(),
                            city: reader["City"].ToString(),
                            phoneNumber: Convert.ToInt32(reader["Number"]),
                            password: reader["Password"].ToString(),
                            isAuthor: reader.GetBoolean(reader.GetOrdinal("IsAuthor"))
                        )
                        {
                            Id = Guid.Parse(reader["Person_Id"].ToString()),
                            CreationDate = DateTime.Parse(reader["CreationDate"].ToString())
                        };
                        userAccounts.Add(person);
                    }
                }
            }
            catch (SqlException e)
            {
                //do stuff here
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return userAccounts;
        }
    }
}