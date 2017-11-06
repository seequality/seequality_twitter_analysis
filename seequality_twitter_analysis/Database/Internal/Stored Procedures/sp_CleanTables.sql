

CREATE PROCEDURE [Internal].[sp_CleanTables]
(
	@CleanSource BIT
)
AS 
BEGIN

IF @CleanSource = 1 
	BEGIN 

		TRUNCATE TABLE [Internal].[Executions]
		TRUNCATE TABLE [Internal].[FileContent]
		TRUNCATE TABLE [Internal].[Tweet]
	END

	TRUNCATE TABLE [TextMining].[Hashtags]
	TRUNCATE TABLE [TextMining].[Accounts]
	TRUNCATE TABLE [TextMining].[Tweet]
	TRUNCATE TABLE [TextMining].[Token1Gram]
	TRUNCATE TABLE [TextMining].[Token2Gram]
	TRUNCATE TABLE [TextMining].[Token3Gram]
	TRUNCATE TABLE [TextMining].[Token4Gram]

END