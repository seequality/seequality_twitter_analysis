



CREATE VIEW [Reporting].[Token1Gram]
AS
SELECT 
      [TweetID]
      ,[Token]
      ,ISNULL([TokenRootWord], 'N/A') AS [TokenRootWord]
      ,[IsEnglishWord]
      ,[IsStopWord]
      ,[IsNotEnglishWordAndNotStopWord]
      ,[IsHashtag]
      ,[IsAccountName]
      ,[IsNumber]
  FROM [TextMining].[Token1Gram] (NOLOCK)
  WHERE TweetID IN 
(
	SELECT TweetID FROM Internal.DistinctTweet
)