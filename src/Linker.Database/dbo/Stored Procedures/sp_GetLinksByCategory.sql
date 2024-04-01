-- =============================================
-- Author:		Shaun Chong
-- Create date: 1 April, 2024
-- Description:	Retrieve links by category
-- =============================================
CREATE PROCEDURE sp_GetLinksByCategory 
	-- Add the parameters for the stored procedure here
	@Category nvarchar(50) = 'None'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Links WHERE Category = @Category;
END