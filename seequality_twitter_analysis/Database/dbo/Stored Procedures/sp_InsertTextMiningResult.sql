
CREATE PROCEDURE [dbo].[sp_InsertTextMiningResult]
(
	@TweetID INT,
	@TextMiningMethodID SMALLINT,
	@TweetText NVARCHAR(500)
)
AS
INSERT INTO [dbo].[TextMiningResult]
(
	[TweetID], 
	[TextMiningMethodID], 
	[TweetText]
)
VALUES 
(
	@TweetID,
	@TextMiningMethodID,
	@TweetText
)