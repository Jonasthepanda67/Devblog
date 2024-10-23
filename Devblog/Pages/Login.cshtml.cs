using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Devblog.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IPersonRepo _personRepo;

        public LoginModel(IConfiguration configuration, IPersonRepo personRepo)
        {
            this.configuration = configuration;
            _personRepo = personRepo;
        }

        [BindProperty, Required]
        public string UserName { get; set; }

        [BindProperty, Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty, Required]
        [MaxLength(80)]
        public string FirstName { get; set; }

        [BindProperty, Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [BindProperty, Required]
        public int Age { get; set; }

        [BindProperty]
        [Required]
        [MaxLength(120), EmailAddress]
        public string Mail { get; set; }

        [BindProperty, Required]
        [MaxLength(70)]
        public string City { get; set; }

        [BindProperty, Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+?\d{4,15}$", ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }

        [BindProperty, Required]
        [MinLength(12), DataType(DataType.Password)]
        public string ChosenPassword { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPost()
        {
            var user = configuration.GetSection("SiteUser").Get<SiteUser>();

            if (UserName == user.UserName)
            {
                var passwordHasher = new PasswordHasher<string>();
                if (passwordHasher.VerifyHashedPassword(null, user.Password, Password) == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, UserName) };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    var returnUrl = Request.Query["ReturnUrl"].FirstOrDefault() ?? "/admin/index";

                    return Redirect(returnUrl);
                }
            }

            Message = "Invalid attempt";
            return Page();
        }

        public IActionResult OnPostCreateAccount()
        {
            ModelState.Remove(nameof(UserName));
            ModelState.Remove(nameof(Password));

            if (Age < 18)
            {
                Message = "Age cannot be below 18";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _personRepo.CreatePerson(FirstName, LastName, Age, Mail, City, PhoneNumber, ChosenPassword);

            return RedirectToPage("/Login");
        }
    }
}