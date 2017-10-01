using HtmlAgilityPack;
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
            Logger logger = LogManager.GetLogger("foo");
            logger.Info("Program started");

            LogManager.ThrowExceptions = true;

            List<string> allFilesPaths = new List<string>();
            List<HtmlDocument> allHTMLDocuments = new List<HtmlDocument>();

            #region Read files

            // get all files from the given directory
            allFilesPaths = Directory.GetFiles(targetDirectory).ToList();

            // read all files and convert to the HTML document
            foreach (var file in allFilesPaths.Take(1))
            {

                try
                {
                    string fileContent = @"<!DOCTYPE html><html><head><title>Page</title></head>" + File.ReadAllText(file) + "</html>";
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(fileContent);
                    allHTMLDocuments.Add(htmlDocument);
                    logger.Info("Reading file " + file + " done");
                }
                catch(Exception exc)
                {
                    logger.Error(exc);
                }

            }

            #endregion

        }
    }
}
