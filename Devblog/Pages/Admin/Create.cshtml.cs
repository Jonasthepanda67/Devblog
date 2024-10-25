using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Devblog.Pages.Admin
{
    [Authorize(Roles = "Author")]
    public class CreateModel : PageModel
    {
        #region Properties

        private IWebHostEnvironment _environment;
        private readonly IBlogView _blogView;
        private readonly ITagRepo _tagRepo;

        public CreateModel(IWebHostEnvironment environment, IBlogView blogView, ITagRepo tagRepo)
        {
            _environment = environment;
            _blogView = blogView;
            _tagRepo = tagRepo;
        }

        private IPost NewPost { get; set; }

        [BindProperty]
        public List<string> SelectedTagIds { get; set; } = new List<string>();

        public List<Tag> Tags { get; set; } = new List<Tag>();

        [BindProperty]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter a Reference")]
        public string Reference { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public string Weblog { get; set; }

        [BindProperty]
        public string Pros { get; set; }

        [BindProperty]
        public string Cons { get; set; }

        [BindProperty]
        public short? Stars { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select a post type.")]
        public PostType? PostTypes { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please a Tag Name")]
        public string TagName { get; set; }

        public bool SliderIsChecked { get; set; } = false;

        #endregion Properties

        public void OnGet()
        {
            Tags = _blogView.LoadListOfTags();
        }

        public IActionResult OnPostCreatePost(string filePathForDb)
        {
            ModelState.Remove(nameof(TagName));
            if (PostTypes == null && SliderIsChecked)
            {
                ModelState.AddModelError("PostTypes", "Please select a post type.");
                return Page();
            }

            if (PostTypes == PostType.BlogPost)
            {
                ModelState.Remove(nameof(Pros));
                ModelState.Remove(nameof(Cons));
                ModelState.Remove(nameof(Stars));
                ModelState.Remove(nameof(Description));
            }
            else if (PostTypes == PostType.Review)
            {
                ModelState.Remove(nameof(Description));
                ModelState.Remove(nameof(Weblog));

                if (string.IsNullOrWhiteSpace(Pros))
                {
                    ModelState.AddModelError("Pros", "Pros are required for Review.");
                }
                if (string.IsNullOrWhiteSpace(Cons))
                {
                    ModelState.AddModelError("Cons", "Cons are required for Review.");
                }
                if (!Stars.HasValue)
                {
                    ModelState.AddModelError("Stars", "Star rating is required for Review.");
                }
            }
            else if (PostTypes == PostType.Project)
            {
                ModelState.Remove(nameof(Pros));
                ModelState.Remove(nameof(Cons));
                ModelState.Remove(nameof(Stars));
                ModelState.Remove(nameof(Weblog));

                if (string.IsNullOrWhiteSpace(Description))
                {
                    ModelState.AddModelError("Description", "Description is required for Project.");
                }
            }

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value.Errors;

                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            if (PostTypes == PostType.BlogPost)
            {
                NewPost = _blogView.AddPost(Title, Reference, Weblog);
            }
            else if (PostTypes == PostType.Review)
            {
                NewPost = _blogView.AddPost(Title, Reference, Pros, Cons, Stars.Value);
            }
            else if (PostTypes == PostType.Project)
            {
                NewPost = _blogView.AddPost(Title, Reference, Description, filePathForDb);
            }

            if (!string.IsNullOrEmpty(Request.Form["SelectedTagIds"]))
            {
                var selectedTagIdsArray = JsonConvert.DeserializeObject<List<string>>(Request.Form["SelectedTagIds"]);

                foreach (var tagIdStr in selectedTagIdsArray)
                {
                    if (Guid.TryParse(tagIdStr, out var tagId))
                    {
                        var tag = _blogView.GetTagById(tagId);
                        if (tag != null)
                        {
                            _blogView.AddTag(tag, NewPost);
                        }
                    }
                }
            }

            return RedirectToPage("/Admin/Index");
        }

        public IActionResult OnPostCreateTag()
        {
            ModelState.Remove(nameof(Title));
            ModelState.Remove(nameof(Reference));
            ModelState.Remove(nameof(Description));
            ModelState.Remove(nameof(Pros));
            ModelState.Remove(nameof(Cons));
            ModelState.Remove(nameof(Stars));
            ModelState.Remove(nameof(Weblog));
            ModelState.Remove(nameof(PostTypes));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrWhiteSpace(TagName))
            {
                ModelState.AddModelError("TagName", "Tag Name is required.");
                return Page();
            }

            _tagRepo.CreateTag(TagName);

            return RedirectToPage("/Admin/Index", new { type = "Tag" });
        }

        public async Task<IActionResult> OnPostUploadProjectFileAsync(IFormFile projectFile)
        {
            if (projectFile == null || projectFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid file.");
                return Page();
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Path.GetFileName(projectFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await projectFile.CopyToAsync(fileStream);
            }

            var fileExtension = Path.GetExtension(fileName);
            var filePathForDb = $@"\Uploads\{fileName}";

            OnPostCreatePost(filePathForDb);

            return RedirectToPage("/Admin/Index");
        }
    }
}