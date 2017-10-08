using Libraries.Classes;
using NLog;
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

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static List<TweetText> GetTweets(string targetSQLConnectionString)
        {
            logger.Info("GetTweets start");

            List<TweetText> tweets = new List<TweetText>();

            SqlConnection connection = new SqlConnection(targetSQLConnectionString);
            SqlCommand command = new SqlCommand("[Internal].[sp_GetTweets]", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                connection.Open();
            }
            catch (Exception exc)
            {
                logger.Error(exc);
            }

            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        TweetText test = new TweetText();
                        test.ID = int.Parse(reader["TweetID"].ToString());
                        test.Text = reader["TweetText"].ToString();
                        tweets.Add(test);
                    }
                }
                catch (Exception exc)
                {
                    logger.Error(exc);
                }

            }

            logger.Info("GetTweets done");

            return tweets;
        }

        public static List<TweetText> GetTweets(string targetSQLConnectionString, string language)
        {
            logger.Info("GetTweets for language " + language + " start");

            List<TweetText> tweets = new List<TweetText>();

            SqlConnection connection = new SqlConnection(targetSQLConnectionString);
            SqlCommand command = new SqlCommand("Internal.sp_GetTweetsByLanguage", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Language", SqlDbType.VarChar,500).Value = language;

            try
            {
                connection.Open();
            }
            catch (Exception exc)
            {
                logger.Error(exc);
            }

            if (connection.State == ConnectionState.Open)
            {
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    TweetText test = new TweetText();
                    test.ID = int.Parse(reader["TweetID"].ToString());
                    test.Text = reader["TweetText"].ToString();
                    tweets.Add(test);
                }
            }

            logger.Info("GetTweets for language " + language + " end");

            return tweets;
        }
    }
}
