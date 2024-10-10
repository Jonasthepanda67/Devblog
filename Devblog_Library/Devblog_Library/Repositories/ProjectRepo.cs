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
    public class ProjectRepo : IRepo<Project>
    {
        private readonly SqlConnection con;

        public ProjectRepo(IConfiguration configuration)
        {
            string conStr = configuration.GetConnectionString("MainConnection");
            con = new SqlConnection(conStr);
        }

        public Project CreatePost(string title, string reference, string description, string image, short stars = 0)
        {
            Project post = new Project(title, reference, PostType.Project, description, image);

            SqlCommand cmd = new("sp_CreateProject", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = post.Id;
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Reference", reference);
            cmd.Parameters.AddWithValue("@PostType", "Project");
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@Image", image);

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