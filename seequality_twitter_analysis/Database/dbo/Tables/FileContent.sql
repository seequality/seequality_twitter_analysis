CREATE TABLE [dbo].[FileContent] (
    [FileContentID] INT            IDENTITY (1, 1) NOT NULL,
    [ExecutionID]   INT            NOT NULL,
    [FilePath]      VARCHAR (1000) NULL,
    [FileSize]      BIGINT         NULL,
    [HTMLDocument]  NVARCHAR (MAX) NULL
);



