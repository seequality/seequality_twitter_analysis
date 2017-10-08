


CREATE PROCEDURE [Internal].[sp_GetTweets]
AS
BEGIN

	SELECT DISTINCT TweetID ,
			TweetText
	FROM   Internal.Tweet

END