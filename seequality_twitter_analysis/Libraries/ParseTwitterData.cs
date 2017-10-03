using HtmlAgilityPack;
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
    public class ParseTwitterData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void ParseAllFilesFromDirectory(string targetDirectory, string targetSQLConnectionString)
        {
            logger.Info("ParseTwitterData started");

            string sqlConnectionString = targetSQLConnectionString;
            List<string> allFilesPaths = new List<string>();
            List<FileContent> filesContent = new List<FileContent>();
            List<Tweet> allTweets = new List<Tweet>();
            SqlConnection conn = new SqlConnection(sqlConnectionString);
            SqlCommand cmd;
            int ExecutionID = 0;

            #region Start execution log in database

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

                cmd = new SqlCommand("[sp_LogStart]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ExecutionID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    cmd.ExecuteNonQuery();
                    ExecutionID = Convert.ToInt32(cmd.Parameters["@ExecutionID"].Value);
                    logger.Info("LogStart done");
                }
                catch (Exception exc)
                {
                    logger.Error(exc);
                }
            }

            #endregion

            if (ExecutionID != 0)
            {

                #region Read files and save into database

                #region Read files

                // get all files from the given directory
                allFilesPaths = Directory.GetFiles(targetDirectory).ToList();

                // read all files and convert to the HTML document
                foreach (var filePath in allFilesPaths.Where(x => x.Contains("alltheothers_since20170926until20170927")))
                {
                    try
                    {
                        string _htmlDocuments = @"<!DOCTYPE html><html><head><title>Page</title></head>" + File.ReadAllText(filePath, Encoding.UTF8) + "</html>";
                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(_htmlDocuments);

                        FileContent fileContent = new FileContent();
                        fileContent.FilePath = filePath;
                        fileContent.FileSize = new System.IO.FileInfo(filePath).Length;
                        fileContent.HTMLDocument = htmlDocument.DocumentNode;
                        filesContent.Add(fileContent);

                        logger.Info("Reading file " + filePath + " done");
                    }
                    catch (Exception exc)
                    {
                        logger.Error(exc);
                    }
                }

                #endregion

                #region Save file info and content into database

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
                    foreach (var fileContent in filesContent)
                    {
                        cmd = new SqlCommand("[sp_InsertFileContent]", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ExecutionID", SqlDbType.Int).Value = ExecutionID;
                        cmd.Parameters.Add("@FilePath", SqlDbType.VarChar, 1000).Value = fileContent.FilePath;
                        cmd.Parameters.Add("@FileSize", SqlDbType.BigInt).Value = fileContent.FileSize;
                        cmd.Parameters.Add("@HTMLDocument", SqlDbType.NVarChar, -1).Value = fileContent.HTMLDocument.OuterHtml;

                        try
                        {
                            cmd.ExecuteNonQuery();
                            logger.Info("FileContent inserted into the database succesfully");
                        }
                        catch (Exception exc)
                        {
                            logger.Error(exc);
                        }
                    }

                    conn.Close();
                }

                #endregion

                #endregion

                #region Parse documents and save tweets into database

                #region Parse documents 

                foreach (var fileContent in filesContent)
                {
                    int iNumberOfTweets = 0;
                    int iTotalNumberOfErrorsDuringParsing = 0;

                    var tweets = fileContent.HTMLDocument.SelectNodes("//div[@class='content']");
                    foreach (var tweet in tweets)
                    {
                        Tweet currentTweet = new Tweet();
                        int iNumberOfErrorsDuringParsing = 0;

                        #region Parse tweet header

                        try
                        {
                            currentTweet.UserAddressName = tweet
                                .SelectSingleNode(".//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']")
                                .GetAttributeValue("href", String.Empty)
                                .ToString()
                                .Trim();
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "UserAddressName" + " " + exc);
                        }

                        try
                        {
                            currentTweet.UserID = Convert.ToInt64(tweet
                                .SelectSingleNode(".//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']")
                                .GetAttributeValue("data-user-id", String.Empty)
                                .ToString()
                                .Trim()
                            );
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "UserID" + " " + exc);
                        }

                        try
                        {
                            currentTweet.UserImagePath = tweet
                                .SelectSingleNode(".//div[@class='stream-item-header']//img[@class='avatar js-action-profile-avatar']")
                                .GetAttributeValue("src", String.Empty)
                                .ToString()
                                .Trim();
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "UserImagePath" + " " + exc);
                        }

                        try
                        {   
                            currentTweet.UserFullName = tweet
                                .SelectSingleNode(".//div[@class='stream-item-header']//strong[@class='fullname show-popup-with-id ' or @class='fullname show-popup-with-id fullname-rtl']")
                                .InnerText
                                .ToString()
                                .Trim();
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "UserFullName" + " " + exc);
                        }

                        try
                        {
                            currentTweet.UserTwitterName = tweet
                                .SelectSingleNode(".//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']//span[@class='username u-dir']")
                                .InnerText
                                .ToString()
                                .Trim();
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "UserTwitterName" + " " + exc);
                        }

                        try
                        {
                            currentTweet.Date = tweet
                                .SelectSingleNode(".//div[@class='stream-item-header']//small[@class='time']")
                                .InnerText
                                .ToString()
                                .Trim();
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "Date" + " " + exc);
                        }

                        try
                        {
                            currentTweet.StatusPath = tweet.
                                SelectSingleNode(".//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']")
                                .GetAttributeValue("href", String.Empty)
                                .ToString()
                                .Trim();
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "StatusPath" + " " + exc);
                        }

                        try
                        {
                            currentTweet.DateTimeTitle = tweet
                                .SelectSingleNode(".//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']")
                                .GetAttributeValue("title", String.Empty)
                                .ToString()
                                .Trim();
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "DateTimeTitle" + " " + exc);
                        }

                        try
                        {
                            currentTweet.ConversationID = Convert.ToInt64(tweet
                                .SelectSingleNode(".//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']")
                                .GetAttributeValue("data-conversation-id", String.Empty)
                                .ToString()
                                .Trim());
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "ConversationID" + " " + exc);
                        }

                        try
                        {
                            currentTweet.OriginalDateTime = Convert.ToInt64(tweet.
                                SelectSingleNode(".//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']")
                                .GetAttributeValue("data-time", String.Empty)
                                .ToString()
                                .Trim());
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "OriginalDateTime" + " " + exc);
                        }

                        try
                        {
                            currentTweet.OriginalDateTimeMS = Convert.ToInt64(tweet
                                .SelectSingleNode(".//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']")
                                .GetAttributeValue("data-time-ms", String.Empty)
                                .ToString()
                                .Trim());
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "OriginalDateTimeMS" + " " + exc);
                        }

                        try
                        {
                            currentTweet.DateTime = HelperMethods.UnixTimeStampToDateTime(Convert.ToInt64(tweet
                                .SelectSingleNode(".//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']")
                                .GetAttributeValue("data-time", String.Empty)
                                .ToString()
                                .Trim()));
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "DateTime" + " " + exc);
                        }

                        #endregion

                        #region Parse tweet text

                        try
                        {
                            currentTweet.TweetText = tweet
                                .SelectSingleNode(".//div[@class='js-tweet-text-container']")
                                .InnerText
                                .ToString()
                                .Trim();
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "TweetText" + " " + exc);
                        }

                        try
                        {
                            currentTweet.TweetLanguage = tweet
                            .SelectSingleNode(".//div[@class='js-tweet-text-container']//p[@class='TweetTextSize  js-tweet-text tweet-texts' or @class='TweetTextSize  js-tweet-text tweet-text' or @class='TweetTextSize  js-tweet-text tweet-text tweet-text-rtl']")
                            .GetAttributeValue("lang", String.Empty)
                            .ToString()
                            .Trim();
                            
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "TweetLanguage" + " " + exc);
                        }

                        #endregion

                        #region Parse tweet adaptive media

                        try
                        {
                            string tweetMediaName = "";
                            string tweetMediaType = "";

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='QuoteMedia-singlePhoto']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "QuoteMedia-singlePhoto";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-singlePhoto']").InnerHtml.Length > 0)
                                    { 
                                        tweetMediaName = "AdaptiveMedia-singlePhoto";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-doublePhoto']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "AdaptiveMedia-doublePhoto";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='QuoteMedia-doublePhoto']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "QuoteMedia-doublePhoto";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-photoContainer js-adaptive-photo ']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "js-adaptive photo";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-video']//div//div").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "AdaptiveMedia-video";
                                        tweetMediaType = "video";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='js-macaw-cards-iframe-container card-type-summary_large_image']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "card-type-summary_large_image";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='js-macaw-cards-iframe-container card-type-periscope_broadcast' or @class='js-macaw-cards-iframe-container initial-card-height card-type-periscope_broadcast']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "card-type-periscope_broadcast";
                                        tweetMediaType = "card";
                                    }
                                }
                                catch (Exception exc) { }
                            }


                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='QuoteMedia']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "QuoteMedia";
                                        tweetMediaType = "quote";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='QuoteTweet-container']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "QuoteTweet-container";
                                        tweetMediaType = "quote";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-badge']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "AdaptiveMedia-badge";
                                        tweetMediaType = "quote";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='PlayableMedia PlayableMedia--gif watched']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "PlayableMedia PlayableMedia--gif watched";
                                        tweetMediaType = "gif";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-halfHeightPhotoContainer']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "AdaptiveMedia-halfHeightPhotoContainer";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-twoThirdsWidthPhoto']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "AdaptiveMedia-twoThirdsWidthPhoto";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-triplePhoto']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "AdaptiveMedia-triplePhoto";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-halfHeightPhoto']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "AdaptiveMedia-halfHeightPhoto";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='PlayableMedia PlayableMedia--video watched']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "PlayableMedia PlayableMedia--video watched";
                                        tweetMediaType = "video";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-videoContainer']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "AdaptiveMedia-videoContainer";
                                        tweetMediaType = "video";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaName == "")
                            {
                                try
                                {
                                    if (tweet.SelectSingleNode(".//div[@class='AdaptiveMedia-halfWidthPhoto']").InnerHtml.Length > 0)
                                    {
                                        tweetMediaName = "AdaptiveMedia-halfWidthPhoto";
                                        tweetMediaType = "photo";
                                    }
                                }
                                catch (Exception exc) { }
                            }

                            if (tweetMediaType != "")
                            {
                                currentTweet.TweetMediaType = tweetMediaType;
                            }
                            else
                            {
                                currentTweet.TweetMediaType = "N/A";
                            }

                            if (tweetMediaName != "")
                            {
                                currentTweet.TweetMediaName = tweetMediaName;
                            }
                            else
                            {
                                currentTweet.TweetMediaName = "N/A";
                            }

                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "tweet media" + " " + exc);
                        }

                        #endregion

                        #region Parse tweet footer

                        try
                        {
                            currentTweet.NumberOfReplies = Convert.ToInt32(tweet
                                .SelectSingleNode(".//div[@class='stream-item-footer']//div[@class='ProfileTweet-actionCountList u-hiddenVisually']//span[@class='ProfileTweet-action--reply u-hiddenVisually']//span[@class='ProfileTweet-actionCount']")
                                .GetAttributeValue("data-tweet-stat-count", String.Empty)
                                .ToString()
                                .Trim());
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "NumberOfReplies" + " " + exc);
                        }

                        try
                        {
                            currentTweet.NumberOfRetweets = Convert.ToInt32(tweet
                                .SelectSingleNode(".//div[@class='stream-item-footer']//div[@class='ProfileTweet-actionCountList u-hiddenVisually']//span[@class='ProfileTweet-action--retweet u-hiddenVisually']//span[@class='ProfileTweet-actionCount']")
                                .GetAttributeValue("data-tweet-stat-count", String.Empty)
                                .ToString()
                                .Trim());
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "NumberOfRetweets" + " " + exc);
                        }

                        try
                        {
                            currentTweet.NumberOFFavourites = Convert.ToInt32(tweet
                                .SelectSingleNode(".//div[@class='stream-item-footer']//div[@class='ProfileTweet-actionCountList u-hiddenVisually']//span[@class='ProfileTweet-action--favorite u-hiddenVisually']//span[@class='ProfileTweet-actionCount']")
                                .GetAttributeValue("data-tweet-stat-count", String.Empty)
                                .ToString()
                                .Trim());
                        }
                        catch (Exception exc)
                        {
                            iNumberOfErrorsDuringParsing++;
                            logger.Error("Error during parsing " + "NumberOFFavourites" + " " + exc);
                        }

                        #endregion

                        currentTweet.NumberOfErrorsDuringParsing = iNumberOfErrorsDuringParsing;
                        allTweets.Add(currentTweet);

                        iNumberOfTweets++;
                        iTotalNumberOfErrorsDuringParsing = iTotalNumberOfErrorsDuringParsing + iNumberOfErrorsDuringParsing;
                    }

                    logger.Info("Parsed " + fileContent.FilePath + ", number of tweets: " + iNumberOfTweets + ", number of errors during parsing: " + iTotalNumberOfErrorsDuringParsing);
                }

                #endregion

                #region Save tweets into database

                conn = new SqlConnection(sqlConnectionString);

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
                    foreach (var tweet in allTweets)
                    {

                        cmd = new SqlCommand("[sp_InsertTweet]", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ExecutionID", SqlDbType.Int).Value = ExecutionID;
                        cmd.Parameters.Add("@UserAddressName", SqlDbType.VarChar, 500).Value = tweet.UserAddressName;
                        cmd.Parameters.Add("@UserID", SqlDbType.BigInt).Value = tweet.UserID;
                        cmd.Parameters.Add("@UserImagePath", SqlDbType.VarChar, 500).Value = tweet.UserImagePath;
                        cmd.Parameters.Add("@UserFullName", SqlDbType.NVarChar, 500).Value = tweet.UserFullName;
                        cmd.Parameters.Add("@UserTwitterName", SqlDbType.VarChar, 500).Value = tweet.UserTwitterName;
                        cmd.Parameters.Add("@Date", SqlDbType.VarChar, 500).Value = tweet.Date;
                        cmd.Parameters.Add("@StatusPath", SqlDbType.VarChar, 500).Value = tweet.StatusPath;
                        cmd.Parameters.Add("@DateTimeTitle", SqlDbType.VarChar, 500).Value = tweet.DateTimeTitle;
                        cmd.Parameters.Add("@ConversationID", SqlDbType.BigInt).Value = tweet.ConversationID;
                        cmd.Parameters.Add("@OriginalDateTime", SqlDbType.BigInt).Value = tweet.OriginalDateTime;
                        cmd.Parameters.Add("@OriginalDateTimeMS", SqlDbType.BigInt).Value = tweet.OriginalDateTimeMS;
                        cmd.Parameters.Add("@DateTime", SqlDbType.DateTime2).Value = tweet.DateTime;
                        cmd.Parameters.Add("@TweetText", SqlDbType.NVarChar, 500).Value = tweet.TweetText;
                        cmd.Parameters.Add("@TweetLanguage", SqlDbType.VarChar, 500).Value = tweet.TweetLanguage;
                        cmd.Parameters.Add("@TweetMediaName", SqlDbType.VarChar, 500).Value = tweet.TweetMediaName;
                        cmd.Parameters.Add("@TweetMediaType", SqlDbType.VarChar, 500).Value = tweet.TweetMediaType;
                        cmd.Parameters.Add("@NumberOfReplies", SqlDbType.Int).Value = tweet.NumberOfReplies;
                        cmd.Parameters.Add("@NumberOfRetweets", SqlDbType.Int).Value = tweet.NumberOfRetweets;
                        cmd.Parameters.Add("@NumberOFFavourites", SqlDbType.Int).Value = tweet.NumberOFFavourites;
                        cmd.Parameters.Add("@NumberOfErrorsDuringParsing", SqlDbType.Int).Value = tweet.NumberOfErrorsDuringParsing;

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

                    logger.Info("Tweets inserted into the database succesfully");
                }

                #endregion

                #endregion

                #region Stop execution log in database 

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

                    cmd = new SqlCommand("[sp_LogStop]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ExecutionID", SqlDbType.Int).Value = ExecutionID;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        logger.Info("LogStop done");
                    }
                    catch (Exception exc)
                    {
                        logger.Error(exc);
                    }
                }

                #endregion

            }
            else
            {
                logger.Error("Error while setting up the database log");
            }

            logger.Info("ParseTwitterData ended");

        }
    }
}
