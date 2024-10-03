using Devblog_Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Admin
{
    public class PortfolioModel : PageModel
    {
        public readonly IBlogView _blogView;

        public PortfolioModel(IBlogView blogView)
        {
            _blogView = blogView;
        }

        public void OnGet()
        {
            _blogView.LoadListOfPosts();
        }
    }
}