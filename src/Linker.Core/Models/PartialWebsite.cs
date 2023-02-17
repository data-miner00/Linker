namespace Linker.Core.Models
{
    /// <summary>
    /// The database object model for a intermediary website state without Tags and parent attributes.
    /// </summary>
    public class PartialWebsite
    {
        /// <summary>
        /// Gets or sets the foreign key that refers to the Link parent.
        /// </summary>
        public string LinkId { get; set; }

        /// <summary>
        /// Gets or sets the name of the website/entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the domain of the website.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the aesthetic value of the website.
        /// </summary>
        public Aesthetics Aesthetics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the website is a subdomain.
        /// </summary>
        public bool IsSubdomain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the website is multilingual.
        /// </summary>
        public bool IsMultilingual { get; set; }
    }
}
