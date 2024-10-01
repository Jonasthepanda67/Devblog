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

        public List<IPost> Projects = [];

        public void OnGet()
        {
            Projects = _blogView.GetListOfPosts(PostType.Project);
        }
    }
}