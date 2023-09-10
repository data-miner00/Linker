namespace Linker.Core.Controllers
{
    using System;
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
        IActionResult GetById(Guid id);

        /// <summary>
        /// Gets all link for the variant.
        /// </summary>
        /// <returns>Http response with data.</returns>
        IActionResult GetAll();

        /// <summary>
        /// Creates a new link item.
        /// </summary>
        /// <param name="request">The create request.</param>
        /// <returns>Http response.</returns>
        IActionResult Create(TCreateRequest request);

        /// <summary>
        /// Updates an existing link.
        /// </summary>
        /// <param name="id">The Id of the link.</param>
        /// <param name="request">The request.</param>
        /// <returns>Http response.</returns>
        IActionResult Update(Guid id, TUpdateRequest request);

        /// <summary>
        /// Deletes an existing link.
        /// </summary>
        /// <param name="id">The Id of the link.</param>
        /// <returns>Http response.</returns>
        IActionResult Delete(Guid id);
    }
}
