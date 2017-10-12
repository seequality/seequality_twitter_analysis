/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4206)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

--    Target Server Version : SQL Server 2016
--    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
--    Target Database Engine Type : Standalone SQL Server
--*/

--USE [TwitterAnalysis]
--GO

--/****** Object:  View [Reporting].[Time]    Script Date: 2017-10-08 5:59:28 PM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO


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
	CONVERT(VARCHAR(4),[TimeID]) AS [TimeID],
    CONVERT(VARCHAR(5),[TimeName]) AS [TimeName],
    CONVERT(VARCHAR(2),[Hour]) AS [Hour],
    CONVERT(VARCHAR(2),[Minute]) AS [Minute],
    CONVERT(VARCHAR(2),[DayPart]) AS [DayPart]  
FROM cte2