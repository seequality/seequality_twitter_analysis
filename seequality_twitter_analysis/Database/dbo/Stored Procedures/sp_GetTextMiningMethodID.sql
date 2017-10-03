

CREATE PROCEDURE [dbo].[sp_GetTextMiningMethodID] 
(
	@TextMiningMethodName VARCHAR(100),
	@TextMiningMethodID SMALLINT OUTPUT
)
AS
BEGIN

	SELECT @TextMiningMethodID = [TextMiningMethodID]
	FROM [TextMiningMethod]
	WHERE [TextMiningMethodName] = @TextMiningMethodName

END