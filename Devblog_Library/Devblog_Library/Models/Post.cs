using Devblog_Library.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Devblog_Library.Models
{
    public abstract class Post : IPost
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public PostType Type { get; init; }
        public TagList Tags { get; set; }
        public List<Comment> Comments { get; set; }
        public bool IsDeleted { get; set; }

        protected Post(string title, string reference, PostType type)
        {
            Id = Guid.NewGuid();
            Title = title;
            Reference = reference;
            Date = DateTime.Now;
            Type = type;
            Tags = new(Id);
            IsDeleted = false;
        }
    }
}