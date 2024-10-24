using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Models
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PostId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }

        public Comment(Guid postId, string userName, string message)
        {
            PostId = postId;
            UserName = userName;
            Message = message;
            CreationDate = DateTime.Now;
        }
    }
}