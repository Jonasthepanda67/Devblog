using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Repositories
{
    public class ReviewRepo : IRepo<Review>
    {
        public Review CreatePost(string title, string reference, string pros, string cons, short stars)
        {
            Review post = new Review(title, reference, PostType.Review, pros, cons, stars);
            using (StreamWriter writer = new StreamWriter(@"C:\Users\U427797\OneDrive - Danfoss\Desktop\testfile.txt", append: true))
            {
                writer.WriteLine($"Review|{DateTime.Now.Date}|{post.Id}|{title}|{reference}|{pros}|{cons}|{stars}|{post.IsDeleted}");
            }
            return post;
        }
    }
}