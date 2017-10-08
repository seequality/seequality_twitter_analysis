


CREATE PROCEDURE [Internal].[sp_GetTweetsByLanguage]
(
	@Language VARCHAR(500) = NULL
)
AS
BEGIN

	SELECT DISTINCT TweetID ,
			TweetText
	FROM   Internal.Tweet
	WHERE TweetLanguage = @Language

END