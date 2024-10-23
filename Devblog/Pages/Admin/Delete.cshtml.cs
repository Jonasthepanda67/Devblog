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
        private readonly IPersonRepo _personRepo;

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }

        public Tag Tag { get; set; }
        public Person Account { get; set; }

        public DeleteModel(IBlogView blogView, ITagRepo tagRepo, IPersonRepo personRepo)
        {
            _blogView = blogView;
            _tagRepo = tagRepo;
            _personRepo = personRepo;
        }

        public IActionResult OnGet()
        {
            if (Type == "Tag")
            {
                Tag = _blogView.GetTagById(Id);

                if (Tag == null)
                {
                    return NotFound();
                }
            }
            else if (Type == "Account")
            {
                Account = _personRepo.GetPersonById(Id);

                if (Account == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (Type == "Tag" & Tag != null)
            {
                _tagRepo.DeleteTag(Tag);
            }
            else if (Type == "Account" & Account != null)
            {
                _personRepo.DeletePerson(Account);
                return RedirectToPage("/Admin/Accounts");
            }

            return RedirectToPage("/Admin/Index");
        }
    }
}