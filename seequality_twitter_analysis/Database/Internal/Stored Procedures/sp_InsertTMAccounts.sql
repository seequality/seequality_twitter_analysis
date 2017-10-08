

CREATE PROCEDURE [Internal].[sp_InsertTMAccounts]
(
	@TweetID INT,
	@Account NVARCHAR(500)
)
AS

INSERT INTO TextMining.Accounts
(
	TweetID,
	Account
)
VALUES
(
	@TweetID,
	@Account
)