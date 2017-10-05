CREATE TABLE [TextMining].[Token1Gram] (
    [TweetID]                        INT            NULL,
    [Token]                          NVARCHAR (500) NULL,
    [TokenRootWord]                  NVARCHAR (500) NULL,
    [IsEnglishWord]                  BIT            NULL,
    [IsStopWord]                     BIT            NULL,
    [IsNotEnglishWordAndNotStopWord] BIT            NULL,
    [IsHashtag]                      BIT            NULL,
    [IsAccountName]                  BIT            NULL,
    [IsNumber]                       BIT            NULL,
    [IsWebsiteUrl]                   BIT            NULL
);

