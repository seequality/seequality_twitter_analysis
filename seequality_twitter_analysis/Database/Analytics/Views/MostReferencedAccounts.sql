
CREATE VIEW [Analytics].[MostReferencedAccounts] AS 
WITH cte AS 
(
	SELECT 
		CONVERT(DATE,rt.DateID) AS CalendarDate, 
		a.Account, 
		COUNT(*) AS CNT, 
		ROW_NUMBER() OVER(PARTITION BY rt.DateID ORDER BY COUNT(*) DESC) AS RN
	FROM Reporting.Tweet rt
	INNER JOIN Reporting.Account a
		ON rt.TweetID = a.TweetID
	GROUP BY rt.DateID, a.Account
), cte2 AS 
(
	SELECT 
		CalendarDate ,
		RN,
		cte.Account + ' (' + CONVERT(VARCHAR(20),cte.CNT) + ')' AS AccountsAndCountOfTweets
	FROM cte
	WHERE RN <= 20
)
SELECT 
	*
FROM cte2
PIVOT
(
  MAX(AccountsAndCountOfTweets)
  FOR CalendarDate IN ([20170920],[20170921],[20170922],[20170923],[20170924],[20170925],[20170926],[20170927],[20170928],[20170929],[20170930])
) piv