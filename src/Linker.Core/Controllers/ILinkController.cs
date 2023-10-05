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
    }
}
