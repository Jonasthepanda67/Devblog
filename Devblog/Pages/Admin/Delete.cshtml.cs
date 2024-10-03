using Devblog_Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Admin
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IBlogView _blogView;
        public List<IPost> Posts { get; set; }
        public IPost Post { get; set; }

        public DeleteModel(IBlogView blogView)
        {
            _blogView = blogView;
            Posts = new List<IPost>();
        }

        public IActionResult OnGet(Guid id)
        {
            Posts = _blogView.LoadListOfPosts();

            Post = _blogView.GetPostById(id, Posts);

            if (Post == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}