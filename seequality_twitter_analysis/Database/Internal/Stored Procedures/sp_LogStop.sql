



CREATE PROCEDURE [Internal].[sp_LogStop]
(
	@ExecutionID INT
)
AS
BEGIN 
	UPDATE [Internal].[Executions]
	SET EndTime = GETDATE()
	WHERE ExecutionID = @ExecutionID
END