CREATE TABLE [TextMining].[Hashtags] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [TweetID]          INT            NULL,
    [Hashtag]          NVARCHAR (500) NULL,
    [IsOnTheWhiteList] BIT            NULL,
    CONSTRAINT [PK_TextMining_Hashtags] PRIMARY KEY CLUSTERED ([ID] ASC)
);



