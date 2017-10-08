using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Classes
{
    public class tmToken1Gram
    {
        public int TweetID { get; set; }
        public string Token { get; set; }
        public string TokenRootWord { get; set; }
        public bool IsEnglishWord { get; set; }
        public bool IsStopWord { get; set; }
        public bool IsNotEnglishWordAndNotStopWord { get; set; }
        public bool IsHashtag { get; set; }
        public bool IsAccountName { get; set; }
        public bool IsNumber { get; set; }
        public bool IsWebsiteUrl { get; set; }

    }
}
