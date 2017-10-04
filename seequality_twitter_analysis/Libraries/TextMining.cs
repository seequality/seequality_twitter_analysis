using Libraries.Classes;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries
{
    public static class TextMining
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void GetTheStopWordsFromFileAndLoadIntoDatabase(string stopWordsFileName, string targetSQLConnectionString)
        {
            SqlConnection conn = new SqlConnection(targetSQLConnectionString);
            SqlCommand cmd;

            List<string> stopWords = File.ReadAllLines(stopWordsFileName).ToList();

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

                foreach (var stopWord in stopWords)
                {

                    cmd = new SqlCommand("[sp_InsertStopWord]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@StopWord", SqlDbType.VarChar, 50).Value = stopWord;

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

                logger.Info("Text mining method stopwords done");
            }
        }

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
                textMiningResult.TweetText = tmRemoveSpecialCharacters(tweet.Text).Trim().ToLower();
                tweetsWithoutSpecialCharacters.Add(textMiningResult);
            }

            SaveTextMiningResultsToDatabase(targetSQLConnectionString, tweetsWithoutSpecialCharacters, TextMiningMethod);
        }

        public static void Tokenize1GramAndSaveIntoDatabase(string targetSQLConnectionString, List<TweetText> tweets, bool removeSpecialCharacters)
        {
            string TextMiningMethod = "N-gram 1 remove special characters : " + removeSpecialCharacters;
            int TextMiningMethodID;

            List<TextMiningResult> tweetsWithoutSpecialCharacters = new List<TextMiningResult>();

            TextMiningMethodID = HelperMethods.GetTextMiningMethodIDFromDatabase(targetSQLConnectionString, TextMiningMethod);

            foreach (var tweet in tweets)
            {
                List<string> words = tmTokenize1Gram(tweet.Text, removeSpecialCharacters);
                foreach (var word in words)
                {
                    TextMiningResult textMiningResult = new TextMiningResult();
                    textMiningResult.TweetID = tweet.ID;
                    textMiningResult.TextMiningMethodID = TextMiningMethodID;
                    textMiningResult.TweetText = word;
                    tweetsWithoutSpecialCharacters.Add(textMiningResult);
                }
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
                        string hashtag = word.Trim().ToLower();
                        if (hashtag.Length > 1)
                        {
                            TextMiningResult textMiningResult = new TextMiningResult();
                            textMiningResult.TweetID = tweet.ID;
                            textMiningResult.TextMiningMethodID = TextMiningMethodID;
                            textMiningResult.TweetText = hashtag;
                            tweetsWithoutSpecialCharacters.Add(textMiningResult);
                        }
                    }
                }
            }

            SaveTextMiningResultsToDatabase(targetSQLConnectionString, tweetsWithoutSpecialCharacters, TextMiningMethod);
        }

        public static void GetAccountsAndSaveIntoDatabase(string targetSQLConnectionString, List<TweetText> tweets)
        {
            string TextMiningMethod = "Twitter accounts";
            int TextMiningMethodID;

            List<TextMiningResult> tweetsWithoutSpecialCharacters = new List<TextMiningResult>();

            TextMiningMethodID = HelperMethods.GetTextMiningMethodIDFromDatabase(targetSQLConnectionString, TextMiningMethod);

            foreach (var tweet in tweets)
            {
                // remove all characters except @ for accounts
                var words = tmRemoveSpecialCharacters(tweet.Text, '@').Split(' ');

                foreach (var word in words)
                {
                    // get only accounts
                    if (word.StartsWith("@"))
                    {
                        string accountName = word.Trim().ToLower();
                        if (accountName.Length > 1)
                        {
                            TextMiningResult textMiningResult = new TextMiningResult();
                            textMiningResult.TweetID = tweet.ID;
                            textMiningResult.TextMiningMethodID = TextMiningMethodID;
                            textMiningResult.TweetText = accountName;
                            tweetsWithoutSpecialCharacters.Add(textMiningResult);
                        }
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

        public static List<string> tmTokenize1Gram(string str, bool removeSpecialCharacters)
        {
            List<string> tokens = new List<string>();

            if (removeSpecialCharacters)
            {
                tokens = tmRemoveSpecialCharacters(str).Split(' ').ToList().Select(x=> x.Trim().ToLower()).Where(x => x.Length > 0).ToList();
            }

            else
            {
                tokens = str.Split(' ').ToList();
            }
            return tokens;
        }

    }
}
