namespace Linker.Core.ApiModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Linker.Core.Models;

    /// <summary>
    /// The website creation request object.
    /// </summary>
    public sealed class CreateWebsiteRequest
    {
        /// <summary>
        /// Gets or sets the name of the website.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the url of the website.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the category of the website.
        /// </summary>
        [Required]
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the description of the website.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tags of the website.
        /// </summary>
        [Required]
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the language of the website.
        /// </summary>
        [Required]
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets the aesthetics of the website.
        /// </summary>
        [Required]
        public Aesthetics Aesthetics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the website is under a subdomain.
        /// </summary>
        [Required]
        public bool IsSubdomain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the website supports multilingual.
        /// </summary>
        [Required]
        public bool IsMultilingual { get; set; }
    }
}
