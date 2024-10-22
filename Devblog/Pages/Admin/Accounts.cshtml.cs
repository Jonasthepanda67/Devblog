using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages.Admin
{
    public class AccountsModel : PageModel
    {
        private readonly IPersonRepo _personRepo;
        public List<Person> Accounts { get; set; }

        public AccountsModel(IPersonRepo personRepo)
        {
            _personRepo = personRepo;
        }

        public void OnGet()
        {
            Accounts = _personRepo.LoadListOfPeople();
        }
    }
}