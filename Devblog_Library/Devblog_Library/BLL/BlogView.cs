﻿using Devblog_Library.Interfaces;
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

        public List<IPost> LoadListOfPosts()
        {
            List<IPost> _posts = new List<IPost>();
            using (StreamReader reader = new StreamReader(@"C:\Users\U427797\OneDrive - Danfoss\Desktop\testfile.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split("|");

                    if (values[0] == "BlogPost")
                    {
                        BlogPost post = new BlogPost(values[2], values[3], PostType.BlogPost, values[4]);
                        post.Date = DateTime.Parse(values[1]);
                        _posts.Add(post);
                    }
                    else if (values[0] == "Review")
                    {
                        Review post = new Review(values[2], values[3], PostType.Review, values[4], values[5], short.Parse(values[6]));
                        post.Date = DateTime.Parse(values[1]);
                        _posts.Add(post);
                    }
                    else if (values[0] == "Project")
                    {
                        Project post = new Project(values[2], values[3], PostType.Project, values[4], values[5]);
                        post.Date = DateTime.Parse(values[1]);
                        _posts.Add(post);
                    }
                }
            }
            return _posts;
        }

        public void RemoveTag(Tag tag, Post post)
        {
            post.Tags.ListOfTags.Remove(tag);
        }

        public void AddTag(Tag tag, Post post)
        {
            post.Tags.ListOfTags.Add(tag);
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

        public IPost GetPostById(Guid id)
        {
            return _posts.FirstOrDefault(post => post.Id == id);
        }
    }
}