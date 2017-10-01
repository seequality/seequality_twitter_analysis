


CREATE PROCEDURE [dbo].[sp_InsertFileContent] 
(
	@FilePath VARCHAR(1000),
	@FileSize [BIGINT],
	@HTMLDocument [NVARCHAR](MAX)
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