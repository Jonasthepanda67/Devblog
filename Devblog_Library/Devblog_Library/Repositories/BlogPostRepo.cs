using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Repositories
{
    public class BlogPostRepo : IRepo<BlogPost>
    {
        private readonly SqlConnection con;

        public BlogPostRepo(IConfiguration configuration)
        {
            string conStr = configuration.GetConnectionString("MainConnection");
            con = new SqlConnection(conStr);
        }

        public BlogPost CreatePost(string title, string reference, string description, string content = "", short stars = 0)
        {
            BlogPost post = new BlogPost(title, reference, PostType.BlogPost, description);

            //SqlCommand cmd = new("INSERT INTO PostTabl(Post_Id, Title, Reference, Weblog) VALUES (@Post_Id, @Title, @Reference, @Weblog)", con);
            SqlCommand cmd = new("sp_CreateBlogPost", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = post.Id;
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Reference", reference);
            cmd.Parameters.AddWithValue("@PostType", "BlogPost");
            cmd.Parameters.AddWithValue("@Weblog", description);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                //Do stuff here
            }
            finally
            {
                con.Close();
            }

            return post;
        }
    }
}