﻿using Devblog_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Interfaces
{
    public interface IBlogView
    {
        public IPost AddPost(string title, string reference, string weblog);

        public IPost AddPost(string title, string reference, string pros, string cons, short stars);

        public IPost AddPost(string title, string reference, string description, string image);

        public Post UpdatePost(Post post, string NewTitle, string NewReference);

        public void DeletePost(Guid id);

        public List<IPost> GetListOfPosts(PostType type);

        public void AddTag(Tag tag, IPost post);

        public void RemoveTag(Tag tag, IPost post);

        public void SoftDeletePost(Guid id);

        public void RestorePost(Guid id);

        public void SavePostsToFile();

        public List<Tag> LoadListOfTags();

        public void SetAuthor();

        public List<IPost> LoadListOfPosts();

        public IPost GetPostById(Guid id, List<IPost> posts);

        public Tag GetTagById(Guid id, List<Tag> tags);
    }
}