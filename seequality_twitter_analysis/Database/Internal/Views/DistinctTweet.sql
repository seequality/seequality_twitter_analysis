

CREATE VIEW [Internal].[DistinctTweet]
AS
WITH cte AS 
(
	SELECT 
		TweetID, 
		ROW_NUMBER() OVER(PARTITION BY ConversationID, StatusPath ORDER BY TweetID ASC) AS RN
	FROM Internal.Tweet (NOLOCK)
)
SELECT cte.TweetID 
FROM cte
WHERE RN = 1