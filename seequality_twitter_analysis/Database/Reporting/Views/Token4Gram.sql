
CREATE VIEW [Reporting].[Token4Gram]
AS
SELECT 
      [TweetID]
      ,[Token]
  FROM [TextMining].[Token4Gram] (NOLOCK)
  WHERE TweetID IN 
(
	SELECT TweetID FROM Internal.DistinctTweet
)