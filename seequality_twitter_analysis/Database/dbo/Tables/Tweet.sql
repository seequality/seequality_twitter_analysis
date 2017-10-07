﻿CREATE TABLE [dbo].[Tweet] (
    [TweetID]                     INT            IDENTITY (1, 1) NOT NULL,
    [ExecutionID]                 INT            NOT NULL,
    [UserAddressName]             VARCHAR (500)  NULL,
    [UserID]                      BIGINT         NULL,
    [UserImagePath]               VARCHAR (500)  NULL,
    [UserFullName]                NVARCHAR (500) NULL,
    [UserTwitterName]             VARCHAR (500)  NULL,
    [Date]                        VARCHAR (500)  NULL,
    [StatusPath]                  VARCHAR (500)  NULL,
    [DateTimeTitle]               VARCHAR (500)  NULL,
    [ConversationID]              BIGINT         NULL,
    [OriginalDateTime]            BIGINT         NULL,
    [OriginalDateTimeMS]          BIGINT         NULL,
    [DateTime]                    DATETIME2 (7)  NULL,
    [TweetText]                   NVARCHAR (500) NULL,
    [TweetLanguage]               VARCHAR (500)  NULL,
    [TweetLanguageName]           VARCHAR (500)  NULL,
    [TweetMediaName]              VARCHAR (500)  NULL,
    [TweetMediaType]              VARCHAR (500)  NULL,
    [NumberOfReplies]             INT            NULL,
    [NumberOfRetweets]            INT            NULL,
    [NumberOFFavourites]          INT            NULL,
    [NumberOfErrorsDuringParsing] INT            NULL,
    CONSTRAINT [PK_dbo_Tweet] PRIMARY KEY CLUSTERED ([TweetID] ASC)
);












GO
CREATE NONCLUSTERED INDEX [NCI_dbo_Tweet_TweetLanguage_INCL_TweetID_TweetText]
    ON [dbo].[Tweet]([TweetLanguage] ASC)
    INCLUDE([TweetID], [TweetText]);

