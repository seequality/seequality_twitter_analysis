CREATE TABLE [TextMining].[Tweet] (
    [TweetID]                                       INT            NOT NULL,
    [OriginalTweetWithoutSpecialCharacters]         NVARCHAR (500) NULL,
    [OriginalTweetEnglishWordsOnly]                 NVARCHAR (500) NULL,
    [OriginalTweetEnglishWordsOnlyWithoutStopWords] NVARCHAR (500) NULL,
    CONSTRAINT [PK_TextMining_Tweet] PRIMARY KEY CLUSTERED ([TweetID] ASC)
);



