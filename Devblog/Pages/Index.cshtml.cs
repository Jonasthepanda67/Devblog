using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Devblog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBlogView _blogView;
        private readonly ICommentRepo _commentRepo;

        public IndexModel(IBlogView blogView, ICommentRepo commentRepo)
        {
            _blogView = blogView;
            _commentRepo = commentRepo;
        }

        [BindProperty]
        public string Message { get; set; }

        public Post currentPost { get; set; }

        [BindProperty]
        public string UserName { get; set; }

        public List<IPost> Posts { get; set; } = new List<IPost>();

        public void OnGet()
        {
            Posts = _blogView.LoadListOfPosts()
                             .Where(post => post.Type == PostType.BlogPost || post.Type == PostType.Review)
                             .OrderByDescending(p => p.Date)
                             .ToList();
        }

        public IActionResult OnPostAddComment(Guid postId, string userName)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Message))
            {
                ModelState.AddModelError("Message", "Message is required.");
                return Page();
            }

            IPost post = _blogView.GetPostById(postId);

            post.Comments.Add(_commentRepo.CreateComment(postId, userName, Message));

            return RedirectToPage();
        }

        public void ShowModal()
        {
        }

        public IActionResult OnPostEditComment(Guid postId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Message))
            {
                ModelState.AddModelError("Message", "Message is required.");
                return Page();
            }

            IPost post = _blogView.GetPostById(postId);
            /*Comment comment = _commentRepo.GetComment(commentId);

            post.Comments.Find(c => c.Id == comment.Id).Message = Message;

            _commentRepo.UpdateComment(comment, Message);*/

            return RedirectToPage();
        }
    }
}