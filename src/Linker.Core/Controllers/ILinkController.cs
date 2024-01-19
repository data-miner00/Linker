namespace Linker.Core.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The controller abstraction for all link variants.
    /// </summary>
    /// <typeparam name="TCreateRequest">The type of create request.</typeparam>
    /// <typeparam name="TUpdateRequest">The type of update request.</typeparam>
    public interface ILinkController<in TCreateRequest, in TUpdateRequest>
    {
        /// <summary>
        /// Gets a link by Id.
        /// </summary>
        /// <param name="id">The Id of the link.</param>
        /// <returns>Http response with data.</returns>
        Task<IActionResult> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all link for the variant.
        /// </summary>
        /// <returns>Http response with data.</returns>
        Task<IActionResult> GetAllAsync();

        /// <summary>
        /// Creates a new link item.
        /// </summary>
        /// <param name="request">The create request.</param>
        /// <returns>Http response.</returns>
        Task<IActionResult> CreateAsync(TCreateRequest request);

        /// <summary>
        /// Updates an existing link.
        /// </summary>
        /// <param name="id">The Id of the link.</param>
        /// <param name="request">The request.</param>
        /// <returns>Http response.</returns>
        Task<IActionResult> UpdateAsync(Guid id, TUpdateRequest request);

        /// <summary>
        /// Deletes an existing link.
        /// </summary>
        /// <param name="id">The Id of the link.</param>
        /// <returns>Http response.</returns>
        Task<IActionResult> DeleteAsync(Guid id);

        /// <summary>
        /// Get all links by a user.
        /// </summary>
        /// <param name="userId">The user links to retrieve.</param>
        /// <returns>A list of links.</returns>
        Task<IActionResult> GetAllByUserAsync(Guid userId);

        /// <summary>
        /// Gets a single link by a user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <param name="linkId">The link Id.</param>
        /// <returns>The found link.</returns>
        Task<IActionResult> GetByUserAsync(Guid userId, Guid linkId);

        /// <summary>
        /// Updates a link owned by a user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <param name="linkId">The link Id.</param>
        /// <param name="request">The update request.</param>
        /// <returns>Http response.</returns>
        Task<IActionResult> UpdateByUserAsync(Guid userId, Guid linkId, TUpdateRequest request);

        /// <summary>
        /// Deletes a link owned by a user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <param name="linkId">The link Id.</param>
        /// <returns>Http response.</returns>
        Task<IActionResult> DeleteByUserAsync(Guid userId, Guid linkId);
    }
}
