namespace Linker.Core.Repositories
{
    using System.Collections.Generic;
    using Linker.Core.Models;

    /// <summary>
    /// The abstraction for the tag repository.
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        /// Get all existing tags.
        /// </summary>
        /// <returns>The list of tags.</returns>
        public IEnumerable<Tag> GetAll();

        /// <summary>
        /// Adds a new tag.
        /// </summary>
        /// <param name="name">The name of the tag.</param>
        public void Add(string name);

        /// <summary>
        /// Add link tag pairs. Used when creating a new <see cref="Link"/>.
        /// </summary>
        /// <param name="linkId">The link Id.</param>
        /// <param name="tagId">The tag Id.</param>
        public void AddLinkTag(string linkId, string tagId);

        /// <summary>
        /// Edit the name of a tag.
        /// </summary>
        /// <param name="id">The Id of the tag.</param>
        /// <param name="newName">The new name of the tag.</param>
        public void EditName(string id, string newName);

        /// <summary>
        /// Delete the tag from Tags and Link_Tags table.
        /// </summary>
        /// <param name="id">The Id of the tag to be deleted.</param>
        public void Delete(string id);

        /// <summary>
        /// Delete link tag. Used when deleting a <see cref="Link"/>.
        /// </summary>
        /// <param name="linkId">The link Id.</param>
        /// <param name="tagId">The tag Id.</param>
        public void DeleteLinkTag(string linkId, string tagId);
    }
}
