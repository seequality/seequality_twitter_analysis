






CREATE PROCEDURE [Internal].[sp_InsertFileContent] 
(
	@ExecutionID INT,
	@FilePath VARCHAR(1000) = NULL,
	@FileSize [BIGINT] = NULL,
	@HTMLDocument [NVARCHAR](MAX) = NULL
)
AS 
INSERT INTO [Internal].[FileContent]
(
	[ExecutionID]
	,[FilePath]
	,[FileSize]
	,[HTMLDocument]
)
VALUES
(
	@ExecutionID,
	@FilePath,
	@FileSize,
	@HTMLDocument
)