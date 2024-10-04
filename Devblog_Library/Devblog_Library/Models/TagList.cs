using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Models
{
    public class TagList : IEnumerable<Tag>
    {
        public List<Tag> ListOfTags { get; set; } = new List<Tag>();
        public Guid PostId { get; init; }

        public TagList(Guid postId)
        {
            PostId = postId;
        }

        // Implement GetEnumerator for IEnumerable<Tag>
        public IEnumerator<Tag> GetEnumerator()
        {
            return ListOfTags.GetEnumerator();
        }

        // Implement non-generic GetEnumerator
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}