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
            string directory = @"C:\Users\sldr01\Desktop\FILES\posts\pl_microsoft_ignite_summary";
            string sqlConnectionString = @"Data Source=localhost\sql2016; Initial Catalog=TwitterAnalysis; Integrated Security=SSPI;";

            //ParseTwitterData.ParseAllFilesFromDirectory(directory, sqlConnectionString);

            var tweets_en = GetTwitterData.GetTweets(sqlConnectionString, "en");
            TextMining.GetOriginalTweetWithoutSpecialCharactersAndSaveIntoDatabase(sqlConnectionString, tweets_en);
            TextMining.GetHashtagsAndSaveIntoDatabase(sqlConnectionString, tweets_en);
            TextMining.GetAccountsAndSaveIntoDatabase(sqlConnectionString, tweets_en);


            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}
