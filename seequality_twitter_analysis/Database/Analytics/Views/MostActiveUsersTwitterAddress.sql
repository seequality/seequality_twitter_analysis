﻿


CREATE VIEW [Analytics].[MostActiveUsersTwitterAddress] AS 
WITH cte AS 
(
	SELECT 
		CONVERT(DATE,rt.DateID) AS CalendarDate, 
		it.UserAddressName, 
		COUNT(*) AS CNT, 
		ROW_NUMBER() OVER(PARTITION BY rt.DateID ORDER BY COUNT(*) DESC) AS RN
	FROM Reporting.Tweet rt
	INNER JOIN Internal.Tweet it
		ON it.TweetID = rt.TweetID
	GROUP BY rt.DateID, it.UserAddressName
), cte2 AS 
(
	SELECT 
		CalendarDate ,
		RN,
		UserAddressName 
	FROM cte
	WHERE RN <= 20
)
SELECT 
	*
FROM cte2
PIVOT
(
  MAX(UserAddressName)
  FOR CalendarDate IN ([20170920],[20170921],[20170922],[20170923],[20170924],[20170925],[20170926],[20170927],[20170928],[20170929],[20170930])
) piv