using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages
{
    public class PortfolioModel : PageModel
    {
        private readonly IBlogView _blogView;

        public PortfolioModel(IBlogView blogView)
        {
            _blogView = blogView;
        }

        public List<Project> Projects { get; set; } = new List<Project>();

        public void OnGet()
        {
            // Fetch all project posts
            Projects = _blogView.LoadListOfPosts()
                                .Where(post => post.Type == PostType.Project)
                                .Cast<Project>()
                                .ToList();
        }
    }
}