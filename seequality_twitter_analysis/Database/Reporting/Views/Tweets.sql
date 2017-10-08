

CREATE VIEW [Reporting].[Tweets] 
AS 
SELECT 
	it.TweetID ,
	CONVERT(VARCHAR(8), it.DateTime, 112) AS DateID ,
	LEFT(REPLACE(CONVERT(VARCHAR(8), it.DateTime, 108), ':',''),4) AS TimeID ,
    it.UserAddressName ,
    it.UserID ,
    it.UserImagePath ,
    it.UserFullName ,
    it.UserTwitterName ,
    it.Date ,
    it.StatusPath ,
    it.DateTimeTitle ,
    it.ConversationID ,
    it.OriginalDateTime ,
    it.OriginalDateTimeMS ,
    it.DateTime ,
    it.TweetText ,
    it.TweetLanguage ,
    it.TweetLanguageName ,
    it.TweetMediaName ,
    it.TweetMediaType ,
    tt.OriginalTweetWithoutSpecialCharacters ,
    tt.OriginalTweetEnglishWordsOnly ,
    tt.OriginalTweetEnglishWordsOnlyWithoutStopWords,
	it.NumberOfReplies ,
    it.NumberOfRetweets ,
    it.NumberOFFavourites ,
    it.NumberOfErrorsDuringParsing,
	LEN(tt.OriginalTweetWithoutSpecialCharacters) AS TwitterLengthText
FROM Internal.Tweet it
INNER JOIN TextMining.Tweet tt
ON it.TweetID = tt.TweetID