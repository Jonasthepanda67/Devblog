using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Devblog.Pages
{
    public class LoginModel : PageModel
    {
        /*private readonly SqlConnection con;

        public AdminModel(IConfiguration configuration)
        {
            string conStr = configuration.GetConnectionString("MainConnection");
            con = new SqlConnection(conStr);
        }*/
        private readonly ILogin _login;
        private readonly IPersonRepo _personRepo;

        public LoginModel(ILogin login, IPersonRepo personRepo)
        {
            _login = login;
            _personRepo = personRepo;
        }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        private string PassWord { get; set; }

        public bool ShowModal { get; set; }

        //Activates when the form is submitted
        public async Task<IActionResult> OnPost(string returnUrl = null, string logout = null)
        {
            if (logout != null)
            {
                HttpContext.Session.Remove("User");
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = "/Login"; // Default to login page
                }
                return LocalRedirect(returnUrl);
            }
            else
            {
                UserName = Request.Form["Username"];
                PassWord = Request.Form["Password"];

                _personRepo.LoadListOfPeople();

                /*con.Open();
                SqlCommand cmd = new("SELECT Username FROM dbo.Login WHERE Username = @Username AND Password = @Password", con);
                cmd.Parameters.AddWithValue("@Username", UserName);
                cmd.Parameters.AddWithValue("@Password", PassWord);
                var Result = (string)cmd.ExecuteScalar();
                con.Close();*/

                bool Result = _login.CheckLogin(UserName, PassWord);

                if (Result)
                {
                    HttpContext.Session.SetString("User", UserName);
                    TempData["result"] = Result;
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "/"; // Default to home page
                    }
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ShowModal = true;
                    TempData["ModalMessage"] = "Invalid login attempt.";
                    return Page();
                }
            }
        }
    }
}