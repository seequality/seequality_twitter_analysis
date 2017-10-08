
CREATE PROCEDURE [Internal].[sp_InsertTMTweet]
(
	@TweetID INT, 
	@OriginalTweetWithoutSpecialCharacters NVARCHAR(500),
	@OriginalTweetEnglishWordsOnly NVARCHAR(500),
	@OriginalTweetEnglishWordsOnlyWithoutStopWords NVARCHAR(500) 
)
AS
INSERT INTO TextMining.Tweet
(
	[TweetID], 
	[OriginalTweetWithoutSpecialCharacters], 
	[OriginalTweetEnglishWordsOnly], 
	[OriginalTweetEnglishWordsOnlyWithoutStopWords]
)
VALUES 
(
	@TweetID, 
	@OriginalTweetWithoutSpecialCharacters, 
	@OriginalTweetEnglishWordsOnly, 
	@OriginalTweetEnglishWordsOnlyWithoutStopWords
)