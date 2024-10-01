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
        public Person User { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public Comment(Person user, string message)
        {
            User = user;
            Message = message;
            Date = DateTime.Now;
        }
    }
}