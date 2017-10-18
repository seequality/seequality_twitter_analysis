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
            logger.Info("MineEntireTweetTextsAndSaveIntoDatabase start");

            List<tmTweet> textMiningResults = new List<tmTweet>();
            string englishWordsDictionary = File.ReadAllText(englishWordsDictionaryPath);
            string englishStopWordsDictionary = File.ReadAllText(englishStopWordsDictionaryPath);

            foreach (var tweet in tweets)
            {
                tmTweet textMiningResult = new tmTweet();
                textMiningResult.TweetID = tweet.ID;
                textMiningResult.OriginalTweetWithoutSpecialCharacters = tmRemoveSpecialCharactersFromText(tweet.Text, true);
                textMiningResult.OriginalTweetEnglishWordsOnly = tmRemoveNonEnglishWords(tweet.Text, englishWordsDictionary);
                textMiningResult.OriginalTweetEnglishWordsOnlyWithoutStopWords = tmRemoveNonEnglishWordsAndStopWords(tweet.Text, englishWordsDictionary, englishStopWordsDictionary);
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

                    cmd = new SqlCommand("[Internal].[sp_InsertTMTweet]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TweetID", SqlDbType.Int).Value = result.TweetID;
                    cmd.Parameters.Add("@OriginalTweetWithoutSpecialCharacters", SqlDbType.NVarChar, 500).Value = result.OriginalTweetWithoutSpecialCharacters;
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

                logger.Info("MineEntireTweetTextsAndSaveIntoDatabase done");
            }
        }

        public static void MineTweetHashtagAndSaveIntoDatabase(string targetSQLConnectionString, List<TweetText> tweets, string IsOnTheWhiteList)
        {
            logger.Info("MineTweetHashtagAndSaveIntoDatabase start");

            List<tmHashtag> textMiningResults = new List<tmHashtag>();

            foreach (var tweet in tweets)
            {
                // remove all characters except # for hashtags
                var words = tmRemoveSpecialCharactersFromText(tweet.Text, '#').Split(' ');

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

                    cmd = new SqlCommand("[Internal].[sp_InsertTMHashtag]", conn);
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

                logger.Info("MineTweetHashtagAndSaveIntoDatabase done");
            }
        }

        public static void MineTweetAccountsAndSaveIntoDatabase(string targetSQLConnectionString, List<TweetText> tweets)
        {
            logger.Info("MineTweetAccountsAndSaveIntoDatabase start");

            List<tmAccount> textMiningResults = new List<tmAccount>();

            foreach (var tweet in tweets)
            {
                // remove all characters except # for hashtags
                var words = tmRemoveSpecialCharactersFromText(tweet.Text, '@').Split(' ');

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

                    cmd = new SqlCommand("[Internal].[sp_InsertTMAccounts]", conn);
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

                logger.Info("MineTweetAccountsAndSaveIntoDatabase done");
            }
        }

        public static void MineTokenizeTweet1Gram(string targetSQLConnectionString, List<TweetText> tweets, string englishWordDictionaryPath, string stopWordsFilePath)
        {
            logger.Info("MineTokenizeTweet1Gram start");

            List<tmToken1Gram> textMiningResults = new List<tmToken1Gram>();
            Iveonik.Stemmers.EnglishStemmer englishStemmer = new Iveonik.Stemmers.EnglishStemmer();

            string englishWordDictionary = File.ReadAllText(englishWordDictionaryPath);
            string stopWordsFile = File.ReadAllText(stopWordsFilePath);

            foreach (var tweet in tweets)
            {
                var words = tmRemoveSpecialCharactersFromText(tweet.Text, true).Split(' ');
                foreach (var word in words)
                {
                    tmToken1Gram result = new tmToken1Gram();
                    result.TweetID = tweet.ID;
                    result.Token = word;

                    if (tmRemoveEnglishStopWords(word, stopWordsFile).Length > 0)
                    {
                        result.IsStopWord = true;
                    }
                    else
                    {
                        result.IsStopWord = false;
                    }

                    if (tmRemoveNonEnglishWords(word, englishWordDictionary).Length > 0)
                    {
                        result.IsEnglishWord = true;
                    }
                    else
                    {
                        result.IsEnglishWord = false;
                    }

                    if (tmRemoveNonEnglishWordsAndStopWords(word, englishWordDictionary, stopWordsFile).Length > 0)
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
                        try
                        {
                            result.TokenRootWord = englishStemmer.Stem(word);
                        }
                        catch { }
                        finally
                        {
                            if (!(result.TokenRootWord.Length > 0))
                            {
                                result.TokenRootWord = "N/A";
                            }
                        }
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

                    cmd = new SqlCommand("[Internal].[sp_InsertToken1Gram]", conn);
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

                logger.Info("MineTokenizeTweet1Gram done");
            }
        }

        public static void MineTokenizeTweet2Gram(string targetSQLConnectionString, List<TweetText> tweets, string englishWordDictionaryPath, string stopWordsFilePath)
        {
            logger.Info("MineTokenizeTweet2Gram start");

            string englishWordDictionary = File.ReadAllText(englishWordDictionaryPath);
            string stopWordsFile = File.ReadAllText(stopWordsFilePath);

            foreach (var tweet in tweets)
            {
                var words = tmRemoveSpecialCharactersFromText(tweet.Text, true).Split(' ');
                List<tmToken2Gram> textMiningResults = new List<tmToken2Gram>();

                for (int i = 0; i < words.Count() - 1; i++)
                {
                    tmToken2Gram result = new tmToken2Gram();
                    result.TweetID = tweet.ID;
                    result.Token = words[i] + " " + words[i + 1];
                    textMiningResults.Add(result);
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

                        cmd = new SqlCommand("[Internal].[sp_InsertToken2Gram]", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@TweetID", SqlDbType.Int).Value = result.TweetID;
                        cmd.Parameters.Add("@Token", SqlDbType.NVarChar, 500).Value = result.Token;

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

                }

            }

            logger.Info("MineTokenizeTweet2Gram done");

        }

        public static void MineTokenizeTweet3Gram(string targetSQLConnectionString, List<TweetText> tweets)
        {
            logger.Info("MineTokenizeTweet2Gram start");

            foreach (var tweet in tweets)
            {
                var words = tmRemoveSpecialCharactersFromText(tweet.Text, true).Split(' ');
                List<tmToken2Gram> textMiningResults = new List<tmToken2Gram>();

                for (int i = 0; i < words.Count() - 1; i++)
                {
                    if (i <= words.Count() - 3)
                    {
                        tmToken2Gram result = new tmToken2Gram();
                        result.TweetID = tweet.ID;
                        result.Token = words[i] + " " + words[i + 1] + " " + words[i + 2];
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

                        cmd = new SqlCommand("[Internal].[sp_InsertToken3Gram]", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@TweetID", SqlDbType.Int).Value = result.TweetID;
                        cmd.Parameters.Add("@Token", SqlDbType.NVarChar, 500).Value = result.Token;

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

                }

            }

            logger.Info("MineTokenizeTweet2Gram done");

        }


        #region Text mining helper methods

        public static string tmRemoveSpecialCharactersFromWord(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == '@' || c == '#')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string tmRemoveSpecialCharactersFromWord(this string str, char exclude)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == exclude)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string tmRemoveSpecialCharactersFromText(this string str, bool removeUrls)
        {
            StringBuilder sb = new StringBuilder();
            List<string> allWords = str.Split(' ').ToList();

            foreach (var word in allWords)
            {
                if ((word.Contains("http") || word.Contains("pic.twitter.com")))
                {
                    if (removeUrls == true)
                    {
                        // do nothing
                    }
                    else
                    {
                        sb.Append(word + " ");
                    }
                }
                else
                {
                    sb.Append(tmRemoveSpecialCharactersFromWord(word) + " ");
                }
            }

            return sb.ToString().Replace(" @ ", " ").Replace(" # ", " ").Replace("&nbsp;", " ").Replace("…", " ").Replace("   ", " ").Replace("  ", " ").Trim().ToLower();
        }

        public static string tmRemoveSpecialCharactersFromText(this string str, char exclude)
        {
            StringBuilder sb = new StringBuilder();
            List<string> allWords = str.Split(' ').ToList();

            foreach (var word in allWords)
            {
                // this method is only for use in parsing hashtags or accounts
                if (!word.Contains("http") && !word.Contains("pic.twitter.com") && (word.Contains("@") || word.Contains("#")))
                {
                    sb.Append(tmRemoveSpecialCharactersFromWord(word, exclude) + " ");
                }
            }

            return sb.ToString().Replace(" @ ", " ").Replace(" # ", " ").Replace("&nbsp;", " ").Replace("…", " ").Replace("   ", " ").Replace("  ", " ").Trim().ToLower();
        }

        public static string tmRemoveNonEnglishWords(string str, string englishWordDictionary)
        {
            string _str = tmRemoveSpecialCharactersFromText(str, true);
            StringBuilder output = new StringBuilder();

            foreach (var word in _str.Split(' '))
            {
                if (englishWordDictionary.Contains(word))
                {
                    output.Append(word + " ");
                }
            }

            return output.ToString().Trim().ToLower();
        }

        public static string tmRemoveEnglishStopWords(string str, string englishStopWordsDictionary)
        {
            string _str = tmRemoveSpecialCharactersFromText(str, true);
            StringBuilder output = new StringBuilder();

            foreach (var word in _str.Split(' '))
            {
                if (englishStopWordsDictionary.Contains(word))
                {
                    output.Append(word + " ");
                }

            }

            return output.ToString().Trim().ToLower();
        }

        public static string tmRemoveNonEnglishWordsAndStopWords(string str, string englishWordsDictionary, string englishStopWordsDictionary)
        {
            string _str = tmRemoveSpecialCharactersFromText(str, true);
            StringBuilder output = new StringBuilder();

            foreach (var word in _str.Split(' '))
            {
                if (englishWordsDictionary.Contains(word) && (!englishStopWordsDictionary.Contains(word)))
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

        #endregion
    }
}
