


CREATE PROCEDURE [dbo].[sp_InsertToken1Gram]
(
	@TweetID INT,
	@Token NVARCHAR(500),
	@TokenRootWord nvarchar(500),
	@IsEnglishWord BIT,
	@IsStopWord BIT,
	@IsNotEnglishWordAndNotStopWord BIT,
	@IsHashtag BIT,
	@IsAccountName BIT,
	@IsNumber BIT,
	@IsWebsiteUrl BIT
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