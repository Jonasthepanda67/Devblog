using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Admin
{
    public class PostDetailsModel : PageModel
    {
        private readonly IBlogView _blogView;
        public IPost Post { get; set; }

        public PostDetailsModel(IBlogView blogView)
        {
            _blogView = blogView;
        }

        public IActionResult OnGet(Guid id)
        {
            _blogView.LoadListOfPosts();
            Post = _blogView.GetPostById(id);

            if (Post == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}