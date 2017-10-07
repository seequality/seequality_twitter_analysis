CREATE TABLE [TextMining].[Accounts] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [TweetID] INT            NULL,
    [Account] NVARCHAR (500) NULL,
    CONSTRAINT [PK_TextMining_Accounts] PRIMARY KEY CLUSTERED ([ID] ASC)
);



