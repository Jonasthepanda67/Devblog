using Azure;
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
    public class CommentRepo : ICommentRepo
    {
        private readonly SqlConnection con;
        private List<Comment> _comments = [];

        public CommentRepo(IConfiguration configuration)
        {
            string conStr = configuration.GetConnectionString("MainConnection");
            con = new SqlConnection(conStr);
        }

        public Comment CreateComment(Guid postId, string userName, string message)
        {
            Comment comment = new Comment(postId, userName, message);

            SqlCommand cmd = new("sp_CreateComment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Comment_Id", SqlDbType.NVarChar).Value = comment.Id;
            cmd.Parameters.AddWithValue("@Post_Id", SqlDbType.NVarChar).Value = postId;
            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@Message", message);
            cmd.Parameters.AddWithValue("@CreationDate", comment.CreationDate);

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
            return comment;
        }

        public List<Comment> LoadListOfComments()
        {
            List<Comment> comments = new List<Comment>();

            string sqlCommand = "SELECT * FROM CommentsTable";

            SqlCommand cmd = new SqlCommand(sqlCommand, con);

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
                        Comment comment = new Comment(
                            postId: Guid.Parse(reader["Post_Id"].ToString()),
                            userName: reader["UserName"].ToString(),
                            message: reader["Message"].ToString()
                        )
                        {
                            Id = Guid.Parse(reader["Comment_Id"].ToString()),
                            CreationDate = DateTime.Parse(reader["CreationDate"].ToString())
                        };
                        comments.Add(comment);
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
            return comments;
        }

        public void DeleteComment(Guid commentId)
        {
            SqlCommand cmd = new("sp_DeleteComment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Comment_Id", SqlDbType.NVarChar).Value = commentId;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                //do error handling here
            }
            finally
            {
                con.Close();
            }
        }

        public Comment GetComment(Guid commentId)
        {
            return _comments.Find(c => c.Id == commentId);
        }

        public void UpdateComment(Comment comment, string NewMessage)
        {
            SqlCommand cmd = new("sp_UpdateComment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Comment_Id", SqlDbType.NVarChar).Value = comment.Id;
            if (!string.IsNullOrEmpty(NewMessage))
                cmd.Parameters.AddWithValue("@Message", NewMessage);
            else
                cmd.Parameters.AddWithValue("@Message", comment.Message);

            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
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
        }
    }
}