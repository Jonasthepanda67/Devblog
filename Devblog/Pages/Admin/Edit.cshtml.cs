using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Admin
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IBlogView _blogView;
        public List<IPost> Posts { get; set; }
        public IPost Post { get; set; }
        public List<Tag> Tags { get; set; }
        public Tag Tag { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }

        public EditModel(IBlogView blogView)
        {
            _blogView = blogView;
            Posts = new List<IPost>();
            Tags = new List<Tag>();
        }

        public IActionResult OnGet()
        {
            if (Type == "Post")
            {
                Posts = _blogView.LoadListOfPosts();

                Post = _blogView.GetPostById(Id, Posts);

                if (Post == null)
                {
                    return NotFound();
                }
            }
            else if (Type == "Tag")
            {
                Tags = _blogView.LoadListOfTags();

                Tag = _blogView.GetTagById(Id, Tags);
            }

            return Page();
        }
    }
}