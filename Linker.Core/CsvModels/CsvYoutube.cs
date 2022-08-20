namespace Linker.Core.CsvModels
{
    using System;

    /// <summary>
    /// The class used for mapper for Youtube.
    /// </summary>
    public class CsvYoutube
    {
        /// <summary>
        /// Gets or sets the unique Id of the Youtube Channel.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the Youtube Channel.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the url of the Youtube Channel.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the name of the Youtuber.
        /// </summary>
        public string Youtuber { get; set; }

        /// <summary>
        /// Gets or sets the category of the Youtube Channel.
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// Gets or sets the language of the Youtube Channel.
        /// </summary>
        public int Language { get; set; }

        /// <summary>
        /// Gets or sets the country of the Youtube Channel.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the description of the Youtube Channel.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tags of the Youtube Channel.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the last visit of the Youtube Channel.
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
