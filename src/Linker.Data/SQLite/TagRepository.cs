namespace Linker.Data.SQLite
{
    using System;
    using System.Data;
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
        public IEnumerable<Tag> GetAll()
        {
            var query = @"SELECT * FROM Tags;";
            var tags = this.connection.Query<Tag>(query);

            return tags;
        }

        /// <inheritdoc/>
        public void Add(string name)
        {
            var randomId = Guid.NewGuid().ToString();

            var query = @"
                INSERT INTO Tags (
                    Id,
                    Name
                ) VALUES (
                    @Id,
                    @Name
                );
            ";

            this.connection.Execute(query, new { Id = randomId, Name = name });
        }

        /// <inheritdoc/>
        public void AddLinkTag(string linkId, string tagId)
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

            this.connection.Execute(operation, new { LinkId = linkId, TagId = tagId });
        }

        /// <inheritdoc/>
        public void EditName(string id, string newName)
        {
            var operation = @"UPDATE Tags SET Name = @Name WHERE Id = @Id;";

            this.connection.Execute(operation, new { Id = id, Name = newName });
        }

        /// <inheritdoc/>
        public void Delete(string id)
        {
            var deleteFromLinkTagsOperation = @"DELETE FROM Link_Tags WHERE LinkId = @Id;";
            var deleteFromTagsOperation = @"DELETE FROM Tags WHERE Id = @Id;";

            this.connection.Execute(deleteFromLinkTagsOperation, new { Id = id });
            this.connection.Execute(deleteFromTagsOperation, new { Id = id });
        }

        /// <inheritdoc/>
        public void DeleteLinkTag(string linkId, string tagId)
        {
            var operation = @"
                DELETE FROM Links_Tags
                WHERE
                    LinkId = @LinkId,
                    TagId = @TagId;
            ";

            this.connection.Execute(operation, new { LinkId = linkId, TagId = tagId });
        }
    }
}
