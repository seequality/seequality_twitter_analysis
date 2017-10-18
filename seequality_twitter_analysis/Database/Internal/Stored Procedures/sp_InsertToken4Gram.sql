
CREATE PROCEDURE [Internal].[sp_InsertToken4Gram]
(
	@TweetID INT,
	@Token NVARCHAR(500) = NULL
)
AS
INSERT INTO TextMining.Token4Gram
(
	[TweetID],
	[Token]
)
VALUES	
(
	@TweetID,
	@Token
)