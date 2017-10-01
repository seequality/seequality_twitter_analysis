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

            #region Read files

            // get all files from the given directory
            allFilesPaths = Directory.GetFiles(targetDirectory).ToList();

            // read all files and convert to the HTML document
            foreach (var filePath in allFilesPaths.Where(x=>x.Contains("OneDrive_since20170925until20170926")))
            {
                try
                {
                    string _htmlDocuments = @"<!DOCTYPE html><html><head><title>Page</title></head>" + File.ReadAllText(filePath) + "</html>";
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

            #region Save file info statistics

            SqlConnection conn = new SqlConnection(sqlConnectionString);

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
                    var filePath = fileContent.FilePath;
                    var fileSize = fileContent.FileSize;
                    var htmlDocument = fileContent.HTMLDocument;


                    SqlCommand cmd = new SqlCommand("[sp_InsertFileContent]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        currentTweet.UserAddressName = tweet.SelectSingleNode("//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']").GetAttributeValue("href", String.Empty).ToString().Trim();
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }


                    try
                    {
                        currentTweet.UserID = Convert.ToInt64(tweet.SelectSingleNode("//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']").GetAttributeValue("data-user-id", String.Empty).ToString().Trim());
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.UserImagePath = tweet.SelectSingleNode("//div[@class='stream-item-header']//img[@class='avatar js-action-profile-avatar']").GetAttributeValue("src", String.Empty).ToString().Trim();
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.UserFullName = tweet.SelectSingleNode("//div[@class='stream-item-header']//strong[@class='fullname show-popup-with-id ']").InnerText.ToString().Trim();
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.UserTwitterName = tweet.SelectSingleNode("//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']//span[@class='username u-dir']").InnerText.ToString().Trim();
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.Date = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']").InnerText.ToString().Trim();
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.StatusPath = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']").GetAttributeValue("href", String.Empty).ToString().Trim();
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.DateTimeTitle = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']").GetAttributeValue("title", String.Empty).ToString().Trim();
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.ConversationID = Convert.ToInt64(tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']").GetAttributeValue("data-conversation-id", String.Empty).ToString().Trim());
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.OriginalDateTime = Convert.ToInt64(tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']").GetAttributeValue("data-time", String.Empty).ToString().Trim());
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.OriginalDateTimeMS = Convert.ToInt64(tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']").GetAttributeValue("data-time-ms", String.Empty).ToString().Trim());
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.DateTime = HelperMethods.UnixTimeStampToDateTime(Convert.ToInt64(tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']").GetAttributeValue("data-time", String.Empty).ToString().Trim()));
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    #endregion

                    #region Parse tweet text

                    try
                    {
                        currentTweet.TweetText = tweet.SelectSingleNode("//div[@class='js-tweet-text-container']").InnerText.ToString().Trim();
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.TweetLanguage = tweet.SelectSingleNode("//div[@class='js-tweet-text-container']//p[@class='TweetTextSize  js-tweet-text tweet-text']").GetAttributeValue("lang", String.Empty).ToString().Trim();
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    #endregion

                    #region Parse tweet adaptive media

                    try
                    {
                        currentTweet.TweetImagePath = tweet.SelectSingleNode("//div[@class='AdaptiveMediaOuterContainer']//img").GetAttributeValue("src", String.Empty).ToString().Trim();
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    #endregion

                    #region Parse tweet footer
                    
                    try
                    {
                        currentTweet.NumberOfReplies = Convert.ToInt32(tweet.SelectSingleNode("//div[@class='stream-item-footer']//div[@class='ProfileTweet-actionCountList u-hiddenVisually']//span[@class='ProfileTweet-action--reply u-hiddenVisually']//span[@class='ProfileTweet-actionCount']").GetAttributeValue("data-tweet-stat-count", String.Empty).ToString().Trim());
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.NumberOfRetweets = Convert.ToInt32(tweet.SelectSingleNode("//div[@class='stream-item-footer']//div[@class='ProfileTweet-actionCountList u-hiddenVisually']//span[@class='ProfileTweet-action--retweet u-hiddenVisually']//span[@class='ProfileTweet-actionCount']").GetAttributeValue("data-tweet-stat-count", String.Empty).ToString().Trim());
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    try
                    {
                        currentTweet.NumberOFFavourites = Convert.ToInt32(tweet.SelectSingleNode("//div[@class='stream-item-footer']//div[@class='ProfileTweet-actionCountList u-hiddenVisually']//span[@class='ProfileTweet-action--favorite u-hiddenVisually']//span[@class='ProfileTweet-actionCount']").GetAttributeValue("data-tweet-stat-count", String.Empty).ToString().Trim());
                    }
                    catch (Exception exc)
                    {
                        iNumberOfErrorsDuringParsing++;
                        logger.Error(exc);
                    }

                    #endregion

                    currentTweet.NumberOfErrorsDuringParsing = iNumberOfErrorsDuringParsing;
                    allTweets.Add(currentTweet);

                    iNumberOfTweets++;
                    iTotalNumberOfErrorsDuringParsing = iTotalNumberOfErrorsDuringParsing + iNumberOfErrorsDuringParsing;
                }

                logger.Info("Parsed " + fileContent.FilePath + ", number of tweets: " + iNumberOfTweets + ", number of errors during parsing: " + iTotalNumberOfErrorsDuringParsing);
            }

            logger.Info("ParseTwitterData ended");

        }
    }
}
