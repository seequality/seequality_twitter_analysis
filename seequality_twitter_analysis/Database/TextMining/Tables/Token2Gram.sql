CREATE TABLE [TextMining].[Token2Gram] (
    [TokenID] INT            IDENTITY (1, 1) NOT NULL,
    [TweetID] INT            NULL,
    [Token]   NVARCHAR (500) NULL,
    CONSTRAINT [PK_TextMining_Token2Gram] PRIMARY KEY CLUSTERED ([TokenID] ASC)
);



