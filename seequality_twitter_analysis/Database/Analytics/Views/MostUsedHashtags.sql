
CREATE VIEW [Analytics].[MostUsedHashtags] AS 
WITH cte AS 
(
	SELECT 
		CONVERT(DATE,rt.DateID) AS CalendarDate, 
		h.Hashtag, 
		COUNT(*) AS CNT, 
		ROW_NUMBER() OVER(PARTITION BY rt.DateID ORDER BY COUNT(*) DESC) AS RN
	FROM Reporting.Tweet rt
	INNER JOIN Reporting.Hashtag h
		ON rt.TweetID = h.TweetID
	WHERE h.IsOnTheWhiteList = 0
	GROUP BY rt.DateID, h.Hashtag
), cte2 AS 
(
	SELECT 
		CalendarDate ,
		RN,
		cte.Hashtag + ' (' + CONVERT(VARCHAR(20),cte.CNT) + ')' AS HashtagAndCountOfTweets
	FROM cte
	WHERE RN <= 20
)
SELECT 
	*
FROM cte2
PIVOT
(
  MAX(HashtagAndCountOfTweets)
  FOR CalendarDate IN ([20170920],[20170921],[20170922],[20170923],[20170924],[20170925],[20170926],[20170927],[20170928],[20170929],[20170930])
) piv