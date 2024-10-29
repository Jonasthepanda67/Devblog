using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Devblog.Pages.Admin
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Properties

        private IWebHostEnvironment _environment;
        private readonly IBlogView _blogView;

        public List<IPost> Posts { get; set; }
        public List<Tag> Tags { get; set; }
        public string SelectedPostType { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }

        #endregion Properties

        public IndexModel(IBlogView blogView, IWebHostEnvironment environment)
        {
            _blogView = blogView;
            Posts = new();
            Tags = new();
            _environment = environment;
        }

        public void OnGet(string postType = "All")
        {
            LoadPosts(postType);
        }

        private void LoadPosts(string postType)
        {
            Posts = _blogView.LoadListOfPosts();
            Tags = _blogView.LoadListOfTags();

            if (postType != "All")
            {
                Posts = Posts.Where(p => p.Type.ToString() == postType && !p.IsDeleted).ToList();
            }
        }

        public IActionResult OnPostSoftDeleteAsync(Guid id)
        {
            // Soft delete the post by its ID
            _blogView.SoftDeletePost(id);

            return RedirectToPage();
        }

        public IActionResult OnPostRestore(Guid id)
        {
            // Restore the post by its ID
            _blogView.RestorePost(id); // Restore the post
            return RedirectToPage(); // Redirect back to the index page
        }

        public IActionResult OnPostPermanentDelete(Guid id)
        {
            // Find the post and remove it from the list
            IPost post = _blogView.GetPostById(id);
            if (post.Type == PostType.Project)
            {
                Project project = (Project)post;
                string file = _environment.WebRootPath + project.Image;
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            if (post != null)
            {
                _blogView.DeletePost(id);
            }
            return RedirectToPage(); // Redirect back to the index page
        }
    }
}