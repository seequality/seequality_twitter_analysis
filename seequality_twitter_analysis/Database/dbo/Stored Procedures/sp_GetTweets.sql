
CREATE PROCEDURE [dbo].[sp_GetTweets]
AS
BEGIN

	SELECT DISTINCT TweetID ,
			TweetText
	FROM   dbo.Tweet

END