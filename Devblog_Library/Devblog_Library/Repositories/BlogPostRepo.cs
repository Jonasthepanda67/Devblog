using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Repositories
{
    public class BlogPostRepo : IRepo<BlogPost>
    {
        public BlogPost CreatePost(string title, string reference, string description, string content = "", short stars = 0)
        {
            BlogPost post = new BlogPost(title, reference, PostType.BlogPost, description);
            return post;
        }
    }
}