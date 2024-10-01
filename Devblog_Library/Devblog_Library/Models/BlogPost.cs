using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Models
{
    public class BlogPost : Post
    {
        public string Weblog { get; set; }

        public BlogPost(string title, string reference, PostType type, string weblog) : base(title, reference, PostType.BlogPost)
        {
            Weblog = weblog;
        }
    }
}