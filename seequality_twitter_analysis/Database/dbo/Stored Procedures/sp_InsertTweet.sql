

CREATE PROCEDURE [dbo].[sp_InsertTweet] 
(
	@UserAddressName [VARCHAR](500) = NULL,
	@UserID [BIGINT] = NULL,
	@UserImagePath [VARCHAR](500) = NULL,
	@UserFullName [VARCHAR](500) = NULL,
	@UserTwitterName [VARCHAR](500) = NULL,
	@Date [VARCHAR](500) = NULL,
	@StatusPath [VARCHAR](500) = NULL,
	@DateTimeTitle [VARCHAR](500) = NULL,
	@ConversationID [BIGINT] = NULL,
	@OriginalDateTime [BIGINT] = NULL,
	@OriginalDateTimeMS [BIGINT] = NULL,
	@DateTime [DATETIME2](7) = NULL,
	@TweetText [VARCHAR](500) = NULL,
	@TweetLanguage [VARCHAR](500) = NULL,
	@TweetImagePath [VARCHAR](500) = NULL,
	@NumberOfReplies [INT] = NULL,
	@NumberOfRetweets [INT] = NULL,
	@NumberOFFavourites [INT] = NULL,
	@NumberOfErrorsDuringParsing [INT] = NULL
)
AS
INSERT INTO [dbo].[Tweet]
(
	[UserAddressName]
	,[UserID]
	,[UserImagePath]
	,[UserFullName]
	,[UserTwitterName]
	,[Date]
	,[StatusPath]
	,[DateTimeTitle]
	,[ConversationID]
	,[OriginalDateTime]
	,[OriginalDateTimeMS]
	,[DateTime]
	,[TweetText]
	,[TweetLanguage]
	,[TweetImagePath]
	,[NumberOfReplies]
	,[NumberOfRetweets]
	,[NumberOFFavourites]
	,[NumberOfErrorsDuringParsing]
)
VALUES	
(
	@UserAddressName,
	@UserID,
	@UserImagePath,
	@UserFullName,
	@UserTwitterName,
	@Date,
	@StatusPath,
	@DateTimeTitle,
	@ConversationID,
	@OriginalDateTime,
	@OriginalDateTimeMS,
	@DateTime,
	@TweetText,
	@TweetLanguage,
	@TweetImagePath,
	@NumberOfReplies,
	@NumberOfRetweets,
	@NumberOFFavourites,
	@NumberOfErrorsDuringParsing
)