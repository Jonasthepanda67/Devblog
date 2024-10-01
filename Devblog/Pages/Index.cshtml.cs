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

        public List<Review> ReviewPosts { get; set; } = [];
        public List<BlogPost> BlogPosts { get; set; } = [];
        public List<IPost> Posts { get; set; } = [];

        public void OnGet()
        {
            Posts = _blogView.GetListOfPosts(PostType.BlogPost);
            foreach (IPost post in _blogView.GetListOfPosts(PostType.Review))
            {
                Posts = Posts.Append(post).ToList();
            }
            Posts = Posts.OrderBy(p => p.Date).ToList();

            foreach (BlogPost blogPost in Posts.OfType<BlogPost>())
            {
                BlogPosts.Add(blogPost);
            }
            BlogPosts = BlogPosts.OrderBy(p => p.Date).ToList();
            foreach (Review review in Posts.OfType<Review>())
            {
                ReviewPosts.Add(review);
            }
            ReviewPosts = ReviewPosts.OrderBy(p => p.Date).ToList();
        }
    }
}