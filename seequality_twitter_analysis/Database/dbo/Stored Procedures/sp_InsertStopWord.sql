CREATE PROCEDURE dbo.sp_InsertStopWord
(
	@StopWord varchar(50)
)
AS
BEGIN

	INSERT INTO dbo.StopWords ( StopWord )
	VALUES ( @StopWord )

END