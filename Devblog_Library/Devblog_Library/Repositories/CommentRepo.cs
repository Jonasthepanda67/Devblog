using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Repositories
{
    public class CommentRepo : ICommentRepo
    {
        private List<Comment> _comments = [];

        public Comment CreateComment(Guid postId, Person user, string message)
        {
            Comment comment = new Comment(user, message);
            _comments.Add(comment);
            return comment;
        }

        public void DeleteComment(Guid commentId)
        {
            _comments.Remove(GetComment(commentId));
        }

        public Comment GetComment(Guid commentId)
        {
            return _comments.Find(c => c.Id == commentId);
        }

        public string EditComment(Guid commentId, Person user, string message)
        {
            Comment currentComment = GetComment(commentId);
            if (currentComment.User != user)
            {
                return "You cannot edit another user's comment...";
            }
            else
            {
                currentComment.Message = message;
                return "Comment edit was succesful";
            }
        }
    }
}