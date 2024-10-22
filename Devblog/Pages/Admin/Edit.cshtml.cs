using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography.Xml;

namespace Devblog.Pages.Admin
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IBlogView _blogView;
        private readonly IPersonRepo _personRepo;
        public List<IPost> Posts { get; set; }
        public IPost Post { get; set; }
        public Tag Tag { get; set; }
        public Person Person { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }

        public EditModel(IBlogView blogView, IPersonRepo personRepo)
        {
            _blogView = blogView;
            Posts = new List<IPost>();
            _personRepo = personRepo;
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
                Person = _personRepo.GetPersonById(Id);

                if (Person == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            Guid postId = Guid.Parse(Request.Form["PostId"]);
            string PostTypes = Request.Form["PostType"];
            string Title = Request.Form["Title"];
            string Reference = Request.Form["Reference"];

            if (PostTypes == "BlogPost")
            {
                string Weblog = Request.Form["Weblog"];
                var blogPost = _blogView.GetPostById(postId) as BlogPost;
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
                    var review = _blogView.GetPostById(postId) as Review;
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
                var project = _blogView.GetPostById(postId) as Project;
                if (project != null)
                {
                    _blogView.UpdatePost(project, Title, Reference, Description, Image);
                }
            }

            return RedirectToPage("/Admin/Index");
        }
    }
}