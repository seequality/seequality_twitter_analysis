

CREATE VIEW [Analytics].[ExtractWWW] AS 
SELECT 
	TweetID, 
	CASE WHEN CHARINDEX(' ',SUBSTRING(TweetText, CHARINDEX('http',TweetText), LEN(TweetText) - CHARINDEX('http',TweetText))) > 0 THEN 
	SUBSTRING(SUBSTRING(TweetText, CHARINDEX('http',TweetText), LEN(TweetText) - CHARINDEX('http',TweetText)), 0, CHARINDEX(' ',SUBSTRING(TweetText, CHARINDEX('http',TweetText), LEN(TweetText) - CHARINDEX('http',TweetText)))) 
	ELSE SUBSTRING(TweetText, CHARINDEX('http',TweetText), LEN(TweetText) - CHARINDEX('http',TweetText))
	END AS WWW
FROM Reporting.Tweet
WHERE TweetText LIKE '%http%'