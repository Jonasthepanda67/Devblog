using Devblog_Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Devblog_Library.Models;

namespace Devblog.Pages.Admin
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IBlogView _blogView;

        public List<IPost> Posts { get; set; }

        public IndexModel(IBlogView blogView)
        {
            _blogView = blogView;
            Posts = new List<IPost>();
        }

        public void OnGet()
        {
            Posts = _blogView.LoadListOfPosts();
        }
    }
}