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

        public LoginModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [BindProperty]
        [Required]
        public string UserName { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

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

                    // Sign in user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Check if there's a return URL and redirect there, or default to /admin/index
                    var returnUrl = Request.Query["ReturnUrl"].FirstOrDefault() ?? "/admin/index";

                    return Redirect(returnUrl);
                }
            }

            // Invalid login attempt
            Message = "Invalid attempt";
            return Page();
        }
    }
}