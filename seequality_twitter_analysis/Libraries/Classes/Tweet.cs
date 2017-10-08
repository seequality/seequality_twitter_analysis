using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Classes
{
    internal class Tweet
    {
        public string UserAddressName { get; set; }
        public long UserID { get; set; }
        public string UserImagePath { get; set; }
        public string UserFullName { get; set; }
        public string UserTwitterName { get; set; }
        public string Date { get; set; }
        public string StatusPath { get; set; }
        public string DateTimeTitle { get; set; }
        public long ConversationID { get; set; }
        public long OriginalDateTime { get; set; }
        public long OriginalDateTimeMS { get; set; }
        public DateTime DateTime { get; set; }
        public string TweetText { get; set; }
        public string TweetLanguage { get; set; }
        public string TweetLanguageName { get; set; }
        public string TweetMediaName { get; set; }
        public string TweetMediaType { get; set; }
        public int NumberOfReplies { get; set; }
        public int NumberOfRetweets { get; set; }
        public int NumberOFFavourites { get; set; }
        public int NumberOfErrorsDuringParsing { get; set; }

    }
}
