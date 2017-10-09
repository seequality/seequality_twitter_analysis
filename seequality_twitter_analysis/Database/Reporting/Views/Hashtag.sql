

CREATE VIEW [Reporting].[Hashtag] 
AS 
WITH cte AS 
(
	SELECT h.Hashtag, ROW_NUMBER() OVER(ORDER BY COUNT(*) desc) AS HashtagRanking
	FROM TextMining.Tweet t (NOLOCK)
	INNER JOIN [TextMining].[Hashtags] h (NOLOCK)
		ON h.TweetID = t.TweetID
	GROUP BY h.Hashtag
)
SELECT 
       h.[TweetID]
      ,h.[Hashtag]
      ,h.[IsOnTheWhiteList]
	  ,c.HashtagRanking
  FROM [TextMining].[Hashtags] h (NOLOCK)
  INNER JOIN cte c 
	ON c.Hashtag = h.Hashtag
  WHERE TweetID IN 
(
	SELECT TweetID FROM Internal.DistinctTweet
)