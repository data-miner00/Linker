namespace Linker.Data.SQLite
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using EnsureThat;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// The repository responsible for handling operations regarding tags.
    /// </summary>
    public class TagRepository : ITagRepository
    {
        private readonly IDbConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection for the database.</param>
        public TagRepository(IDbConnection connection)
        {
            this.connection = EnsureArg.IsNotNull(connection, nameof(connection));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            var query = @"SELECT * FROM Tags;";
            var tags = await this.connection
                .QueryAsync<Tag>(query)
                .ConfigureAwait(false);

            return tags;
        }

        /// <inheritdoc/>
        public async Task<Tag> GetByAsync(string type, string value)
        {
            if (!new[] { "Id", "Name" }.Contains(type))
            {
                throw new ArgumentException(type, nameof(type));
            }

            var query = $"SELECT * FROM Tags WHERE {type} = @value;";
            var tag = await this.connection
                .QueryFirstAsync<Tag>(query, new { value })
                .ConfigureAwait(false);

            return tag;
        }

        /// <inheritdoc/>
        public async Task AddAsync(string name)
        {
            var randomId = Guid.NewGuid().ToString();

            var query = @"
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

            await this.connection.ExecuteAsync(query, new
            {
                Id = randomId,
                Name = name,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            }).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task AddLinkTagAsync(string linkId, string tagId)
        {
            var operation = @"
                INSERT INTO Links_Tags (
                    LinkId,
                    TagId
                ) VALUES (
                    @LinkId,
                    @TagId
                );
            ";

            await this.connection
                .ExecuteAsync(operation, new { LinkId = linkId, TagId = tagId })
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task EditNameAsync(string id, string newName)
        {
            var operation = @"
                UPDATE Tags
                SET
                    Name = @Name,
                    ModifiedAt = @ModifiedAt
                WHERE Id = @Id;
            ";

            await this.connection.ExecuteAsync(operation, new
            {
                Id = id,
                Name = newName,
                ModifiedAt = DateTime.Now,
            }).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(string id)
        {
            var deleteFromLinkTagsOperation = @"DELETE FROM Links_Tags WHERE TagId = @Id;";
            var deleteFromTagsOperation = @"DELETE FROM Tags WHERE Id = @Id;";

            await this.connection
                .ExecuteAsync(deleteFromLinkTagsOperation, new { Id = id })
                .ConfigureAwait(false);
            await this.connection
                .ExecuteAsync(deleteFromTagsOperation, new { Id = id })
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task DeleteLinkTagAsync(string linkId, string tagId)
        {
            var operation = @"
                DELETE FROM Links_Tags
                WHERE
                    LinkId = @LinkId AND
                    TagId = @TagId;
            ";

            await this.connection
                .ExecuteAsync(operation, new { LinkId = linkId, TagId = tagId })
                .ConfigureAwait(false);
        }
    }
}
