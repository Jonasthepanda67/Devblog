using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Models
{
    public class Tag
    {
        public Guid Id { get; set; } //fix when you use a database
        public string Name { get; set; }

        public Tag(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}