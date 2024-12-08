CREATE VIEW [dbo].[vw_TaggedLinks]
WITH SCHEMABINDING
AS
SELECT l.[Id]
      ,l.[Name]
      ,[Domain]
      ,[Url]
      ,[Description]
      ,[AddedBy]
      ,[IsSubdomain]
      ,[IsMultilingual]
      ,[IsResource]
      ,[Category]
      ,[Language]
      ,[Rating]
      ,[Aesthetics]
      ,[Type]
      ,[Country]
      ,[KeyPersonName]
      ,[Grammar]
      ,[Visibility]
      ,l.[CreatedAt]
      ,l.[ModifiedAt]
      ,[ThumbnailUrl]
      ,[FaviconUrl]
	  ,STRING_AGG(t.[Name], ',') AS Tags
  FROM [dbo].[Links] l
  JOIN [dbo].[LinksTags] lt
  ON lt.LinkId = l.Id
  JOIN [dbo].[Tags] t
  ON lt.TagId = t.Id
  GROUP BY
	  l.[Id]
      ,l.[Name]
      ,[Domain]
      ,[Url]
      ,[Description]
      ,[AddedBy]
      ,[IsSubdomain]
      ,[IsMultilingual]
      ,[IsResource]
      ,[Category]
      ,[Language]
      ,[Rating]
      ,[Aesthetics]
      ,[Type]
      ,[Country]
      ,[KeyPersonName]
      ,[Grammar]
      ,[Visibility]
      ,l.[CreatedAt]
      ,l.[ModifiedAt]
      ,[ThumbnailUrl]
      ,[FaviconUrl];