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
            Posts = _blogView.LoadListOfPosts();
            foreach (IPost post in Posts)
            {
                if (post.Type == PostType.Project)
                {
                    Posts.Remove(post);
                }
            }
            Posts = Posts.OrderBy(p => p.Date).ToList();
        }
    }
}