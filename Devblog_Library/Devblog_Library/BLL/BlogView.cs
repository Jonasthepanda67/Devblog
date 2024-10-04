﻿using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Devblog_Library.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Devblog_Library.BLL
{
    public class BlogView : IBlogView
    {
        public Person Author { get; set; }
        private List<IPost> _posts = [];
        private List<Tag> _tags = [];
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
            _tags = LoadListOfTags();
        }

        public IPost AddPost(string title, string reference, string weblog)
        {
            IPost post = _blogPostRepo.CreatePost(title, reference, weblog);
            _posts.Add(post);
            return post;
        }

        public IPost AddPost(string title, string reference, string pros, string cons, short stars)
        {
            IPost post = _reviewRepo.CreatePost(title, reference, pros, cons, stars);
            _posts.Add(post);
            return post;
        }

        public IPost AddPost(string title, string reference, string description, string image)
        {
            IPost post = _projectRepo.CreatePost(title, reference, description, image);
            _posts.Add(post);
            return post;
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

        public void DeletePost(Guid id)
        {
            string line = null;
            string lineToDelete = id.ToString();

            // Remove the post from the in-memory list
            _posts.Remove(_posts.Find(post => post.Id == id));

            // File paths
            string originalFilePath = @"C:\Users\U427797\OneDrive - Danfoss\Desktop\testfile.txt";
            string tempFilePath = @"C:\Users\U427797\OneDrive - Danfoss\Desktop\tempfile.txt";

            // Using a temporary file to avoid simultaneous access to the same file
            using (StreamReader reader = new StreamReader(originalFilePath))
            {
                using (StreamWriter writer = new StreamWriter(tempFilePath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Write lines to the temporary file except for the line to delete
                        if (String.Compare(line, lineToDelete) == 0)
                            continue;

                        writer.WriteLine(line);
                    }
                }
            }

            // Replace the original file with the temp file
            System.IO.File.Delete(originalFilePath);  // Delete the original file
            System.IO.File.Move(tempFilePath, originalFilePath);  // Rename temp file to original
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
                SavePostsToFile();
            }
        }

        public void RestorePost(Guid id)
        {
            var post = GetPostById(id, _posts);
            if (post != null)
            {
                post.IsDeleted = false;
                SavePostsToFile();
            }
        }

        public void SavePostsToFile()
        {
            using (StreamWriter writer = new StreamWriter(@"C:\Users\U427797\OneDrive - Danfoss\Desktop\testfile.txt", append: false))
            {
                foreach (var post in _posts)
                {
                    string tags = post.Tags != null && post.Tags.Any()
                        ? string.Join(",", post.Tags.Select(t => t.Id))  // Store tags as comma-separated Ids
                        : "NoTags";  // If no tags, store a placeholder

                    if (post is BlogPost blogPost)
                    {
                        writer.WriteLine($"BlogPost|{blogPost.Date}|{blogPost.Id}|{blogPost.Title}|{blogPost.Reference}|{blogPost.Weblog}|{(post.IsDeleted ? "true" : "false")}|{tags}");
                    }
                    else if (post is Review review)
                    {
                        writer.WriteLine($"Review|{review.Date}|{review.Id}|{review.Title}|{review.Reference}|{review.Pros}|{review.Cons}|{review.Stars}|{(post.IsDeleted ? "true" : "false")}|{tags}");
                    }
                    else if (post is Project project)
                    {
                        writer.WriteLine($"Project|{project.Date}|{project.Id}|{project.Title}|{project.Reference}|{project.Description}|{project.Image}|{(post.IsDeleted ? "true" : "false")}|{tags}");
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

                    if (values.Length < 8) // Adjusted for additional Tags field
                    {
                        continue;
                    }

                    // Check for IsDeleted property (second-last field)
                    bool isDeleted = bool.Parse(values[^2]); // Adjusting index due to tags field

                    // Parse the tags (last field)
                    List<Tag> postTags = new List<Tag>();
                    if (values[^1] != "NoTags")
                    {
                        var tagIds = values[^1].Split(',').Select(id => Guid.Parse(id)).ToList();
                        foreach (var tagId in tagIds)
                        {
                            var tag = GetTagById(tagId, _tags); // Get each tag by ID
                            if (tag != null)
                            {
                                postTags.Add(tag);  // Add valid tags to the post
                            }
                        }
                    }

                    // Parse based on the post type
                    if (values[0] == "BlogPost")
                    {
                        BlogPost post = new BlogPost(values[3], values[4], PostType.BlogPost, values[5])
                        {
                            Date = DateTime.Parse(values[1]),
                            Id = Guid.Parse(values[2]),
                            IsDeleted = isDeleted,
                            //Tags = postTags // Assign the tags to the post
                        };
                        foreach (Tag tag in postTags)
                        {
                            post.Tags.ListOfTags.Add(tag);
                        }
                        posts.Add(post);
                    }
                    else if (values[0] == "Review")
                    {
                        Review post = new Review(values[3], values[4], PostType.Review, values[5], values[6], short.Parse(values[7]))
                        {
                            Date = DateTime.Parse(values[1]),
                            Id = Guid.Parse(values[2]),
                            IsDeleted = isDeleted,
                            //Tags = postTags // Assign the tags to the post
                        };
                        foreach (Tag tag in postTags)
                        {
                            post.Tags.ListOfTags.Add(tag);
                        }
                        posts.Add(post);
                    }
                    else if (values[0] == "Project")
                    {
                        Project post = new Project(values[3], values[4], PostType.Project, values[5], values[6])
                        {
                            Date = DateTime.Parse(values[1]),
                            Id = Guid.Parse(values[2]),
                            IsDeleted = isDeleted,
                            //Tags = postTags // Assign the tags to the post
                        };
                        foreach (Tag tag in postTags)
                        {
                            post.Tags.ListOfTags.Add(tag);
                        }
                        posts.Add(post);
                    }
                }
            }
            return posts;
        }

        public void RemoveTag(Tag tag, IPost post)
        {
            post.Tags.ListOfTags.Remove(tag);
        }

        public void AddTag(Tag tag, IPost post)
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