using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Devblog_Library.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Devblog_Library.BLL
{
    public class BlogView : IBlogView
    {
        private readonly SqlConnection con;
        private SqlCommand cmd;

        public Person Author { get; set; }
        private List<IPost> _posts = [];
        private List<Tag> _tags = [];
        private readonly IRepo<BlogPost> _blogPostRepo;
        private readonly IRepo<Project> _projectRepo;
        private readonly IRepo<Review> _reviewRepo;
        private readonly IPersonRepo _personRepo;

        public BlogView(IConfiguration configuration, IRepo<BlogPost> blogPostRepo, IRepo<Project> projectRepo, IRepo<Review> reviewRepo)
        {
            string conStr = configuration.GetConnectionString("MainConnection");
            con = new SqlConnection(conStr);
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

        public IPost UpdatePost(BlogPost post, string NewTitle, string NewReference, string NewWeblog)
        {
            cmd = new("sp_UpdateBlogPost", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = post.Id;
            if (!string.IsNullOrEmpty(NewTitle))
                cmd.Parameters.AddWithValue("@Title", NewTitle);
            else
                cmd.Parameters.AddWithValue("@Title", post.Title);
            if (!string.IsNullOrEmpty(NewReference))
                cmd.Parameters.AddWithValue("@Reference", NewReference);
            else
                cmd.Parameters.AddWithValue("@Reference", post.Reference);
            if (!string.IsNullOrEmpty(NewWeblog))
                cmd.Parameters.AddWithValue("@Weblog", NewWeblog);
            else
                cmd.Parameters.AddWithValue("@Weblog", post.Weblog);
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                int affectedRows = cmd.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    if (!string.IsNullOrEmpty(NewTitle))
                        post.Title = NewTitle;
                    if (!string.IsNullOrEmpty(NewReference))
                        post.Reference = NewReference;
                    if (!string.IsNullOrEmpty(NewWeblog))
                        post.Weblog = NewWeblog;
                }
            }
            catch (SqlException e)
            {
                // sidisdi
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return post;
        }

        public IPost UpdatePost(Review post, string NewTitle, string NewReference, string NewPros, string NewCons, short NewStars)
        {
            cmd = new("sp_UpdateReview", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = post.Id;
            if (!string.IsNullOrEmpty(NewTitle))
                cmd.Parameters.AddWithValue("@Title", NewTitle);
            else
                cmd.Parameters.AddWithValue("@Title", post.Title);
            if (!string.IsNullOrEmpty(NewReference))
                cmd.Parameters.AddWithValue("@Reference", NewReference);
            else
                cmd.Parameters.AddWithValue("@Reference", post.Reference);
            if (!string.IsNullOrEmpty(NewPros))
                cmd.Parameters.AddWithValue("@Pros", NewPros);
            else
                cmd.Parameters.AddWithValue("@Pros", post.Pros);
            if (!string.IsNullOrEmpty(NewCons))
                cmd.Parameters.AddWithValue("@Cons", NewCons);
            else
                cmd.Parameters.AddWithValue("@Cons", post.Cons);
            if (NewStars != null)
                cmd.Parameters.AddWithValue("@Stars", NewStars);
            else
                cmd.Parameters.AddWithValue("@Stars", post.Stars);
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                int affectedRows = cmd.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    if (!string.IsNullOrEmpty(NewTitle))
                        post.Title = NewTitle;
                    if (!string.IsNullOrEmpty(NewReference))
                        post.Reference = NewReference;
                    if (!string.IsNullOrEmpty(NewPros))
                        post.Pros = NewPros;
                    if (!string.IsNullOrEmpty(NewCons))
                        post.Cons = NewCons;
                    if (NewStars != null)
                        post.Stars = NewStars;
                }
            }
            catch (SqlException e)
            {
                //sgdggdgdji
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return post;
        }

        public IPost UpdatePost(Project post, string NewTitle, string NewReference, string NewDescription, string NewImage)
        {
            cmd = new("sp_UpdateProject", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = post.Id;
            if (!string.IsNullOrEmpty(NewTitle))
                cmd.Parameters.AddWithValue("@Title", NewTitle);
            else
                cmd.Parameters.AddWithValue("@Title", post.Title);
            if (!string.IsNullOrEmpty(NewReference))
                cmd.Parameters.AddWithValue("@Reference", NewReference);
            else
                cmd.Parameters.AddWithValue("@Reference", post.Reference);
            if (!string.IsNullOrEmpty(NewDescription))
                cmd.Parameters.AddWithValue("@Description", NewDescription);
            else
                cmd.Parameters.AddWithValue("@Description", post.Description);
            if (!string.IsNullOrEmpty(NewImage))
                cmd.Parameters.AddWithValue("@Image", NewImage);
            else
                cmd.Parameters.AddWithValue("@Image", post.Image);

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                int affectedRows = cmd.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    if (!string.IsNullOrEmpty(NewTitle))
                        post.Title = NewTitle;
                    if (!string.IsNullOrEmpty(NewReference))
                        post.Reference = NewReference;
                    if (!string.IsNullOrEmpty(NewDescription))
                        post.Description = NewDescription;
                    if (!string.IsNullOrEmpty(NewImage))
                        post.Image = NewImage;
                }
            }
            catch (SqlException e)
            {
                // sisisis
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return post;
        }

        public void DeletePost(Guid id)
        {
            PostType postType = GetPostById(id).Type;
            if (postType == PostType.BlogPost)
            {
                cmd = new("sp_FullDeleteBlogPost", con);
            }
            else if (postType == PostType.Review)
            {
                cmd = new("sp_FullDeleteReview", con);
            }
            else if (postType == PostType.Project)
            {
                cmd = new("sp_FullDeleteProject", con);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = id;

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    _posts.Remove(GetPostById(id));
                }
            }
            catch (SqlException e)
            {
                //do stuff here
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public List<IPost> LoadListOfPosts()
        {
            List<IPost> posts = new List<IPost>();

            string sqlCommand = @"
        SELECT * FROM PostTable
        LEFT JOIN ProjectTable ON PostTable.Post_Id = ProjectTable.Post_Id
        LEFT JOIN BlogPostTable ON PostTable.Post_Id = BlogPostTable.Post_Id
        LEFT JOIN ReviewTable ON PostTable.Post_Id = ReviewTable.Post_Id";

            cmd = new SqlCommand(sqlCommand, con);

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var postType = reader["PostType"].ToString();

                        if (postType == "BlogPost")
                        {
                            BlogPost blogPost = new BlogPost(
                                title: reader["Title"].ToString(),
                                reference: reader["Reference"].ToString(),
                                type: PostType.BlogPost,
                                weblog: reader["Weblog"].ToString()
                            )
                            {
                                Date = DateTime.Parse(reader["Date"].ToString()),
                                Id = Guid.Parse(reader["Post_Id"].ToString()),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("Isdeleted"))
                            };
                            blogPost.Tags = GetTagsForPost(blogPost.Id);
                            posts.Add(blogPost);
                        }
                        else if (postType == "Project")
                        {
                            Project project = new Project(
                                title: reader["Title"].ToString(),
                                reference: reader["Reference"].ToString(),
                                type: PostType.Project,
                                description: reader["Description"].ToString(),
                                image: reader["Image"].ToString()
                            )
                            {
                                Date = DateTime.Parse(reader["Date"].ToString()),
                                Id = Guid.Parse(reader["Post_Id"].ToString()),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("Isdeleted"))
                            };
                            project.Tags = GetTagsForPost(project.Id);
                            posts.Add(project);
                        }
                        else if (postType == "Review")
                        {
                            Review review = new Review(
                                title: reader["Title"].ToString(),
                                reference: reader["Reference"].ToString(),
                                type: PostType.Review,
                                pros: reader["Pros"].ToString(),
                                cons: reader["Cons"].ToString(),
                                stars: short.Parse(reader["Stars"].ToString())
                            )
                            {
                                Date = DateTime.Parse(reader["Date"].ToString()),
                                Id = Guid.Parse(reader["Post_Id"].ToString()),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("Isdeleted"))
                            };
                            review.Tags = GetTagsForPost(review.Id);
                            posts.Add(review);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                //do stuff here
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return posts;
        }

        public void SoftDeletePost(Guid id)
        {
            var post = GetPostById(id);
            if (post != null)
            {
                cmd = new("sp_SoftDelete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = id;

                try
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                        post.IsDeleted = true;
                }
                catch (SqlException e)
                {
                    //sjgiseiog
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
        }

        public void RestorePost(Guid id)
        {
            var post = GetPostById(id);
            if (post != null)
            {
                cmd = new("sp_RestorePost", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = id;

                try
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                        post.IsDeleted = false;
                }
                catch (SqlException e)
                {
                    //do stuff here
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
        }

        public void RemoveTag(Tag tag, IPost post)
        {
            cmd = new("sp_RemoveTagFromPost", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = post.Id;
            cmd.Parameters.AddWithValue("@Tag_Id", SqlDbType.NVarChar).Value = tag.Id;

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    post.Tags.ListOfTags.Remove(tag);
                }
            }
            catch (SqlException e)
            {
                //error handling
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public void AddTag(Tag tag, IPost post)
        {
            cmd = new("sp_AddTagToPost", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Tag_Id", SqlDbType.NVarChar).Value = tag.Id;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = post.Id;

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    post.Tags.ListOfTags.Add(tag);
                }
            }
            catch (SqlException e)
            {
                //error handling
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public List<Tag> LoadListOfTags()
        {
            List<Tag> tags = new List<Tag>();
            cmd = new("SELECT * FROM TagsTable", con);

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tag tag = new(name: reader["Name"].ToString())
                        {
                            Id = Guid.Parse(reader["Tag_Id"].ToString())
                        };
                        tags.Add(tag);
                    }
                }
            }
            catch (SqlException e)
            {
                //error handling
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return tags;
        }

        public void SetAuthor()
        {
            Author = _personRepo.UserAccounts.Find(p => p.IsAuthor == true);
            if (Author is null)
            {
                _personRepo.LoadListOfPeople();
                SetAuthor();
            }
        }

        public IPost GetPostById(Guid id)
        {
            List<IPost> posts = LoadListOfPosts();
            return posts.Find(post => post.Id == id);
        }

        public Tag GetTagById(Guid id)
        {
            List<Tag> tags = LoadListOfTags();
            return tags.Find(tag => tag.Id == id);
        }

        public TagList GetTagsForPost(Guid postId)
        {
            List<Tag> tags = new List<Tag>();
            using (SqlConnection newCon = new SqlConnection(con.ConnectionString))
            {
                cmd = new("sp_GetTagsForPost", newCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Post_Id", postId);

                try
                {
                    newCon.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tag tag = new(name: reader["Name"].ToString())
                            {
                                Id = Guid.Parse(reader["Tag_Id"].ToString())
                            };
                            tags.Add(tag);
                        }
                    }
                }
                catch (SqlException e)
                {
                    //do stuff yes
                }
            }

            TagList Tags = new(postId)
            {
                ListOfTags = tags
            };

            return Tags;
        }
    }
}