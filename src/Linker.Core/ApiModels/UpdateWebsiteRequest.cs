namespace Linker.Core.ApiModels
{
    using System.ComponentModel.DataAnnotations;
    using Linker.Core.Models;

    /// <summary>
    /// The update website request object.
    /// </summary>
    public sealed class UpdateWebsiteRequest
    {
        /// <summary>
        /// Gets or sets the name of the website to update.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the url of the website to update.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the category of the website to update.
        /// </summary>
        [Required]
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the description of the website to update.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the language of the website to update.
        /// </summary>
        [Required]
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets the aesthetics of the website to update.
        /// </summary>
        [Required]
        public Aesthetics Aesthetics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the website to update is a subdomain.
        /// </summary>
        [Required]
        public bool IsSubdomain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the website to update supports multilingual.
        /// </summary>
        [Required]
        public bool IsMultilingual { get; set; }
    }
}
