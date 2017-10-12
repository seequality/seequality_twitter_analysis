



CREATE VIEW [Reporting].[Tweet] 
AS 
SELECT 
	it.TweetID ,
	--CONVERT(VARCHAR(8), it.DateTime, 112) AS CalendarID, -- should be fixed on the c# part
	IIF(it.OriginalDateTimeMS > 0, CONVERT(VARCHAR(8), it.DateTime, 112),CONVERT(VARCHAR(8),CONVERT(DATETIME, RTRIM(LTRIM(SUBSTRING(it.DateTimeTitle,CHARINDEX('-',it.DateTimeTitle) + 1, 20)))), 112) ) AS DateID ,
	--LEFT(REPLACE(CONVERT(VARCHAR(8), it.DateTime, 108), ':',''),4) AS TimeID , -- should be fixed on the c# part
	CASE WHEN it.OriginalDateTimeMS > 0 THEN LEFT(REPLACE(CONVERT(VARCHAR(8), it.DateTime, 108), ':',''),4)
	ELSE 
		CASE 
		WHEN it.DateTimeTitle LIKE '%AM%' THEN 
			CASE REPLACE(SUBSTRING(REPLACE(RTRIM(LTRIM(SUBSTRING(it.DateTimeTitle,1, CHARINDEX('-',it.DateTimeTitle)))), '-',''), 1, CHARINDEX(':',it.DateTimeTitle)), ':','')
				WHEN 12 THEN '00'
				WHEN 1 THEN '01'
				WHEN 2 THEN '02'
				WHEN 3 THEN '03'
				WHEN 4 THEN '04'
				WHEN 5 THEN '05'
				WHEN 6 THEN '06'
				WHEN 7 THEN '07'
				WHEN 8 THEN '08'
				WHEN 9 THEN '09' 
				ELSE REPLACE(SUBSTRING(REPLACE(RTRIM(LTRIM(SUBSTRING(it.DateTimeTitle,1, CHARINDEX('-',it.DateTimeTitle)))), '-',''), 1, CHARINDEX(':',it.DateTimeTitle)), ':','')
			END  
		WHEN it.DateTimeTitle LIKE '%PM%' THEN 
			CASE REPLACE(SUBSTRING(REPLACE(RTRIM(LTRIM(SUBSTRING(it.DateTimeTitle,1, CHARINDEX('-',it.DateTimeTitle)))), '-',''), 1, CHARINDEX(':',it.DateTimeTitle)), ':','')
				WHEN 1 THEN '13'
				WHEN 2 THEN '14'
				WHEN 3 THEN '15'
				WHEN 4 THEN '16'
				WHEN 5 THEN '17'
				WHEN 6 THEN '18'
				WHEN 7 THEN '19'
				WHEN 8 THEN '20'
				WHEN 9 THEN '21'
				WHEN 10 THEN '22'
				WHEN 11 THEN '23'
			ELSE REPLACE(SUBSTRING(REPLACE(RTRIM(LTRIM(SUBSTRING(it.DateTimeTitle,1, CHARINDEX('-',it.DateTimeTitle)))), '-',''), 1, CHARINDEX(':',it.DateTimeTitle)), ':','')
		END
		ELSE '00'
	END 
	+ 
	REPLACE(SUBSTRING(REPLACE(RTRIM(LTRIM(SUBSTRING(it.DateTimeTitle,1, CHARINDEX('-',it.DateTimeTitle)))), '-',''),CHARINDEX(':',it.DateTimeTitle)+1, 2 ), ':','')
	END AS TimeID, 
    it.UserTwitterName ,
    it.ConversationID ,
	it.StatusPath,
    it.TweetText ,
    it.TweetLanguageName ,
    it.TweetMediaName ,
    it.TweetMediaType ,
    ISNULL(tt.OriginalTweetWithoutSpecialCharacters, 'N/A') AS OriginalTweetWithoutSpecialCharacters ,
    ISNULL(tt.OriginalTweetEnglishWordsOnly, 'N/A') AS OriginalTweetEnglishWordsOnly ,
    ISNULL(tt.OriginalTweetEnglishWordsOnlyWithoutStopWords, 'N/A') AS OriginalTweetEnglishWordsOnlyWithoutStopWords,
	it.NumberOfReplies ,
    it.NumberOfRetweets ,
    it.NumberOFFavourites ,
	LEN(IIF(tt.OriginalTweetWithoutSpecialCharacters IS NULL, it.TweetText, tt.OriginalTweetWithoutSpecialCharacters)) AS TwitterLengthText
FROM Internal.Tweet it (NOLOCK)
LEFT JOIN TextMining.Tweet tt (NOLOCK)
ON it.TweetID = tt.TweetID
WHERE it.TweetID IN 
(
	SELECT TweetID FROM Internal.DistinctTweet
)