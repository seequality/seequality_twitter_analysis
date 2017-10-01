using HtmlAgilityPack;
using Libraries.Classes;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries
{
    public class ParseTwitterData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void ParseAllFilesFromDirectory(string targetDirectory)
        {
            logger.Info("ParseTwitterData started");

            List<string> allFilesPaths = new List<string>();
            List<FileContent> filesContent = new List<FileContent>();

            

            #region Read files

            // get all files from the given directory
            allFilesPaths = Directory.GetFiles(targetDirectory).ToList();

            // read all files and convert to the HTML document
            foreach (var filePath in allFilesPaths.Take(1))
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
                catch(Exception exc)
                {
                    logger.Error(exc);
                }
            }

            #endregion

            //TODO possibly if needed at some point save to database as table [File]
            foreach (var fileContent in filesContent)
            {
                var filePath = fileContent.FilePath;
                var fileSize = fileContent.FileSize;
                var htmlDocument = fileContent.HTMLDocument;
            }

            foreach (var fileContent in filesContent)
            {
                int iNumberOfTweets = 0;
                var tweets = fileContent.HTMLDocument.SelectNodes("//div[@class='content']");
                foreach (var tweet in tweets)
                {
                    // parse header
                    string userAddressName = tweet.SelectSingleNode("//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']").GetAttributeValue("href", String.Empty).ToString().Trim();
                    long userID = Convert.ToInt64(tweet.SelectSingleNode("//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']").GetAttributeValue("data-user-id", String.Empty).ToString().Trim());
                    string userImagePath = tweet.SelectSingleNode("//div[@class='stream-item-header']//img[@class='avatar js-action-profile-avatar']").GetAttributeValue("src", String.Empty).ToString().Trim();
                    string userFullName = tweet.SelectSingleNode("//div[@class='stream-item-header']//strong[@class='fullname show-popup-with-id ']").InnerText.ToString().Trim();
                    string userTwitterName = tweet.SelectSingleNode("//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']//span[@class='username u-dir']").InnerText.ToString().Trim();
                    string date = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']").InnerText.ToString().Trim();
                    string statusPath = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']").GetAttributeValue("href", String.Empty).ToString().Trim();
                    string dateTimeTitle = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']").GetAttributeValue("title", String.Empty).ToString().Trim();
                    long conversationID = Convert.ToInt64(tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']").GetAttributeValue("data-conversation-id", String.Empty).ToString().Trim());
                    long originalDateTime = Convert.ToInt64(tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']").GetAttributeValue("data-time", String.Empty).ToString().Trim());
                    long originalDateTimeMS = Convert.ToInt64(tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']").GetAttributeValue("data-time-ms", String.Empty).ToString().Trim());
                    DateTime dateTime = HelperMethods.UnixTimeStampToDateTime(Convert.ToInt64(tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']").GetAttributeValue("data-time", String.Empty).ToString().Trim()));
                
                    // parse tweet text
                    string tweetText = tweet.SelectSingleNode("//div[@class='js-tweet-text-container']").InnerText.ToString().Trim();
                    string tweetLanguage = tweet.SelectSingleNode("//div[@class='js-tweet-text-container']//p[@class='TweetTextSize  js-tweet-text tweet-text']").GetAttributeValue("lang",String.Empty).ToString().Trim();

                    // parse adaptive media
                    string tweetImagePath = tweet.SelectSingleNode("//div[@class='AdaptiveMediaOuterContainer']//img").GetAttributeValue("src",String.Empty).ToString().Trim();

                    // parse footer
                    int numberOfReplies = Convert.ToInt32(tweet.SelectSingleNode("//div[@class='stream-item-footer']//div[@class='ProfileTweet-actionCountList u-hiddenVisually']//span[@class='ProfileTweet-action--reply u-hiddenVisually']//span[@class='ProfileTweet-actionCount']").GetAttributeValue("data-tweet-stat-count", String.Empty).ToString().Trim());
                    int numberOfRetweets = Convert.ToInt32(tweet.SelectSingleNode("//div[@class='stream-item-footer']//div[@class='ProfileTweet-actionCountList u-hiddenVisually']//span[@class='ProfileTweet-action--retweet u-hiddenVisually']//span[@class='ProfileTweet-actionCount']").GetAttributeValue("data-tweet-stat-count", String.Empty).ToString().Trim());
                    int numberOFFavourites = Convert.ToInt32(tweet.SelectSingleNode("//div[@class='stream-item-footer']//div[@class='ProfileTweet-actionCountList u-hiddenVisually']//span[@class='ProfileTweet-action--favorite u-hiddenVisually']//span[@class='ProfileTweet-actionCount']").GetAttributeValue("data-tweet-stat-count", String.Empty).ToString().Trim());


                    iNumberOfTweets++;
                }


            }

            logger.Info("ParseTwitterData ended");
        }
    }
}
