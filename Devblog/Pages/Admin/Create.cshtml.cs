using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Devblog.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly IBlogView _blogView;

        public CreateModel(IBlogView blogView)
        {
            _blogView = blogView;
        }

        [BindProperty]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter a Reference")]
        public string Reference { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public string Pros { get; set; }

        [BindProperty]
        public string Cons { get; set; }

        [BindProperty]
        public short? Stars { get; set; }

        [BindProperty]
        public string Image { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select a post type.")]
        public PostType? PostTypes { get; set; }

        public IActionResult OnPostCreatePost()
        {
            if (PostTypes == null)
            {
                ModelState.AddModelError("PostTypes", "Please select a post type.");
                return Page();
            }

            if (PostTypes == PostType.BlogPost)
            {
                ModelState.Remove(nameof(Pros));
                ModelState.Remove(nameof(Cons));
                ModelState.Remove(nameof(Stars));
                ModelState.Remove(nameof(Image));
            }
            else if (PostTypes == PostType.Review)
            {
                ModelState.Remove(nameof(Description));
                ModelState.Remove(nameof(Image));
            }
            else if (PostTypes == PostType.Project)
            {
                ModelState.Remove(nameof(Pros));
                ModelState.Remove(nameof(Cons));
                ModelState.Remove(nameof(Stars));
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (PostTypes == PostType.BlogPost && string.IsNullOrWhiteSpace(Description))
            {
                ModelState.AddModelError("Description", "Description is required for BlogPost.");
                return Page();
            }

            if (PostTypes == PostType.Review)
            {
                if (string.IsNullOrWhiteSpace(Pros)) ModelState.AddModelError("Pros", "Pros are required.");
                if (string.IsNullOrWhiteSpace(Cons)) ModelState.AddModelError("Cons", "Cons are required.");
                if (!Stars.HasValue) ModelState.AddModelError("Stars", "Stars rating is required.");
                if (!ModelState.IsValid) return Page();
            }

            if (PostTypes == PostType.Project && (string.IsNullOrWhiteSpace(Description) || string.IsNullOrWhiteSpace(Image)))
            {
                ModelState.AddModelError("Description", "Description is required for Project.");
                ModelState.AddModelError("Image", "Image is required for Project.");
                return Page();
            }

            if (PostTypes == PostType.BlogPost)
            {
                _blogView.AddPost(Title, Reference, Description);
            }
            else if (PostTypes == PostType.Review)
            {
                _blogView.AddPost(Title, Reference, Pros, Cons, Stars.Value);
            }
            else if (PostTypes == PostType.Project)
            {
                _blogView.AddPost(Title, Reference, Description, Image);
            }

            return RedirectToPage("/Admin/Index");
        }
    }
}