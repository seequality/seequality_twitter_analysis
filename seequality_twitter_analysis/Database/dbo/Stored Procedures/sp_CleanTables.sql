





CREATE PROCEDURE [dbo].[sp_CleanTables]
(
	@CleanSource BIT
)
AS 
BEGIN

IF @CleanSource = 1 
	BEGIN 

		TRUNCATE TABLE [dbo].[Executions]
		TRUNCATE TABLE [dbo].[FileContent]
		TRUNCATE TABLE [dbo].[Tweet]
	END

	TRUNCATE TABLE [TextMining].[Hashtags]
	TRUNCATE TABLE [TextMining].[Accounts]
	TRUNCATE TABLE [TextMining].[Tweet]
	TRUNCATE TABLE [TextMining].[Token1Gram]
	TRUNCATE TABLE [TextMining].[Token2Gram]

END