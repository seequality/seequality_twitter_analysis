
CREATE VIEW [Analytics].[MostUsedToken2] AS 
WITH cte AS 
(
	SELECT 
		CONVERT(DATE,rt.DateID) AS CalendarDate, 
		t1.Token, 
		COUNT(*) AS CNT, 
		ROW_NUMBER() OVER(PARTITION BY rt.DateID ORDER BY COUNT(*) DESC) AS RN
	FROM Reporting.Tweet rt
	INNER JOIN Reporting.Token2Gram t1
		ON rt.TweetID = t1.TweetID
	GROUP BY rt.DateID, t1.Token
), cte2 AS 
(
	SELECT 
		CalendarDate ,
		RN,
		cte.Token + ' (' + CONVERT(VARCHAR(20),cte.CNT) + ')' AS TokenAndCountOfTweets
	FROM cte
	WHERE RN <= 20
)
SELECT 
	*
FROM cte2
PIVOT
(
  MAX(TokenAndCountOfTweets)
  FOR CalendarDate IN ([20170920],[20170921],[20170922],[20170923],[20170924],[20170925],[20170926],[20170927],[20170928],[20170929],[20170930])
) piv