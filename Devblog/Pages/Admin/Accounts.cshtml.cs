using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Admin
{
    [Authorize(Roles = "Author")]
    public class AccountsModel : PageModel
    {
        #region Properties

        private readonly IPersonRepo _personRepo;
        public List<Person> Accounts { get; set; }

        #endregion Properties

        public AccountsModel(IPersonRepo personRepo)
        {
            _personRepo = personRepo;
        }

        public void OnGet()
        {
            Accounts = _personRepo.LoadListOfPeople();
        }

        public IActionResult OnPostDelete()
        {
            return RedirectToPage("/Admin/Accounts");
        }
    }
}