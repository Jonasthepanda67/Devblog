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
using static System.Net.Mime.MediaTypeNames;

namespace Devblog_Library.Repositories
{
    public class TagRepo : ITagRepo
    {
        private List<Tag> _tags = [];
        private readonly SqlConnection con;

        public TagRepo(IConfiguration configuration)
        {
            string conStr = configuration.GetConnectionString("MainConnection");
            con = new SqlConnection(conStr);
        }

        public Tag CreateTag(string Name)
        {
            Tag tag = new Tag(Name);

            SqlCommand cmd = new("sp_CreateTag", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Tag_Id", SqlDbType.NVarChar).Value = tag.Id;
            cmd.Parameters.AddWithValue("@Name", tag.Name);

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
            _tags.Add(tag);
            return tag;
        }

        public void DeleteTag(Tag tag)
        {
            SqlCommand cmd = new("sp_DeleteTag", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Tag_Id", SqlDbType.NVarChar).Value = tag.Id;

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

        public void UpdateTag(Guid Id, string NewName)
        {
            SqlCommand cmd = new("sp_UpdateTag", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Tag_Id", SqlDbType.NVarChar).Value = Id;
            cmd.Parameters.AddWithValue("@Name", NewName);

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