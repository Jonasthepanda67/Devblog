using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Interfaces
{
    public interface ICommentRepo
    {
        public Comment CreateComment(Guid postId, string UserName, string message);

        public List<Comment> LoadListOfComments();

        public void DeleteComment(Guid commentId);

        public Comment GetComment(Guid commentId);

        public void UpdateComment(Comment comment, string NewMessage);
    }
}