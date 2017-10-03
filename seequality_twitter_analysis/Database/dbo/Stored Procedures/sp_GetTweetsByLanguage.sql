
CREATE PROCEDURE [dbo].[sp_GetTweetsByLanguage]
(
	@Language VARCHAR(500) = NULL
)
AS
BEGIN

	SELECT DISTINCT TweetID ,
			TweetText
	FROM   dbo.Tweet
	WHERE TweetLanguage = @Language

END