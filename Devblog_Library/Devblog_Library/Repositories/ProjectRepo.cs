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
        public Project CreatePost(string title, string reference, string description, string image, short stars = 0)
        {
            Project post = new Project(title, reference, PostType.Project, description, image);
            using (StreamWriter writer = new StreamWriter(@"C:\Users\U427797\OneDrive - Danfoss\Desktop\testfile.txt", append: true))
            {
                writer.WriteLine($"Project|{DateTime.Now.Date}|{post.Id}|{title}|{reference}|{description}|{image}|{post.IsDeleted}");
            }
            return post;
        }
    }
}