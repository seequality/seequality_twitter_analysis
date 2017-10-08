
CREATE PROCEDURE [Internal].[sp_InsertTMHashtag]
(
	@TweetID INT,
	@Hashtag NVARCHAR(500),
	@IsOnTheWhiteList BIT
)
AS

INSERT INTO TextMining.Hashtags 
(
	TweetID,
	Hashtag,
	IsOnTheWhiteList
)
VALUES
(
	@TweetID,
	@Hashtag,
	@IsOnTheWhiteList
)