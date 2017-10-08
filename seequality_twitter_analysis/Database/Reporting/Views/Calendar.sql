




CREATE VIEW [Reporting].[Calendar]
AS
WITH cte AS
(
SELECT 
TOP 
	(
		SELECT 
		DATEDIFF(
			d, 
			(SELECT DATEADD(yy, DATEDIFF(yy, 0, MIN(CONVERT(DATE,DateID))), 0) AS StartOfYear FROM Reporting.Tweet), 
			(SELECT DATEADD(yy, DATEDIFF(yy, 0, MIN(CONVERT(DATE,DateID)))+ 1, -1) AS StartOfYear FROM Reporting.Tweet)
		) + 1
	)
	DATEADD(DAY, ROW_NUMBER() OVER(ORDER BY a.name ASC) - 1, (SELECT DATEADD(yy, DATEDIFF(yy, 0, MIN(CONVERT(DATE,DateID))), 0) AS StartOfYear FROM Reporting.Tweet)) AS FullTime
FROM sys.system_objects a
CROSS JOIN sys.system_objects b
)
SELECT 
	CONVERT(VARCHAR(8), FullTime, 112) AS DateID,
	CONVERT(DATE, cte.FullTime) AS [Date],
	FORMAT(cte.FullTime,'yyyy') AS [Year],
	FORMAT(cte.FullTime,'MM') AS [Month],
	FORMAT(cte.FullTime,'dd') AS [Day],
	FORMAT(cte.FullTime,'MMMM') AS [MonthName],
	FORMAT(cte.FullTime,'dddd') AS [DayName]
FROM cte