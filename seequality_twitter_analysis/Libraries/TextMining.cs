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

        public static void MineEntireTweetTextsAndSaveIntoDatabase(string targetSQLConnectionString, List<TweetText> tweets, string englishWordsDictionaryPath, string englishStopWordsDictionaryPath)
        {
            string TextMiningMethod = "MineEntireTweetTextsAndSaveIntoDatabase";

            List<tmTweet> textMiningResults = new List<tmTweet>();

            foreach (var tweet in tweets)
            {
                tmTweet textMiningResult = new tmTweet();
                textMiningResult.TweetID = tweet.ID;
                textMiningResult.OriginalTweetWithoutSpecialCharacters = tmRemoveSpecialCharacters(tweet.Text);
                textMiningResult.OriginalTweetEnglishWordsOnly = tmRemoveNonEnglishWords(tweet.Text, englishWordsDictionaryPath);
                textMiningResult.OriginalTweetEnglishWordsOnlyWithoutStopWords = tmRemoveNonEnglishWordsAndStopWords(tweet.Text, englishWordsDictionaryPath, englishStopWordsDictionaryPath) ;
                textMiningResults.Add(textMiningResult);
            }

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
                foreach (var result in textMiningResults)
                {

                    cmd = new SqlCommand("[sp_InsertTMTweet]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TweetID", SqlDbType.Int).Value = result.TweetID;
                    cmd.Parameters.Add("@OriginalTweetWithoutSpecialCharacters", SqlDbType.NVarChar,500).Value = result.OriginalTweetWithoutSpecialCharacters;
                    cmd.Parameters.Add("@OriginalTweetEnglishWordsOnly", SqlDbType.NVarChar, 500).Value = result.OriginalTweetEnglishWordsOnly;
                    cmd.Parameters.Add("@OriginalTweetEnglishWordsOnlyWithoutStopWords", SqlDbType.NVarChar, 500).Value = result.OriginalTweetEnglishWordsOnlyWithoutStopWords;

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

        public static void MineTweetHashtagAndSaveIntoDatabase(string targetSQLConnectionString, List<TweetText> tweets, string IsOnTheWhiteList)
        {
            string TextMiningMethod = "MineTweetHashtagAndSaveIntoDatabase";

            List<tmHashtag> textMiningResults = new List<tmHashtag>();

            foreach (var tweet in tweets)
            {
                // remove all characters except # for hashtags
                var words = tmRemoveSpecialCharacters(tweet.Text).Split(' ');

                foreach (var word in words)
                {
                    // get only hashtags
                    if (word.StartsWith("#"))
                    {
                        string hashtag = word.Trim().ToLower();
                        if (hashtag.Length > 1)
                        {
                            bool isOnTheWhiteList = false;
                            if (hashtag == IsOnTheWhiteList) isOnTheWhiteList = true;

                            tmHashtag textMiningResult = new tmHashtag();
                            textMiningResult.TweetID = tweet.ID;
                            textMiningResult.Hashtag = hashtag;
                            textMiningResult.IsOnTheWhiteList = isOnTheWhiteList;
                            textMiningResults.Add(textMiningResult);
                        }
                    }
                }
            }

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
                foreach (var result in textMiningResults)
                {

                    cmd = new SqlCommand("[sp_InsertTMHashtag]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TweetID", SqlDbType.Int).Value = result.TweetID;
                    cmd.Parameters.Add("@Hashtag", SqlDbType.NVarChar, 500).Value = result.Hashtag;
                    cmd.Parameters.Add("@IsOnTheWhiteList", SqlDbType.Bit).Value = result.IsOnTheWhiteList;

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

        public static void MineTweetAccountsAndSaveIntoDatabase(string targetSQLConnectionString, List<TweetText> tweets)
        {
            string TextMiningMethod = "MineTweetHashtagAndSaveIntoDatabase";

            List<tmAccount> textMiningResults = new List<tmAccount>();

            foreach (var tweet in tweets)
            {
                // remove all characters except # for hashtags
                var words = tmRemoveSpecialCharacters(tweet.Text).Split(' ');

                foreach (var word in words)
                {
                    // get only hashtags
                    if (word.StartsWith("@"))
                    {
                        string account = word.Trim().ToLower();
                        if (account.Length > 1)
                        {

                            tmAccount textMiningResult = new tmAccount();
                            textMiningResult.TweetID = tweet.ID;
                            textMiningResult.Account = account;
                            textMiningResults.Add(textMiningResult);
                        }
                    }
                }
            }

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
                foreach (var result in textMiningResults)
                {

                    cmd = new SqlCommand("[sp_InsertTMAccounts]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TweetID", SqlDbType.Int).Value = result.TweetID;
                    cmd.Parameters.Add("@Account", SqlDbType.NVarChar, 500).Value = result.Account;

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

        public static void MineTokenizeTweet1Gram(string targetSQLConnectionString, List<TweetText> tweets, string englishWordDictionaryPath, string stopWordsFilePath)
        {
            string TextMiningMethod = "MineTokenizeTweet1Gram";

            List<tmToken1Gram> textMiningResults = new List<tmToken1Gram>();
            Iveonik.Stemmers.EnglishStemmer englishStemmer = new Iveonik.Stemmers.EnglishStemmer();

            foreach (var tweet in tweets)
            {
                var words = tmRemoveSpecialCharacters(tweet.Text).Split(' ');
                foreach (var word in words)
                {
                    tmToken1Gram result = new tmToken1Gram();
                    result.TweetID = tweet.ID;
                    result.Token = word;

                    if (tmRemoveEnglishStopWords(word, stopWordsFilePath).Length > 0)
                    {
                        result.IsStopWord = true;
                    }
                    else
                    {
                        result.IsStopWord = false;
                    }

                    if (tmRemoveNonEnglishWords(word, englishWordDictionaryPath).Length > 0)
                    {
                        result.IsEnglishWord = true;
                    }
                    else
                    {
                        result.IsEnglishWord = false;
                    }

                    if (tmRemoveNonEnglishWordsAndStopWords(word, englishWordDictionaryPath, stopWordsFilePath).Length > 0)
                    {
                        result.IsNotEnglishWordAndNotStopWord = true;
                    }
                    else
                    {
                        result.IsNotEnglishWordAndNotStopWord = false;
                    }

                    result.IsNumber = tmCheckIfNumber(word);

                    if (word.StartsWith("@"))
                    {
                        result.IsAccountName = true;
                    }
                    else
                    {
                        result.IsAccountName = false;
                    }

                    if (word.StartsWith("#"))
                    {
                        result.IsHashtag = true;
                    }
                    else
                    {
                        result.IsHashtag = false;
                    }

                    //Uri uriResult;
                    //result.IsWebsiteUrl = Uri.TryCreate(word, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if (word.StartsWith("www") || word.StartsWith("http") || word.StartsWith("https"))
                    {
                        result.IsWebsiteUrl = true;
                    }
                    else
                    {
                        result.IsWebsiteUrl = false;
                    }

                    if (result.IsEnglishWord == true && result.IsStopWord == false)
                    {
                        result.TokenRootWord = englishStemmer.Stem(word);
                    }

                    textMiningResults.Add(result);
                }
            }

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
                foreach (var result in textMiningResults)
                {

                    cmd = new SqlCommand("[sp_InsertToken1Gram]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TweetID", SqlDbType.Int).Value = result.TweetID;
                    cmd.Parameters.Add("@Token", SqlDbType.NVarChar, 500).Value = result.Token;
                    cmd.Parameters.Add("@TokenRootWord", SqlDbType.NVarChar, 500).Value = result.TokenRootWord;
                    cmd.Parameters.Add("@IsEnglishWord", SqlDbType.Bit).Value = result.IsEnglishWord;
                    cmd.Parameters.Add("@IsStopWord", SqlDbType.Bit).Value = result.IsStopWord;
                    cmd.Parameters.Add("@IsNotEnglishWordAndNotStopWord", SqlDbType.Bit).Value = result.IsNotEnglishWordAndNotStopWord;
                    cmd.Parameters.Add("@IsHashtag", SqlDbType.Bit).Value = result.IsHashtag;
                    cmd.Parameters.Add("@IsAccountName", SqlDbType.Bit).Value = result.IsAccountName;
                    cmd.Parameters.Add("@IsNumber", SqlDbType.Bit).Value = result.IsNumber;
                    cmd.Parameters.Add("@IsWebsiteUrl", SqlDbType.Bit).Value = result.IsWebsiteUrl;

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
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == '@' || c == '#')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Replace("  ","").Replace("   ","").Replace(" @ ","").Replace(" # ","").Trim().ToLower();
        }

        public static string tmRemoveNonEnglishWords(string str, string englishWordDictionaryPath)
        {
            string _str = tmRemoveSpecialCharacters(str);
            StringBuilder output = new StringBuilder();

            foreach (var word in _str.Split(' '))
            {
                if (File.ReadAllText(englishWordDictionaryPath).Contains(word))
                {
                    output.Append(word + " ");
                }
            }

            return output.ToString().Trim().ToLower();
        }

        public static string tmRemoveEnglishStopWords(string str, string englishStopWordsDictionaryPath)
        {
            string _str = tmRemoveSpecialCharacters(str);
            StringBuilder output = new StringBuilder();

            foreach (var word in _str.Split(' '))
            {
                if (File.ReadAllText(englishStopWordsDictionaryPath).Contains(word))
                {
                    output.Append(word + " ");
                }

            }

            return output.ToString().Trim().ToLower();
        }

        public static string tmRemoveNonEnglishWordsAndStopWords(string str, string englishWordsDictionaryPath, string englishStopWordsDictionaryPath)
        {
            string _str = tmRemoveSpecialCharacters(str);
            StringBuilder output = new StringBuilder();

            foreach (var word in _str.Split(' '))
            {
                if (File.ReadAllText(englishWordsDictionaryPath).Contains(word) && (!File.ReadAllText(englishStopWordsDictionaryPath).Contains(word)))
                {
                    output.Append(word + " ");
                }
            }

            return output.ToString().Trim().ToLower();
        }

        public static bool tmCheckIfNumber(string str)
        {
            bool isNumber = false;

            int intVariable = 0;
            decimal decimalVariable = 0;

            if (int.TryParse(str, out intVariable) || decimal.TryParse(str, out decimalVariable))
            {
                isNumber = true;
            }

            return isNumber;

        }

    }
}
