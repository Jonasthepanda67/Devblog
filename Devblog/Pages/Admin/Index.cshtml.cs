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
        private readonly IBlogView _blogView;

        public List<IPost> Posts { get; set; }
        public List<Tag> Tags { get; set; }
        public string SelectedPostType { get; set; }

        public IndexModel(IBlogView blogView)
        {
            _blogView = blogView;
            Posts = new List<IPost>();
            Tags = new List<Tag>();
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
            _blogView.SoftDeletePost(id); // Soft delete the post by its ID

            return RedirectToPage(); // Redirect back to the index page
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
            if (post != null)
            {
                _blogView.DeletePost(id);
            }
            return RedirectToPage(); // Redirect back to the index page
        }
    }
}