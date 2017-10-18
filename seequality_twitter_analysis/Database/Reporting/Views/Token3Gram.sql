
CREATE VIEW [Reporting].[Token3Gram]
AS
SELECT 
      [TweetID]
      ,[Token]
  FROM [TextMining].[Token3Gram] (NOLOCK)
  WHERE TweetID IN 
(
	SELECT TweetID FROM Internal.DistinctTweet
)