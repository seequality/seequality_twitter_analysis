

CREATE PROCEDURE [dbo].[sp_LogStop]
(
	@ExecutionID INT
)
AS
BEGIN 
	UPDATE [dbo].[Executions]
	SET EndTime = GETDATE()
	WHERE ExecutionID = @ExecutionID
END