namespace Linker.Data.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Dapper;
    using EnsureThat;
    using Linker.Core.Models;
    using Linker.Core.Models.Dtos;
    using Linker.Core.Repositories;

    /// <summary>
    /// A repository for Youtube entity.
    /// </summary>
    public class YoutubeRepository : IYoutubeRepository
    {
        private readonly IDbConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="YoutubeRepository"/> class.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/>.</param>
        public YoutubeRepository(IDbConnection connection)
        {
            this.connection = EnsureArg.IsNotNull(connection, nameof(connection));
        }

        /// <inheritdoc/>
        public void Add(Youtube item)
        {
            var randomId = Guid.NewGuid().ToString();

            var insertToLinksOperation = @"
                INSERT INTO Links (
                    Id,
                    Url,
                    Category,
                    Description,
                    Language,
                    LastVisitAt,
                    CreatedAt,
                    ModifiedAt
                ) VALUES (
                    @Id,
                    @Url,
                    @Category,
                    @Description,
                    @Language,
                    @LastVisitAt,
                    @CreatedAt,
                    @ModifiedAt
                );
            ";

            var insertToYoutubeOperation = @"
                INSERT INTO Youtube (
                    LinkId,
                    Name,
                    Youtuber,
                    Country
                ) VALUES (
                    @LinkId,
                    @Name,
                    @Youtuber,
                    @Country
                );
            ";

            var selectIdFromTagsQuery = @"SELECT (Id) FROM Tags WHERE Name = @Name;";

            var insertIntoTagsOperation = @"
                INSERT INTO Tags (
                    Id,
                    Name,
                    CreatedAt,
                    ModifiedAt
                ) VALUES (
                    @Id,
                    @Name,
                    @CreatedAt,
                    @ModifiedAt
                );
            ";

            var insertIntoLinkTagsOperation = @"
                INSERT INTO Links_Tags (
                    LinkId,
                    TagId
                ) VALUES (
                    @LinkId,
                    @TagId
                );
            ";

            this.connection.Execute(insertToLinksOperation, new
            {
                Id = randomId,
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                item.LastVisitAt,
                item.CreatedAt,
                item.ModifiedAt,
            });

            this.connection.Execute(insertToYoutubeOperation, new
            {
                LinkId = randomId,
                item.Name,
                item.Youtuber,
                item.Country,
            });

            foreach (var tag in item.Tags)
            {
                var result = this.connection.Query<Tag>(selectIdFromTagsQuery, new { Name = tag });

                if (!result.Any())
                {
                    var randomId2 = Guid.NewGuid().ToString();
                    this.connection.Execute(insertIntoTagsOperation, new
                    {
                        Id = randomId2,
                        Name = tag,
                        item.CreatedAt,
                        item.ModifiedAt,
                    });
                    this.connection.Execute(insertIntoLinkTagsOperation, new { LinkId = randomId, TagId = randomId2 });
                }
                else
                {
                    this.connection.Execute(insertIntoLinkTagsOperation, new { LinkId = randomId, TagId = result.FirstOrDefault()?.Id });
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Youtube> GetAll()
        {
            var channels = new List<Youtube>();

            var selectFromYoutubeQuery = @"SELECT * FROM Youtube;";

            var partialChannels =
                this.connection.Query<PartialYoutube>(selectFromYoutubeQuery);

            foreach (var partialChannel in partialChannels)
            {
                var tags = new List<string>();

                var selectFromLinksQuery = @"SELECT * FROM Links WHERE Id =@Id;";
                var link = this.connection.QueryFirst<Link>(selectFromLinksQuery, new { Id = partialChannel.LinkId });

                var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @Id;";
                var tagsz = this.connection.Query<LinkTagPair>(selectFromLinksTagsQuery, new { Id = partialChannel.LinkId });

                foreach (var tagz in tagsz)
                {
                    var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                    var tag = this.connection.QueryFirst<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                    tags.Add(tag.Name);
                }

                var youtube = new Youtube
                {
                    Id = link.Id,
                    Url = link.Url,
                    Category = link.Category,
                    Description = link.Description,
                    Tags = tags,
                    Language = link.Language,
                    LastVisitAt = link.LastVisitAt,
                    CreatedAt = link.CreatedAt,
                    ModifiedAt = link.ModifiedAt,
                    Name = partialChannel.Name,
                    Youtuber = partialChannel.Youtuber,
                    Country = partialChannel.Country,
                };

                channels.Add(youtube);
            }

            return channels;
        }

        /// <inheritdoc/>
        public Youtube GetById(string id)
        {
            var tags = new List<string>();

            var partialChannel = this.TryGetItem(id);

            var selectFromLinksQuery = @"SELECT * FROM Links WHERE Id = @Id;";

            var link = this.connection.QueryFirst<Link>(selectFromLinksQuery, new { Id = id });

            var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";

            var tagsz = this.connection.Query<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = id });

            foreach (var tagz in tagsz)
            {
                var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";
                var tag = this.connection.QueryFirst<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                tags.Add(tag.Name);
            }

            var youtube = new Youtube
            {
                Id = link.Id,
                Url = link.Url,
                Category = link.Category,
                Description = link.Description,
                Tags = tags,
                Language = link.Language,
                LastVisitAt = link.LastVisitAt,
                CreatedAt = link.CreatedAt,
                ModifiedAt = link.ModifiedAt,
                Name = partialChannel.Name,
                Youtuber = partialChannel.Youtuber,
                Country = partialChannel.Country,
            };

            return youtube;
        }

        /// <inheritdoc/>
        public void Remove(string id)
        {
            this.TryGetItem(id);

            var deleteFromYoutubeOperation = @"DELETE FROM Youtube WHERE LinkId = @Id;";
            var deleteFromLinksOperation = @"DELETE FROM Links WHERE Id = @Id;";
            var deleteFromLinksTagsOperation = @"DELETE FROM Links_Tags WHERE LinkId = @Id;";

            this.connection.Execute(deleteFromYoutubeOperation, new { Id = id });
            this.connection.Execute(deleteFromLinksOperation, new { Id = id });
            this.connection.Execute(deleteFromLinksTagsOperation, new { Id = id });
        }

        /// <inheritdoc/>
        public void Update(Youtube item)
        {
            this.TryGetItem(item.Id);

            var updateYoutubeOperation = @"
                UPDATE Youtube
                SET
                    Name = @Name,
                    Youtuber = @Youtuber,
                    Country = @Country
                WHERE
                    LinkId = @Id;
            ";

            var updateLinksOperation = @"
                UPDATE Links
                SET
                    Url = @Url,
                    Category = @Category,
                    Description = @Description,
                    Language = @Language,
                    ModifiedAt = @ModifiedAt
                WHERE
                    Id = @Id;
            ";

            this.connection.Execute(updateYoutubeOperation, new
            {
                item.Id,
                item.Name,
                item.Youtuber,
                item.Country,
            });

            this.connection.Execute(updateLinksOperation, new
            {
                item.Id,
                item.Url,
                Category = item.Category.ToString(),
                item.Description,
                Language = item.Language.ToString(),
                ModifiedAt = DateTime.Now,
            });
        }

        private PartialYoutube TryGetItem(string id)
        {
            var selectFromChannelQuery = @"SELECT * FROM Youtube WHERE LinkId = @Id;";
            var partialChannel = this.connection.QueryFirst<PartialYoutube>(selectFromChannelQuery, new { Id = id });

            return partialChannel;
        }
    }
}
