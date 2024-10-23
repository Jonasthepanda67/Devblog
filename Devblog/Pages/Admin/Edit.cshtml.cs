using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography.Xml;
using System.Security.Claims;

namespace Devblog.Pages.Admin
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBlogView _blogView;
        private readonly IPersonRepo _personRepo;
        private readonly ITagRepo _tagRepo;
        public string UserRole { get; private set; }
        public List<IPost> Posts { get; set; }
        public IPost Post { get; set; }
        public Tag Tag { get; set; }
        public Person Account { get; set; }

        [BindProperty]
        public string UserType { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }

        public EditModel(IHttpContextAccessor httpContextAccessor, IBlogView blogView, ITagRepo tagRepo, IPersonRepo personRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _blogView = blogView;
            Posts = new List<IPost>();
            _tagRepo = tagRepo;
            _personRepo = personRepo;

            UserType = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        }

        public IActionResult OnGet()
        {
            if (Type == "Post")
            {
                Posts = _blogView.LoadListOfPosts();

                Post = _blogView.GetPostById(Id);

                if (Post == null)
                {
                    return NotFound();
                }
            }
            else if (Type == "Tag")
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

        public IActionResult OnPostEditPost()
        {
            Guid postId = Guid.Parse(Request.Form["PostId"]);
            string PostTypes = Request.Form["PostType"];
            string Title = Request.Form["Title"];
            string Reference = Request.Form["Reference"];

            if (PostTypes == "BlogPost")
            {
                string Weblog = Request.Form["Weblog"];
                BlogPost blogPost = _blogView.GetPostById(postId) as BlogPost;
                if (blogPost != null)
                {
                    _blogView.UpdatePost(blogPost, Title, Reference, Weblog);
                }
            }
            else if (PostTypes == "Review")
            {
                string Pros = Request.Form["Pros"];
                string Cons = Request.Form["Cons"];
                if (short.TryParse(Request.Form["Stars"], out short Stars))
                {
                    Review review = _blogView.GetPostById(postId) as Review;
                    if (review != null)
                    {
                        _blogView.UpdatePost(review, Title, Reference, Pros, Cons, Stars);
                    }
                }
            }
            else if (PostTypes == "Project")
            {
                string Description = Request.Form["Description"];
                string Image = Request.Form["Image"];
                Project project = _blogView.GetPostById(postId) as Project;
                if (project != null)
                {
                    _blogView.UpdatePost(project, Title, Reference, Description, Image);
                }
            }

            return RedirectToPage("/Admin/Index");
        }

        public IActionResult OnPostEditTag()
        {
            Guid tagId = Guid.Parse(Request.Form["TagId"]);
            string name = Request.Form["TagName"];

            _tagRepo.UpdateTag(tagId, name);

            return RedirectToPage("/Admin/Index");
        }

        public IActionResult OnPostEditAccount()
        {
            Person person = _personRepo.GetPersonById(Guid.Parse(Request.Form["AccountId"]));
            string firstName = Request.Form["FirstName"];
            string lastName = Request.Form["LastName"];
            int age = Convert.ToInt32(Request.Form["Age"]);
            string mail = Request.Form["Mail"];
            string city = Request.Form["City"];
            string phoneNumber = Request.Form["PhoneNumber"];
            string password = Request.Form["Password"];
            string fullName = firstName + " " + lastName;
            if (string.IsNullOrEmpty(UserType))
            {
                UserType = "User";
            }

            _personRepo.UpdatePerson(person, firstName, lastName, fullName, age, mail, city, phoneNumber, password, UserType);

            return RedirectToPage("/Admin/Accounts");
        }
    }
}