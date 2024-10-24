using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using System.Runtime.Versioning;

namespace Devblog.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SqlConnection con;
        private readonly IConfiguration configuration;
        private readonly IPersonRepo _personRepo;

        public LoginModel(IConfiguration configuration, IPersonRepo personRepo)
        {
            this.configuration = configuration;
            _personRepo = personRepo;
            string conStr = configuration.GetConnectionString("MainConnection");
            con = new SqlConnection(conStr);
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

        [BindProperty, Required]
        [MaxLength(18)]
        public string ChosenUserName { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPost()
        {
            string query = "SELECT UserName, Password, UserType FROM PersonTable WHERE UserName = @username";

            SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@username", UserName);

            await con.OpenAsync();

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    string hashedPassword = reader["Password"].ToString();
                    string userType = reader["UserType"].ToString();

                    var passwordHasher = new PasswordHasher<string>();
                    if (passwordHasher.VerifyHashedPassword(null, hashedPassword, Password) == PasswordVerificationResult.Success)
                    {
                        // Create claims
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, UserName),
                            new Claim(ClaimTypes.Role, userType) // Add the role to claims
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Sign in the user
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        // Redirect to the specified or default page
                        var returnUrl = Request.Query["ReturnUrl"].FirstOrDefault() ?? "/admin/index";
                        return Redirect(returnUrl);
                    }
                }
            }

            // If we reach here, it means authentication failed
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

            _personRepo.CreatePerson(FirstName, LastName, ChosenUserName, Age, Mail, City, PhoneNumber, ChosenPassword);

            return RedirectToPage("/Login");
        }
    }
}