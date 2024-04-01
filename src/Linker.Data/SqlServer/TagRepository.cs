namespace Linker.Data.SqlServer;

using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using EnsureThat;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;

/// <summary>
/// The repository responsible for handling operations regarding tags.
/// </summary>
public class TagRepository : ITagRepository
{
    private static readonly string[] Columns = ["Id", "Name"];
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
    public Task<IEnumerable<Tag>> GetAllAsync()
    {
        var query = @"SELECT * FROM Tags;";
        return this.connection.QueryAsync<Tag>(query);
    }

    /// <inheritdoc/>
    public Task<Tag> GetByAsync(string type, string value)
    {
        if (!Columns.Contains(type))
        {
            throw new ArgumentException(type, nameof(type));
        }

        var query = $"SELECT * FROM Tags WHERE {type} = @value;";
        return this.connection.QueryFirstAsync<Tag>(query, new { value });
    }

    /// <inheritdoc/>
    public Task AddAsync(string name)
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

        return this.connection.ExecuteAsync(query, new
        {
            Id = randomId,
            Name = name,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        });
    }

    /// <inheritdoc/>
    public Task AddLinkTagAsync(string linkId, string tagId)
    {
        var operation = @"
            INSERT INTO LinksTags (
                LinkId,
                TagId
            ) VALUES (
                @LinkId,
                @TagId
            );
        ";

        return this.connection.ExecuteAsync(operation, new { LinkId = linkId, TagId = tagId });
    }

    /// <inheritdoc/>
    public Task EditNameAsync(string id, string newName)
    {
        var operation = @"
            UPDATE Tags
            SET
                Name = @Name,
                ModifiedAt = @ModifiedAt
            WHERE Id = @Id;
        ";

        return this.connection.ExecuteAsync(operation, new
        {
            Id = id,
            Name = newName,
            ModifiedAt = DateTime.Now,
        });
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string id)
    {
        var deleteFromLinkTagsOperation = @"DELETE FROM LinksTags WHERE TagId = @Id;";
        var deleteFromTagsOperation = @"DELETE FROM Tags WHERE Id = @Id;";

        await this.connection
            .ExecuteAsync(deleteFromLinkTagsOperation, new { Id = id })
            .ConfigureAwait(false);
        await this.connection
            .ExecuteAsync(deleteFromTagsOperation, new { Id = id })
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task DeleteLinkTagAsync(string linkId, string tagId)
    {
        var operation = @"
            DELETE FROM LinksTags
            WHERE
                LinkId = @LinkId AND
                TagId = @TagId;
        ";

        return this.connection.ExecuteAsync(operation, new { LinkId = linkId, TagId = tagId });
    }
}
