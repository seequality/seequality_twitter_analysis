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

            foreach (var htmlDocument in filesContent)
            {
            }

            logger.Info("ParseTwitterData ended");
        }
    }
}
