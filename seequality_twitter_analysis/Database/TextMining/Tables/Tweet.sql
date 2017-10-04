CREATE TABLE [TextMining].[Tweet] (
    [TweetID]                                       INT            NULL,
    [OriginalTweetWithoutSpecialCharacters]         NVARCHAR (500) NULL,
    [OriginalTweetEnglishWordsOnly]                 NVARCHAR (500) NULL,
    [OriginalTweetEnglishWordsOnlyWithoutStopWords] NVARCHAR (500) NULL
);

