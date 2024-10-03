using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Devblog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBlogView _blogView;

        public IndexModel(IBlogView blogView)
        {
            _blogView = blogView;
        }

        public List<IPost> Posts { get; set; } = [];

        public void OnGet()
        {
            // Load the list of posts and exclude Project type directly using LINQ
            Posts = _blogView.LoadListOfPosts()
                            .Where(post => post.Type != PostType.Project)
                            .OrderByDescending(p => p.Date) // Order by date (newest first)
                            .ToList();
        }
    }
}