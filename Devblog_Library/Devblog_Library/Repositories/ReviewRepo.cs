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
        public Review CreatePost(string title, string reference, string description, string content, short stars)
        {
            Review post = new Review(title, reference, PostType.Review, description, content, stars);
            return post;
        }
    }
}