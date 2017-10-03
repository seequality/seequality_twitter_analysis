CREATE TABLE [dbo].[TextMiningResult] (
    [TextMiningResultID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TweetID]            INT            NOT NULL,
    [TextMiningMethodID] SMALLINT       NOT NULL,
    [TweetText]          NVARCHAR (500) NOT NULL
);

