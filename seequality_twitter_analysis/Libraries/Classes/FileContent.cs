using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Libraries.Classes
{
    internal class FileContent
    {
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public HtmlNode HTMLDocument { get; set; }

    }
}
