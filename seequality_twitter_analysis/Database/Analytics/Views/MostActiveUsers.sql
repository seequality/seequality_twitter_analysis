
CREATE VIEW Analytics.MostActiveUsers AS 
WITH cte AS 
(
	SELECT 
		CONVERT(DATE,rt.DateID) AS CalendarDate, 
		rt.UserTwitterName, 
		COUNT(*) AS CNT, 
		ROW_NUMBER() OVER(PARTITION BY rt.DateID ORDER BY COUNT(*) desc) AS RN
	FROM Reporting.Tweet rt
	GROUP BY rt.DateID, rt.UserTwitterName
), cte2 AS 
(
	SELECT 
		CalendarDate ,
		RN,
		UserTwitterName + ' (' + CONVERT(VARCHAR(20),cte.CNT) + ')' AS UserTwitterNameAndCountOfTweets
	FROM cte
	WHERE RN <= 20
)
SELECT 
	*
FROM cte2
pivot
(
  max(UserTwitterNameAndCountOfTweets)
  for CalendarDate in ([20170920],[20170921],[20170922],[20170923],[20170924],[20170925],[20170926],[20170927],[20170928],[20170929],[20170930])
) piv