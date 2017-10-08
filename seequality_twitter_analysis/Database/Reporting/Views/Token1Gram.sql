﻿

CREATE VIEW [Reporting].[Token1Gram]
AS
SELECT 
      [TweetID]
      ,[Token]
      ,[TokenRootWord]
      ,[IsEnglishWord]
      ,[IsStopWord]
      ,[IsNotEnglishWordAndNotStopWord]
      ,[IsHashtag]
      ,[IsAccountName]
      ,[IsNumber]
  FROM [TextMining].[Token1Gram]