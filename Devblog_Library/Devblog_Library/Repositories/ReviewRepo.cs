using Devblog_Library.Interfaces;
using Devblog_Library.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devblog_Library.Repositories
{
    public class ReviewRepo : IRepo<Review>
    {
        private readonly SqlConnection con;

        public ReviewRepo(IConfiguration configuration)
        {
            string conStr = configuration.GetConnectionString("MainConnection");
            con = new SqlConnection(conStr);
        }

        public Review CreatePost(string title, string reference, string pros, string cons, short stars)
        {
            Review post = new Review(title, reference, PostType.Review, pros, cons, stars);

            SqlCommand cmd = new("sp_CreateReview", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = post.Id;
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Reference", reference);
            cmd.Parameters.AddWithValue("@PostType", "Review");
            cmd.Parameters.AddWithValue("@Pros", pros);
            cmd.Parameters.AddWithValue("@Cons", cons);
            cmd.Parameters.AddWithValue("@Stars", stars);

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