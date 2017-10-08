
CREATE VIEW [Reporting].[Time] 
AS
WITH cte AS 
(
	SELECT 
	TOP 1440
		DATEADD(MINUTE, ROW_NUMBER() OVER(ORDER BY a.name ASC) - 1, '1900-01-01 00:00:00') AS FullTime
	FROM sys.system_objects a
	CROSS JOIN sys.system_objects b
), cte2 AS 
(
	SELECT 
	LEFT(REPLACE(CONVERT(VARCHAR(8), FullTime, 108), ':',''),4) [TimeID],
	LEFT(CONVERT(VARCHAR(8), FullTime, 108),5) AS [TimeName],
	SUBSTRING(CONVERT(VARCHAR(8), FullTime, 108),1,2) AS [Hour],
	SUBSTRING(CONVERT(VARCHAR(8), FullTime, 108),4,2) AS [Minute],
	CASE 
		WHEN CONVERT(INT,SUBSTRING(CONVERT(VARCHAR(8), FullTime, 108),1,2)) >= 12 THEN 'PM'
		ELSE 'AM'
	END AS [DayPart]
	FROM cte
)
SELECT 
	CONVERT(INT,[TimeID]) AS [TimeID],
    CONVERT(VARCHAR(5),[TimeName]) AS [TimeName],
    CONVERT(VARCHAR(2),[Hour]) AS [Hour],
    CONVERT(VARCHAR(2),[Minute]) AS [Minute],
    CONVERT(VARCHAR(2),[DayPart]) AS [DayPart]  
FROM cte2