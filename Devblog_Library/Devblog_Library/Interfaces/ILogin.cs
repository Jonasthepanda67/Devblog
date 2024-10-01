using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Interfaces
{
    public interface ILogin
    {
        public bool CheckLogin(string username, string password);
    }
}