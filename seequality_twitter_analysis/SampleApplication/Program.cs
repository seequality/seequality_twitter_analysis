using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Libraries;

namespace SampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            string directory = @"C:\Users\sldr01\Desktop\FILES\posts\pl_microsoft_ignite_summary";
            string sqlConnectionString = @"Data Source=localhost\sql2016; Initial Catalog=TwitterAnalysis; Integrated Security=SSPI;";
            string stopWordsFilePath = @"C:\SLDR\seequality\seequality_twitter_analysis\seequality_twitter_analysis\EnglishStopWords.txt";
            string englishWordDictionaryPath = @"C:\SLDR\seequality\seequality_twitter_analysis\seequality_twitter_analysis\EnglishWords.txt";

            HelperMethods.CleanDatabase(sqlConnectionString, true);

            ParseTwitterData.ParseAllFilesFromDirectory(directory, sqlConnectionString);

            var tweets_en = GetTwitterData.GetTweets(sqlConnectionString, "en");
            var tweets = GetTwitterData.GetTweets(sqlConnectionString);

            TextMining.MineEntireTweetTextsAndSaveIntoDatabase(sqlConnectionString, tweets_en, englishWordDictionaryPath, stopWordsFilePath);
            TextMining.MineTweetHashtagAndSaveIntoDatabase(sqlConnectionString, tweets, "#msignite");
            TextMining.MineTweetAccountsAndSaveIntoDatabase(sqlConnectionString, tweets);
            TextMining.MineTokenizeTweet1Gram(sqlConnectionString, tweets_en, englishWordDictionaryPath, stopWordsFilePath);
            TextMining.MineTokenizeTweet2Gram(sqlConnectionString, tweets_en, englishWordDictionaryPath, stopWordsFilePath);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            var elapsedSedonds = elapsedMs / 1000;
            Console.WriteLine("Elapsed miliseconds: " + elapsedMs.ToString() + " (" + elapsedSedonds.ToString() + " seconds)");

            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}
