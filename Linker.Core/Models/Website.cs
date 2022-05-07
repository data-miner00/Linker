namespace Linker.Core.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The model for a general website's link.
    /// </summary>
    public class Website
    {
        /// <summary>
        /// Gets or sets the unique identifier for the object.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the website/entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the url of the website.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the category of the website.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the domain of the website.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the description of the website.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tags of the website.
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the main language used in the website.
        /// </summary>
        public Language MainLanguage { get; set; } = default;

        /// <summary>
        /// Gets or sets the aesthetic value of the website.
        /// </summary>
        public Aesthetics Aesthetics { get; set; }

        /// <summary>
        /// Gets or sets whether the website is a subdomain.
        /// </summary>
        public bool IsSubdomain { get; set; }

        /// <summary>
        /// Gets or sets whether the website is multilingual.
        /// </summary>
        public bool IsMultilingual { get; set; }

        /// <summary>
        /// Gets or sets the last visit of the website.
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
