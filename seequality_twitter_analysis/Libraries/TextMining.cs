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
    public static class TextMining
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public static void GetOriginalTweetWithoutSpecialCharactersAndSaveIntoDatabase(string targetSQLConnectionString, List<TweetText> tweets)
        {
            string TextMiningMethod = "Original tweet without special characters";
            int TextMiningMethodID;

            List<TextMiningResult> tweetsWithoutSpecialCharacters = new List<TextMiningResult>();

            TextMiningMethodID = HelperMethods.GetTextMiningMethodIDFromDatabase(targetSQLConnectionString, TextMiningMethod);

            foreach (var tweet in tweets)
            {
                TextMiningResult textMiningResult = new TextMiningResult();
                textMiningResult.TweetID = tweet.ID;
                textMiningResult.TextMiningMethodID = TextMiningMethodID;
                textMiningResult.TweetText = tmRemoveSpecialCharacters(tweet.Text);
                tweetsWithoutSpecialCharacters.Add(textMiningResult);
            }

            SaveTextMiningResultsToDatabase(targetSQLConnectionString, tweetsWithoutSpecialCharacters, TextMiningMethod);
        }

        public static void GetHashtagsAndSaveIntoDatabase(string targetSQLConnectionString, List<TweetText> tweets)
        {
            string TextMiningMethod = "Hashtag";
            int TextMiningMethodID;

            List<TextMiningResult> tweetsWithoutSpecialCharacters = new List<TextMiningResult>();

            TextMiningMethodID = HelperMethods.GetTextMiningMethodIDFromDatabase(targetSQLConnectionString, TextMiningMethod);

            foreach (var tweet in tweets)
            {
                // remove all characters except # for hashtags
                var words = tmRemoveSpecialCharacters(tweet.Text, '#').Split(' ');

                foreach (var word in words)
                {
                    // get only hashtags
                    if (word.StartsWith("#"))
                    {
                        TextMiningResult textMiningResult = new TextMiningResult();
                        textMiningResult.TweetID = tweet.ID;
                        textMiningResult.TextMiningMethodID = TextMiningMethodID;
                        textMiningResult.TweetText = word.Trim().ToLower();
                        tweetsWithoutSpecialCharacters.Add(textMiningResult);
                    }
                }
            }

            SaveTextMiningResultsToDatabase(targetSQLConnectionString, tweetsWithoutSpecialCharacters, TextMiningMethod);
        }

        public static void SaveTextMiningResultsToDatabase(string targetSQLConnectionString, List<TextMiningResult> results, string TextMiningMethod)
        {
            SqlConnection conn = new SqlConnection(targetSQLConnectionString);
            SqlCommand cmd;

            try
            {
                conn.Open();
            }
            catch (Exception exc)
            {
                logger.Error(exc);
            }

            if (conn.State == ConnectionState.Open)
            {
                foreach (var result in results)
                {

                    cmd = new SqlCommand("[sp_InsertTextMiningResult]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TweetID", SqlDbType.Int).Value = result.TweetID;
                    cmd.Parameters.Add("@TextMiningMethodID", SqlDbType.SmallInt).Value = result.TextMiningMethodID;
                    cmd.Parameters.Add("@TweetText", SqlDbType.NVarChar, 500).Value = result.TweetText;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        logger.Error(exc);
                    }
                }

                conn.Close();

                logger.Info("Text mining method " + TextMiningMethod + " done");
            }
        }

        public static string tmRemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string tmRemoveSpecialCharacters(this string str, char exceptChar)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == exceptChar)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

    }
}
