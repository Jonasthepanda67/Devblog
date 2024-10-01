using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Repositories
{
    public class ProjectRepo : IRepo<Project>
    {
        public Project CreatePost(string title, string reference, string description, string content, short stars = 0)
        {
            Project post = new Project(title, reference, PostType.Project, description, content);
            using (StreamWriter writer = new StreamWriter(@"c:\testfile.txt", append: true))
            {
                writer.WriteLine($"{title}|{reference}|{description}|{content}");
            }
            return post;
        }
    }
}