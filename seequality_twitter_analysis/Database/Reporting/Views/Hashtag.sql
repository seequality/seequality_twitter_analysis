
CREATE VIEW [Reporting].[Hashtag] 
AS 
SELECT 
      [TweetID]
      ,[Hashtag]
      ,[IsOnTheWhiteList]
  FROM [TextMining].[Hashtags]