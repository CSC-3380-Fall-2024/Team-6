using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TutorialCS
{
    /// <summary>
    /// Summary description for EventManager
    /// </summary>
    public class EventManager
    {

        public DataTable FilteredData(DateTime start, DateTime end)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [event] WHERE NOT (([eventend] <= @start) OR ([eventstart] >= @end))", ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("start", start);
            da.SelectCommand.Parameters.AddWithValue("end", end);

            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        public void EventEdit(string id, string name)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE [event] SET [name] = @name WHERE [id] = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("name", name);
                cmd.ExecuteNonQuery();

            }
        }

        public void EventMove(string id, DateTime start, DateTime end)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE [event] SET [eventstart] = @start, [eventend] = @end WHERE [id] = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.ExecuteNonQuery();

            }
        }

        public Event Get(string id)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [event] WHERE id - @id)", ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString);
            da.SelectCommand.Parameters.AddWithValue("id", id);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new Event
                {
                    Id = id,
                    Text = (string)dr["text"],
                    Start = (DateTime)dr["eventstart"],
                    End = (DateTime)dr["eventend"]
                };
            }
            return null;
        }

        internal void EventCreate(DateTime start, DateTime end, string text)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO [event] (eventstart, eventend, name) VALUES (@start, @end, @name); ", con);  // SELECT SCOPE_IDENTITY();
                cmd.Parameters.AddWithValue("start", start);
                cmd.Parameters.AddWithValue("end", end);
                cmd.Parameters.AddWithValue("name", text);
                cmd.ExecuteScalar();
            }
        }

        public class Event
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
        }

        public void EventDelete(string id)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM [event] WHERE id = @id", con);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }

}