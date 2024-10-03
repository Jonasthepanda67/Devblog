using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Devblog_Library.Repositories
{
    public class TagRepo : ITagRepo
    {
        private List<Tag> _tags = [];

        public Tag CreateTag(string Name)
        {
            Tag tag = new Tag(Name);
            using (StreamWriter writer = new StreamWriter(@"C:\Users\U427797\OneDrive - Danfoss\Desktop\Tags.txt", append: true))
            {
                writer.WriteLine($"{tag.Id}|{Name}");
            }
            _tags.Add(tag);
            return tag;
        }
    }
}