using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Interfaces
{
    public interface IBlogView
    {
        public void AddPost(string title, string reference, string weblog);

        public void AddPost(string title, string reference, string pros, string cons, short stars);

        public void AddPost(string title, string reference, string description, string image);

        public Post UpdatePost(Post post, string NewTitle, string NewReference);

        public void DeletePost(Post post);

        public List<IPost> GetListOfPosts(PostType type);

        public void SetAuthor();

        public List<IPost> LoadListOfPosts();

        public IPost GetPostById(Guid id);
    }
}