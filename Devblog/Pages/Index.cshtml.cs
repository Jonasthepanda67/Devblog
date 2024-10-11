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

        public List<IPost> Posts { get; set; } = new List<IPost>();

        public void OnGet()
        {
            // Fetch all posts (BlogPosts and Reviews only)
            Posts = _blogView.LoadListOfPosts()
                             .Where(post => post.Type == PostType.BlogPost || post.Type == PostType.Review)
                             .OrderByDescending(p => p.Date)
                             .ToList();
        }
    }
}