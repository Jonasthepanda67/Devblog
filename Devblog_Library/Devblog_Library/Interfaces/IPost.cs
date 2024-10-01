﻿using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Interfaces
{
    public interface IPost
    {
        public Guid Id { get; init; }
        public string Title { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; init; }
        public PostType Type { get; init; }
        public TagList Tags { get; init; }
        public bool IsDeleted { get; set; }
    }
}