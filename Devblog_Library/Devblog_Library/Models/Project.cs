using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Models
{
    public class Project : Post
    {
        public string Description { get; set; }
        public string Image { get; set; }

        public Project(string title, string reference, PostType type, string description, string image) : base(title, reference, PostType.Project)
        {
            Description = description;
            Image = image;
        }
    }
}