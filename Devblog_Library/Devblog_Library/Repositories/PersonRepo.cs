using Azure.Identity;
using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Devblog_Library.Repositories
{
    public class PersonRepo : IPersonRepo
    {
        #region Properties

        private readonly SqlConnection con;
        public List<Person> UserAccounts { get; } = [];

        private const int _saltSize = 16;
        private const int _keySize = 32;
        private const int _iterations = 50000;
        private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;
        private const char segmentDelimiter = ':';

        #endregion Properties

        public PersonRepo(IConfiguration configuration)
        {
            string conStr = configuration.GetConnectionString("MainConnection");
            con = new SqlConnection(conStr);
            UserAccounts = LoadListOfPeople();
        }

        public Person CreatePerson(string firstName, string lastName, string userName, int age, string mail, string city, string phoneNumber, string password)
        {
            Person person = new Person(firstName, lastName, userName, age, mail, city, phoneNumber, password);

            SqlCommand cmd = new("sp_CreateUserAccount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Person_Id", SqlDbType.NVarChar).Value = person.Id;
            cmd.Parameters.AddWithValue("@FName", SqlDbType.NVarChar).Value = firstName;
            cmd.Parameters.AddWithValue("@LName", SqlDbType.NVarChar).Value = lastName;
            cmd.Parameters.AddWithValue("@FullName", SqlDbType.NVarChar).Value = person.FullName;
            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@Age", age);
            cmd.Parameters.AddWithValue("@Mail", mail);
            cmd.Parameters.AddWithValue("@City", city);
            cmd.Parameters.AddWithValue("@Number", SqlDbType.NVarChar).Value = phoneNumber;
            cmd.Parameters.AddWithValue("@Password", SqlDbType.NVarChar).Value = password;
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

        public Person GetPersonById(Guid id)
        {
            List<Person> userAccounts = LoadListOfPeople();
            return userAccounts.Find(p => p.Id == id);
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
                            userName: reader["UserName"].ToString(),
                            age: Convert.ToInt32(reader["Age"]),
                            mail: reader["Mail"].ToString(),
                            city: reader["City"].ToString(),
                            phoneNumber: reader["Number"].ToString(),
                            password: reader["Password"].ToString()
                        )
                        {
                            Id = Guid.Parse(reader["Person_Id"].ToString()),
                            CreationDate = DateTime.Parse(reader["CreationDate"].ToString()),
                            UserType = reader["UserType"].ToString()
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

        public void DeletePerson(Person person)
        {
            SqlCommand cmd = new("sp_DeleteUserAccount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Person_Id", SqlDbType.NVarChar).Value = person.Id;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                //do error handling here
            }
            finally
            {
                con.Close();
            }
        }

        public void UpdatePerson(Person person, string NewFirstName, string NewLastName, string NewFullName, int NewAge, string NewMail, string NewCity, string NewPhoneNumber, string NewPassword, string NewUserType)
        {
            SqlCommand cmd = new("sp_UpdateUserAccount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Person_Id", SqlDbType.NVarChar).Value = person.Id;
            if (!string.IsNullOrEmpty(NewFirstName))
                cmd.Parameters.AddWithValue("@FName", NewFirstName);
            else
                cmd.Parameters.AddWithValue("@FName", person.FirstName);
            if (!string.IsNullOrEmpty(NewLastName))
                cmd.Parameters.AddWithValue("@LName", NewLastName);
            else
                cmd.Parameters.AddWithValue("@LName", person.LastName);
            if (!string.IsNullOrEmpty(NewFullName))
                cmd.Parameters.AddWithValue("@FullName", NewFullName);
            else
                cmd.Parameters.AddWithValue("@FullName", person.FullName);
            if (!string.IsNullOrEmpty(NewAge.ToString()))
                cmd.Parameters.AddWithValue("@Age", NewAge);
            else
                cmd.Parameters.AddWithValue("@Age", person.Age);
            if (!string.IsNullOrEmpty(NewMail))
                cmd.Parameters.AddWithValue("@Mail", NewMail);
            else
                cmd.Parameters.AddWithValue("@Mail", person.Mail);
            if (!string.IsNullOrEmpty(NewCity))
                cmd.Parameters.AddWithValue("@City", NewCity);
            else
                cmd.Parameters.AddWithValue("@City", person.City);
            if (!string.IsNullOrEmpty(NewPhoneNumber))
                cmd.Parameters.AddWithValue("@Number", NewPhoneNumber);
            else
                cmd.Parameters.AddWithValue("@Number", person.PhoneNumber);
            if (!string.IsNullOrEmpty(NewPassword))
                cmd.Parameters.AddWithValue("@Password", NewPassword);
            else
                cmd.Parameters.AddWithValue("@Password", person.Password);
            if (person.UserType != NewUserType)
            {
                cmd.Parameters.AddWithValue("@UserType", NewUserType);
            }

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                //sgdggdgdji
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public string HashPassword(string input)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                _iterations,
                _algorithm,
                _keySize
            );
            return string.Join(
                segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                _iterations,
                _algorithm
            );
        }

        public bool VerifyPassword(string input, string hashString)
        {
            string[] segments = hashString.Split(segmentDelimiter);
            byte[] hash = Convert.FromHexString(segments[0]);
            byte[] salt = Convert.FromHexString(segments[1]);
            int iterations = int.Parse(segments[2]);
            HashAlgorithmName algorithm = new HashAlgorithmName(segments[3]);
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                iterations,
                algorithm,
                hash.Length
            );
            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }
    }
}