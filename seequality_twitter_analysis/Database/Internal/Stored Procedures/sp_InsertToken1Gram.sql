




CREATE PROCEDURE [Internal].[sp_InsertToken1Gram]
(
	@TweetID INT,
	@Token NVARCHAR(500) = NULL,
	@TokenRootWord NVARCHAR(500) = NULL,
	@IsEnglishWord BIT = NULL,
	@IsStopWord BIT = NULL,
	@IsNotEnglishWordAndNotStopWord BIT = NULL,
	@IsHashtag BIT = NULL,
	@IsAccountName BIT = NULL,
	@IsNumber BIT = NULL,
	@IsWebsiteUrl BIT = NULL
)
AS
INSERT INTO TextMining.Token1Gram
(
	[TweetID],
	[Token],
	[TokenRootWord],
	[IsEnglishWord],
	[IsStopWord],
	[IsNotEnglishWordAndNotStopWord],
	[IsHashtag],
	[IsAccountName],
	[IsNumber],
	[IsWebsiteUrl]
)
VALUES	
(
	@TweetID,
	@Token,
	@TokenRootWord,
	@IsEnglishWord,
	@IsStopWord,
	@IsNotEnglishWordAndNotStopWord,
	@IsHashtag,
	@IsAccountName,
	@IsNumber,
	@IsWebsiteUrl
)