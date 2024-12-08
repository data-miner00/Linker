-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetLinksWithMatchingTag]
	@Tag NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT * FROM [dbo].[vw_TaggedLinks]
	WHERE [dbo].[vw_TaggedLinks].[Tags] LIKE '%' + @Tag + '%'
END