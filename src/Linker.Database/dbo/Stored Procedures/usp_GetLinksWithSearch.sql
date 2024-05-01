-- =============================================
-- Author:		Shaun Chong
-- Create date: May 1, 2024
-- Description:	Search links by keyword
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetLinksWithSearch]
	-- Add the parameters for the stored procedure here
	@Keyword nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM [dbo].[Links]
	WHERE
		[Name] LIKE '%' + @Keyword + '%' OR
		[Description] LIKE '%' + @Keyword + '%' OR
		[Domain] LIKE '%' + @Keyword + '%' OR
		[KeyPersonName] LIKE '%' + @Keyword + '%' OR
		[Country] LIKE '%' + @Keyword + '%';
END