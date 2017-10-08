

CREATE PROCEDURE [Internal].[sp_LogStart]
(
	@ExecutionID INT OUTPUT
)
AS
BEGIN 
	INSERT INTO [Internal].[Executions] (StartTime) VALUES (GETDATE())
	SELECT @ExecutionID = SCOPE_IDENTITY()
END