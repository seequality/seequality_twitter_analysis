CREATE TABLE [dbo].[Executions] (
    [ExecutionID] INT      IDENTITY (1, 1) NOT NULL,
    [StartTime]   DATETIME NULL,
    [EndTime]     DATETIME NULL,
    CONSTRAINT [PK_dbo_Executions] PRIMARY KEY CLUSTERED ([ExecutionID] ASC)
);



