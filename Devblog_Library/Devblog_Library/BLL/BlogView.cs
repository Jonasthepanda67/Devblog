using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.BLL
{
    public class BlogView : IBlogView
    {
        public Person Author { get; set; }
        private List<IPost> _posts = [];
        private readonly IRepo<BlogPost> _blogPostRepo;
        private readonly IRepo<Project> _projectRepo;
        private readonly IRepo<Review> _reviewRepo;
        private readonly IPersonRepo _personRepo;

        public BlogView(IRepo<BlogPost> blogPostRepo, IRepo<Project> projectRepo, IRepo<Review> reviewRepo)
        {
            _blogPostRepo = blogPostRepo;
            _projectRepo = projectRepo;
            _reviewRepo = reviewRepo;
            _posts = LoadListOfPosts();
        }

        public void AddPost(string title, string reference, string weblog)
        {
            _posts.Add(_blogPostRepo.CreatePost(title, reference, weblog));
        }

        public void AddPost(string title, string reference, string pros, string cons, short stars)
        {
            _posts.Add(_reviewRepo.CreatePost(title, reference, pros, cons, stars));
        }

        public void AddPost(string title, string reference, string description, string image)
        {
            _posts.Add(_projectRepo.CreatePost(title, reference, description, image));
        }

        public Post UpdatePost(Post post, string NewTitle, string NewReference)
        {
            if (!string.IsNullOrEmpty(NewTitle))
            {
                post.Title = NewTitle;
            }
            if (!string.IsNullOrEmpty(NewReference))
            {
                post.Reference = NewReference;
            }

            return post;
        }

        public void DeletePost(Post post) //if you have the time then make a soft delete function and then a list over soft deleted items that can then be hard deleted or undone
        {
            post.IsDeleted = true;
        }

        public List<IPost> GetListOfPosts(PostType type)
        {
            List<IPost> viewList = new List<IPost>();
            foreach (IPost post in _posts)
            {
                if (post.Type == type)
                {
                    viewList.Add(post);
                }
            }

            return viewList;
        }

        public void SoftDeletePost(Guid id)
        {
            var post = GetPostById(id, _posts);
            if (post != null)
            {
                post.IsDeleted = true;
                SavePostsToFile(); // Save changes to file
            }
        }

        public void RestorePost(Guid id)
        {
            var post = GetPostById(id, _posts);
            if (post != null)
            {
                post.IsDeleted = false;
                SavePostsToFile(); // Save changes to file
            }
        }

        public void SavePostsToFile()
        {
            using (StreamWriter writer = new StreamWriter(@"C:\Users\U427797\OneDrive - Danfoss\Desktop\testfile.txt", append: false))
            {
                foreach (var post in _posts)
                {
                    if (post is BlogPost blogPost)
                    {
                        writer.WriteLine($"BlogPost|{blogPost.Date}|{blogPost.Id}|{blogPost.Title}|{blogPost.Reference}|{blogPost.Weblog}|{(post.IsDeleted ? "true" : "false")}");
                    }
                    else if (post is Review review)
                    {
                        writer.WriteLine($"Review|{review.Date}|{review.Id}|{review.Title}|{review.Reference}|{review.Pros}|{review.Cons}|{review.Stars}|{(post.IsDeleted ? "true" : "false")}");
                    }
                    else if (post is Project project)
                    {
                        writer.WriteLine($"Project|{project.Date}|{project.Id}|{project.Title}|{project.Reference}|{project.Description}|{project.Image}|{(post.IsDeleted ? "true" : "false")}");
                    }
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            // Assume you have a method to write the posts back to the file.
            using (StreamWriter writer = new StreamWriter(@"C:\Users\U427797\OneDrive - Danfoss\Desktop\testfile.txt", false)) // Overwrite the file
            {
                foreach (var post in _posts)
                {
                    // Write each post based on its type and include the IsDeleted status
                    if (post is BlogPost blogPost)
                    {
                        await writer.WriteLineAsync($"BlogPost|{blogPost.Date}|{blogPost.Id}|{blogPost.Title}|{blogPost.Reference}|{blogPost.Weblog}|{blogPost.IsDeleted}");
                    }
                    else if (post is Review reviewPost)
                    {
                        await writer.WriteLineAsync($"Review|{reviewPost.Date}|{reviewPost.Id}|{reviewPost.Title}|{reviewPost.Reference}|{reviewPost.Pros}|{reviewPost.Cons}|{reviewPost.Stars}|{reviewPost.IsDeleted}");
                    }
                    else if (post is Project projectPost)
                    {
                        await writer.WriteLineAsync($"Project|{projectPost.Date}|{projectPost.Id}|{projectPost.Title}|{projectPost.Reference}|{projectPost.Description}|{projectPost.Image}|{projectPost.IsDeleted}");
                    }
                }
            }
        }

        public List<IPost> LoadListOfPosts()
        {
            List<IPost> posts = new List<IPost>();
            using (StreamReader reader = new StreamReader(@"C:\Users\U427797\OneDrive - Danfoss\Desktop\testfile.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split("|");

                    if (values.Length < 7) // Not enough fields, skip the line
                    {
                        continue;
                    }

                    // Check for IsDeleted property (last field)
                    bool isDeleted = bool.Parse(values[^1]); // Using C# 8.0 index from end

                    // Parse based on the post type
                    if (values[0] == "BlogPost")
                    {
                        BlogPost post = new BlogPost(values[3], values[4], PostType.BlogPost, values[5])
                        {
                            Date = DateTime.Parse(values[1]),
                            Id = Guid.Parse(values[2]),
                            IsDeleted = isDeleted // Set IsDeleted from the file
                        };
                        posts.Add(post);
                    }
                    else if (values[0] == "Review")
                    {
                        Review post = new Review(values[3], values[4], PostType.Review, values[5], values[6], short.Parse(values[7]))
                        {
                            Date = DateTime.Parse(values[1]),
                            Id = Guid.Parse(values[2]),
                            IsDeleted = isDeleted // Set IsDeleted from the file
                        };
                        posts.Add(post);
                    }
                    else if (values[0] == "Project")
                    {
                        // Adjusting the Project parsing logic accordingly
                        Project post = new Project(values[3], values[4], PostType.Project, values[5], values[6])
                        {
                            Date = DateTime.Parse(values[1]),
                            Id = Guid.Parse(values[2]),
                            IsDeleted = isDeleted // Set IsDeleted from the file
                        };
                        posts.Add(post);
                    }
                }
            }
            return posts;
        }

        public void RemoveTag(Tag tag, Post post)
        {
            post.Tags.ListOfTags.Remove(tag);
        }

        public void AddTag(Tag tag, Post post)
        {
            post.Tags.ListOfTags.Add(tag);
        }

        public List<Tag> LoadListOfTags()
        {
            List<Tag> tags = new List<Tag>();
            using (StreamReader reader = new StreamReader(@"C:\Users\U427797\OneDrive - Danfoss\Desktop\Tags.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split("|");
                    Tag currentTag = new Tag(values[1]);
                    currentTag.Id = Guid.Parse(values[0]);
                    tags.Add(currentTag);
                }
            }
            return tags;
        }

        public void SetAuthor()
        {
            Author = _personRepo.People.Find(p => p.IsAuthor == true);
            if (Author is null)
            {
                _personRepo.LoadListOfPeople();
                SetAuthor();
            }
        }

        public IPost GetPostById(Guid id, List<IPost> posts)
        {
            return posts.Find(post => post.Id == id);
        }

        public Tag GetTagById(Guid id, List<Tag> tags)
        {
            return tags.Find(tag => tag.Id == id);
        }
    }
}