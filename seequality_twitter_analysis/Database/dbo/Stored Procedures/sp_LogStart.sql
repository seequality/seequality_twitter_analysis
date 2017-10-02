CREATE PROCEDURE dbo.sp_LogStart
(
	@ExecutionID INT OUTPUT
)
AS
BEGIN 
	INSERT INTO [dbo].[Executions] (StartTime) VALUES (GETDATE())
	SELECT @ExecutionID = SCOPE_IDENTITY()
END