

CREATE PROCEDURE [Internal].[sp_InsertToken3Gram]
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