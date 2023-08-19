namespace Linker.Data.SQLite
{
    using System;
    using System.Data;
    using Dapper;
    using EnsureThat;
    using Linker.Core.Models;

    /// <summary>
    /// The repository responsible for handling operations regarding tags.
    /// </summary>
    public class TagRepository
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

        /// <summary>
        /// Adds a new tag.
        /// </summary>
        /// <param name="name">The name of the tag.</param>
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

        /// <summary>
        /// Add link tag pairs. Used when creating a new <see cref="Link"/>.
        /// </summary>
        /// <param name="linkId">The link Id.</param>
        /// <param name="tagId">The tag Id.</param>
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

        /// <summary>
        /// Edit the name of a tag.
        /// </summary>
        /// <param name="id">The Id of the tag.</param>
        /// <param name="newName">The new name of the tag.</param>
        public void EditName(string id, string newName)
        {
            var operation = @"UPDATE Tags SET Name = @Name WHERE Id = @Id;";

            this.connection.Execute(operation, new { Id = id, Name = newName });
        }

        /// <summary>
        /// Delete the tag from Tags and Link_Tags table.
        /// </summary>
        /// <param name="id">The Id of the tag to be deleted.</param>
        public void Delete(string id)
        {
            var deleteFromLinkTagsOperation = @"DELETE FROM Link_Tags WHERE LinkId = @Id;";
            var deleteFromTagsOperation = @"DELETE FROM Tags WHERE Id = @Id;";

            this.connection.Execute(deleteFromLinkTagsOperation, new { Id = id });
            this.connection.Execute(deleteFromTagsOperation, new { Id = id });
        }

        /// <summary>
        /// Delete link tag. Used when deleting a <see cref="Link"/>.
        /// </summary>
        /// <param name="linkId">The link Id.</param>
        /// <param name="tagId">The tag Id.</param>
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
