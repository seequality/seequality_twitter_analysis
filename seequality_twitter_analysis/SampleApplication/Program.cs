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
            ParseTwitterData.ParseAllFilesFromDirectory(directory);


        }
    }
}
