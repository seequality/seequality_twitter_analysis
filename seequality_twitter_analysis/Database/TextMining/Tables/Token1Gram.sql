CREATE TABLE [TextMining].[Token1Gram] (
    [TokenID]                        INT            IDENTITY (1, 1) NOT NULL,
    [TweetID]                        INT            NULL,
    [Token]                          NVARCHAR (500) NULL,
    [TokenRootWord]                  NVARCHAR (500) NULL,
    [IsEnglishWord]                  BIT            NULL,
    [IsStopWord]                     BIT            NULL,
    [IsNotEnglishWordAndNotStopWord] BIT            NULL,
    [IsHashtag]                      BIT            NULL,
    [IsAccountName]                  BIT            NULL,
    [IsNumber]                       BIT            NULL,
    [IsWebsiteUrl]                   BIT            NULL,
    CONSTRAINT [PK_TextMining_Token1Gram] PRIMARY KEY CLUSTERED ([TokenID] ASC)
);



