
CREATE PROCEDURE [dbo].[sp_InsertTweet] 
(
	@UserAddressName [VARCHAR](500),
	@UserID [BIGINT],
	@UserImagePath [VARCHAR](500),
	@UserFullName [VARCHAR](500),
	@UserTwitterName [VARCHAR](500),
	@Date [VARCHAR](500),
	@StatusPath [VARCHAR](500),
	@DateTimeTitle [VARCHAR](500),
	@ConversationID [BIGINT],
	@OriginalDateTime [BIGINT],
	@OriginalDateTimeMS [BIGINT],
	@DateTime [DATETIME2](7),
	@TweetText [VARCHAR](500),
	@TweetLanguage [VARCHAR](500),
	@TweetImagePath [VARCHAR](500),
	@NumberOfReplies [INT],
	@NumberOfRetweets [INT],
	@NumberOFFavourites [INT],
	@NumberOfErrorsDuringParsing [INT]
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