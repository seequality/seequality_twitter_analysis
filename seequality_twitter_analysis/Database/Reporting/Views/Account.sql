

CREATE VIEW [Reporting].[Account] 
AS 
SELECT 
       TweetID ,
       Account 
FROM TextMining.Accounts (NOLOCK)
WHERE TweetID IN 
(
	SELECT TweetID FROM Internal.DistinctTweet
)