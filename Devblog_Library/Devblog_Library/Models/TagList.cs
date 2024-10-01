using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Models
{
    public class TagList
    {
        public List<Tag> ListOfTags = new List<Tag>();
        public Guid PostId { get; init; }

        public TagList(Guid postId)
        {
            PostId = postId;
            ListOfTags = [];
        }
    }
}