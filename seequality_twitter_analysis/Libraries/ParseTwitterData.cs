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

            LogManager.ThrowExceptions = true;

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

            foreach (var fileContent in filesContent)
            {
                // table 1 : [Files]
                var filePath = fileContent.FilePath;
                var fileSize = fileContent.FileSize;
                var htmlDocument = fileContent.HTMLDocument;

                var tweets = fileContent.HTMLDocument.SelectNodes("//div[@class='content']");
                foreach (var tweet in tweets)
                {
                    // header
                    var userAddressName = tweet.SelectSingleNode("//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']").GetAttributeValue("href", String.Empty).ToString().Trim();
                    var userID = tweet.SelectSingleNode("//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']").GetAttributeValue("data-user-id", String.Empty).ToString().Trim();
                    var userImagePath = tweet.SelectSingleNode("//div[@class='stream-item-header']//img[@class='avatar js-action-profile-avatar']").GetAttributeValue("src", String.Empty).ToString().Trim();
                    var userFullName = tweet.SelectSingleNode("//div[@class='stream-item-header']//strong[@class='fullname show-popup-with-id ']").InnerText.ToString().Trim();
                    var userTwitterName = tweet.SelectSingleNode("//div[@class='stream-item-header']//a[@class='account-group js-account-group js-action-profile js-user-profile-link js-nav']//span[@class='username u-dir']").InnerText.ToString().Trim();
                    var date = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']").InnerText.ToString().Trim();
                    var statusPath = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']").GetAttributeValue("href", String.Empty).ToString().Trim();
                    var dateTimeTitle = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']").GetAttributeValue("title", String.Empty).ToString().Trim();
                    var conversationID = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']").GetAttributeValue("data-conversation-id", String.Empty).ToString().Trim();
                    var dateTime = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']").GetAttributeValue("data-time", String.Empty).ToString().Trim();
                    var dateTimeMS = tweet.SelectSingleNode("//div[@class='stream-item-header']//small[@class='time']//a[@class='tweet-timestamp js-permalink js-nav js-tooltip']//span[@class='_timestamp js-short-timestamp ']").GetAttributeValue("data-time-ms", String.Empty).ToString().Trim();

                    
                }

            }

            logger.Info("ParseTwitterData ended");
        }
    }
}
