using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Admin
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IBlogView _blogView;
        private readonly ITagRepo _tagRepo;
        public Tag Tag { get; set; }

        public DeleteModel(IBlogView blogView, ITagRepo tagRepo)
        {
            _blogView = blogView;
            _tagRepo = tagRepo;
        }

        public IActionResult OnGet(Guid id)
        {
            Tag = _blogView.GetTagById(id);

            if (Tag == null)
            {
                return NotFound();
            }

            _tagRepo.DeleteTag(Tag);

            return Page();
        }
    }
}