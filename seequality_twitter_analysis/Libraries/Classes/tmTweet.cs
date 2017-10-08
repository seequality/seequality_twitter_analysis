using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Classes
{
    class tmTweet
    {
        public int TweetID { get; set; }
        public string OriginalTweetWithoutSpecialCharacters { get; set; }
        public string OriginalTweetEnglishWordsOnly { get; set; }
        public string OriginalTweetEnglishWordsOnlyWithoutStopWords { get; set; }

    }
}
