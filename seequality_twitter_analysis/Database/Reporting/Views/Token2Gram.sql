

CREATE VIEW [Reporting].[Token2Gram]
AS
SELECT 
      [TweetID]
      ,[Token]
  FROM [TextMining].[Token2Gram] (NOLOCK)
  WHERE TweetID IN 
(
	SELECT TweetID FROM Internal.DistinctTweet
)