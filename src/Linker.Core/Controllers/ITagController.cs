namespace Linker.Core.Controllers
{
    using System;
    using Linker.Core.ApiModels;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The interface for the tag controller.
    /// </summary>
    public interface ITagController
    {
        /// <summary>
        /// Retrieves all existing tags in the database.
        /// </summary>
        /// <returns>The Http response with existing tags.</returns>
        public IActionResult GetAll();

        /// <summary>
        /// Gets a tag either by Id or by name.
        /// </summary>
        /// <param name="id">The Id of the tag.</param>
        /// <param name="name">The name of the tag.</param>
        /// <returns>The Http response with found tag.</returns>
        public IActionResult GetBy(string? id, string? name);

        /// <summary>
        /// Adds a new tag.
        /// </summary>
        /// <param name="request">The create tag request.</param>
        /// <returns>Http response with no content.</returns>
        public IActionResult Create(CreateTagRequest request);

        /// <summary>
        /// Creates a connection between a link and a tag.
        /// </summary>
        /// <param name="linkId">The Id of the link.</param>
        /// <param name="tagId">The Id of the tag.</param>
        /// <returns>Http response.</returns>
        public IActionResult CreateLinkTag(Guid linkId, Guid tagId);

        /// <summary>
        /// Update the name of the tag.
        /// </summary>
        /// <param name="id">The Id of the tag to be updated.</param>
        /// <param name="request">The update request.</param>
        /// <returns>Http response.</returns>
        public IActionResult Update(Guid id, UpdateTagRequest request);

        /// <summary>
        /// Delete the tag entirely.
        /// </summary>
        /// <param name="id">The Id of the tag to be deleted.</param>
        /// <returns>Http response.</returns>
        public IActionResult Delete(Guid id);

        /// <summary>
        /// Removes a connection between a link and a tag.
        /// </summary>
        /// <param name="linkId">The link Id.</param>
        /// <param name="tagId">The tag Id.</param>
        /// <returns>Http response.</returns>
        public IActionResult DeleteLinkTag(Guid linkId, Guid tagId);
    }
}
