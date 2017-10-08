


CREATE PROCEDURE [Internal].[sp_InsertToken2Gram]
(
	@TweetID INT,
	@Token NVARCHAR(500) = NULL
)
AS
INSERT INTO TextMining.Token2Gram
(
	[TweetID],
	[Token]
)
VALUES	
(
	@TweetID,
	@Token
)