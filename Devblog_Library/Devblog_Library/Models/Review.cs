using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Models
{
    public class Review : Post
    {
        public string Pros { get; set; }
        public string Cons { get; set; }
        public short Stars { get; set; }

        public Review(string title, string reference, PostType type, string pros, string cons, short stars) : base(title, reference, PostType.Review)
        {
            Pros = pros;
            Cons = cons;
            Stars = stars;
        }
    }
}