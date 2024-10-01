using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Interfaces
{
    public interface IRepo<T> where T : Post
    {
        public T CreatePost(string title, string reference, string description, string content = "", short stars = 0);
    }
}