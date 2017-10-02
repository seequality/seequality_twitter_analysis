



CREATE PROCEDURE [dbo].[sp_InsertFileContent] 
(
	@FilePath VARCHAR(1000) = NULL,
	@FileSize [BIGINT] = NULL,
	@HTMLDocument [NVARCHAR](MAX) = NULL
)
AS 
INSERT INTO [dbo].[FileContent]
(
	FilePath
	,[FileSize]
	,[HTMLDocument]
)
VALUES
(
	@FilePath,
	@FileSize,
	@HTMLDocument
)