using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.BLL
{
    public class Login : ILogin
    {
        private readonly IPersonRepo _personRepo;

        public Login(IPersonRepo personRepo)
        {
            _personRepo = personRepo;
        }

        public bool CheckLogin(string id, string password)
        {
            Person Author;
            if ((Author = _personRepo.GetPersonDetails(id)) is null)
            {
                return false;
            }
            if (id == Author.Id.ToString() && password == Author.Password) { return true; }
            else { return false; }
        }
    }
}