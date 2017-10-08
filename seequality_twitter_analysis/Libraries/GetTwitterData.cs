using Libraries.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries
{
    public static class GetTwitterData
    {
        public static List<TweetText> GetTweets(string targetSQLConnectionString)
        {
            List<TweetText> tweets = new List<TweetText>();

            SqlConnection connection = new SqlConnection(targetSQLConnectionString);
            SqlCommand command = new SqlCommand("[Internal].[sp_GetTweets]", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                TweetText test = new TweetText();
                test.ID = int.Parse(reader["TweetID"].ToString());
                test.Text = reader["TweetText"].ToString();
                tweets.Add(test);
            }

            return tweets;
        }

        public static List<TweetText> GetTweets(string targetSQLConnectionString, string language)
        {
            List<TweetText> tweets = new List<TweetText>();

            SqlConnection connection = new SqlConnection(targetSQLConnectionString);
            SqlCommand command = new SqlCommand("Internal.sp_GetTweetsByLanguage", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Language", SqlDbType.VarChar,500).Value = language;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                TweetText test = new TweetText();
                test.ID = int.Parse(reader["TweetID"].ToString());
                test.Text = reader["TweetText"].ToString();
                tweets.Add(test);
            }

            return tweets;
        }
    }
}
