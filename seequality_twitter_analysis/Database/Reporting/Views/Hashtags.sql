
CREATE VIEW [Reporting].[Hashtags] 
AS 
SELECT 
      [TweetID]
      ,[Hashtag]
      ,[IsOnTheWhiteList]
  FROM [TextMining].[Hashtags]