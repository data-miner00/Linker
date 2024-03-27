namespace Linker.Core.V2.Models
{
    using System;

    /// <summary>
    /// The model for a tag.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the tag Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTime ModifiedAt { get; set; }
    }
}
