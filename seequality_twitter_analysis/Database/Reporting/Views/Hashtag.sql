

CREATE VIEW [Reporting].[Hashtag] 
AS 
SELECT 
      [TweetID]
      ,[Hashtag]
      ,[IsOnTheWhiteList]
  FROM [TextMining].[Hashtags] (NOLOCK)
  WHERE TweetID IN 
(
	SELECT TweetID FROM Internal.DistinctTweet
)