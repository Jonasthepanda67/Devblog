using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Interfaces
{
    public interface ITagRepo
    {
        public Tag CreateTag(string Name);

        public void DeleteTag(Tag tag);

        public void UpdateTag(Guid Id, string NewName);
    }
}