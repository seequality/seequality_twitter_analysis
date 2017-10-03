CREATE PROCEDURE [dbo].[sp_InsertTweet] 
(
	@ExecutionID INT,
	@UserAddressName [VARCHAR](500) = NULL,
	@UserID [BIGINT] = NULL,
	@UserImagePath [VARCHAR](500) = NULL,
	@UserFullName [NVARCHAR](500) = NULL,
	@UserTwitterName [VARCHAR](500) = NULL,
	@Date [VARCHAR](500) = NULL,
	@StatusPath [VARCHAR](500) = NULL,
	@DateTimeTitle [VARCHAR](500) = NULL,
	@ConversationID [BIGINT] = NULL,
	@OriginalDateTime [BIGINT] = NULL,
	@OriginalDateTimeMS [BIGINT] = NULL,
	@DateTime [DATETIME2](7) = NULL,
	@TweetText [NVARCHAR](500) = NULL,
	@TweetLanguage [VARCHAR](500) = NULL,
	@TweetMediaName [VARCHAR](500) NULL,
	@TweetMediaType [VARCHAR](500) NULL,
	@NumberOfReplies [INT] = NULL,
	@NumberOfRetweets [INT] = NULL,
	@NumberOFFavourites [INT] = NULL,
	@NumberOfErrorsDuringParsing [INT] = NULL
)
AS
INSERT INTO [dbo].[Tweet]
(
	[ExecutionID]
	,[UserAddressName]
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
	,[TweetMediaName]
	,[TweetMediaType]
	,[NumberOfReplies]
	,[NumberOfRetweets]
	,[NumberOFFavourites]
	,[NumberOfErrorsDuringParsing]
)
VALUES	
(
	@ExecutionID,
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
	@TweetMediaName,
	@TweetMediaType,
	@NumberOfReplies,
	@NumberOfRetweets,
	@NumberOFFavourites,
	@NumberOfErrorsDuringParsing
)