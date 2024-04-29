-- =============================================
-- Author:		Shaun Chong
-- Create date: 29 April, 2024
-- Description:	Filter retrieved links
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetLinksWithFilters] 
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50) = NULL,
	@Description nvarchar(200) = NULL,
	@Domain nvarchar(50) = NULL,
	@Category nvarchar(50) = NULL,
	@Language nvarchar(50) = NULL,
	@Rating nvarchar(50) = NULL,
	@Aesthetics nvarchar(50) = NULL,
	@Grammar nvarchar(50) = NULL,
	@Country nvarchar(50) = NULL,
	@KeyPersonName nvarchar(50) = NULL,
	@CreatedAtStart datetime2(7) = NULL,
	@CreatedAtEnd datetime2(7) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM [dbo].[Links]
	WHERE
		((@Name IS NULL) OR ([Name] LIKE '%' + @Name + '%')) AND
		((@Description IS NULL) OR ([Description] LIKE '%' + @Description + '%')) AND
		((@Domain IS NULL) OR ([Domain] LIKE '%' + @Domain + '%')) AND
		((@Category IS NULL) OR ([Category] = @Category)) AND
		((@Language IS NULL) OR ([Language] = @Language)) AND
		((@Rating IS NULL) OR ([Rating] = @Rating)) AND
		((@Aesthetics IS NULL) OR ([Aesthetics] = @Aesthetics)) AND
		((@Grammar IS NULL) OR ([Grammar] = @Grammar)) AND
		((@Country IS NULL) OR ([Country] LIKE '%' + @Country + '%')) AND
		((@KeyPersonName IS NULL) OR ([KeyPersonName] LIKE '%' + @KeyPersonName + '%')) AND
		((@CreatedAtStart IS NULL OR ((@CreatedAtEnd IS NULL) OR ([CreatedAt] BETWEEN @CreatedAtStart AND @CreatedAtEnd)) OR ([CreatedAt] <= @CreatedAtStart)));
END