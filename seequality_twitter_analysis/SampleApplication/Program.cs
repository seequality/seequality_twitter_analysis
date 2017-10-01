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

            ParseTwitterData.ParseAllFilesFromDirectory(directory, sqlConnectionString);

            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}
