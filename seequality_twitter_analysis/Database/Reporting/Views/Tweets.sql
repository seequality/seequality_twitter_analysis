
CREATE VIEW [Reporting].[Tweets] 
AS 
SELECT 
	it.TweetID ,
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
    it.NumberOfReplies ,
    it.NumberOfRetweets ,
    it.NumberOFFavourites ,
    it.NumberOfErrorsDuringParsing ,
    tt.OriginalTweetWithoutSpecialCharacters ,
    tt.OriginalTweetEnglishWordsOnly ,
    tt.OriginalTweetEnglishWordsOnlyWithoutStopWords 
FROM Internal.Tweet it
INNER JOIN TextMining.Tweet tt
ON it.TweetID = tt.TweetID