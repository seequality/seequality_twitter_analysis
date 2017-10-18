

CREATE PROCEDURE [Internal].[sp_InsertTokenGram]
(
	@TweetID INT,
	@Token NVARCHAR(500) = NULL
)
AS
INSERT INTO TextMining.Token3Gram
(
	[TweetID],
	[Token]
)
VALUES	
(
	@TweetID,
	@Token
)