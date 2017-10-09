
CREATE VIEW [Reporting].[Account] 
AS 
WITH cte AS 
(
	SELECT a.Account, ROW_NUMBER() OVER(ORDER BY COUNT(*) desc) AS AccountRanking
	FROM TextMining.Tweet t (NOLOCK)
	INNER JOIN [TextMining].Accounts a (NOLOCK)
		ON a.TweetID = t.TweetID
	GROUP BY a.Account
)
SELECT 
       a.TweetID ,
       a.Account ,
	   c.AccountRanking
FROM TextMining.Accounts a (NOLOCK)
INNER JOIN cte c 
	ON c.Account = a.Account
WHERE TweetID IN 
(
	SELECT TweetID FROM Internal.DistinctTweet
)