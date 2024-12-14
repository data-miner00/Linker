namespace Linker.Core.V2.Repositories;

using Linker.Core.V2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// The abstraction for the <see cref="Playlist"/> repository.
/// </summary>
public interface IPlaylistRepository
{
    /// <summary>
    /// Gets all playlists for a certain user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of playlists.</returns>
    Task<IEnumerable<Playlist>> GetAllByUserAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the playlist by Id.
    /// </summary>
    /// <param name="id">The playlist Id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The found playlist.</returns>
    Task<Playlist> GetByIdAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new playlist.
    /// </summary>
    /// <param name="playlist">The playlist to be created.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task AddAsync(Playlist playlist, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing playlist.
    /// </summary>
    /// <param name="playlist">The playlist to be updated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a playlist by Id.
    /// </summary>
    /// <param name="id">The id of the playlist to be removed.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task RemoveAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the links of a playlist.
    /// </summary>
    /// <param name="id">The playlist id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of links.</returns>
    Task<IEnumerable<Link>> GetPlaylistLinksAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds the link to a playlist.
    /// </summary>
    /// <param name="playlistId">The playlist Id.</param>
    /// <param name="linkId">The link Id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task AddPlaylistLinkAsync(string playlistId, string linkId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes the link from a playlist.
    /// </summary>
    /// <param name="playlistId">The playlist Id.</param>
    /// <param name="linkId">The link Id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task RemovePlaylistLinkAsync(string playlistId, string linkId, CancellationToken cancellationToken);
}
