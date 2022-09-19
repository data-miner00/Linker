namespace Linker.Core.CsvModels
{
    using System;

    /// <summary>
    /// The base class for Csv Entity.
    /// </summary>
    public abstract class CsvLink
    {
        /// <summary>
        /// Gets or sets the unique identifier for the link.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the url of the link.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the category of the link.
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// Gets or sets the description/summary of the link.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tags of the link.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the language of the link.
        /// </summary>
        public int Language { get; set; }

        /// <summary>
        /// Gets or sets the last visit of the link.
        /// </summary>
        public DateTime LastVisitAt { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the link entry.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the modified date of the link entry.
        /// </summary>
        public DateTime ModifiedAt { get; set; }
    }
}
