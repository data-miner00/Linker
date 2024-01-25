#nullable disable
namespace Linker.Core.ApiModels
{
    using System.ComponentModel.DataAnnotations;
    using Linker.Core.Models;

    /// <summary>
    /// The update Youtube channel record request object.
    /// </summary>
    public sealed class UpdateYoutubeRequest
    {
        /// <summary>
        /// Gets or sets the url of the Youtube channel to be updated.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the category of the Youtube channel to be updated.
        /// </summary>
        [Required]
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the description of the Youtube channel to be updated.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the language of the Youtube channel to be updated.
        /// </summary>
        [Required]
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets the rating of the article.
        /// </summary>
        [Required]
        public Rating Rating { get; set; }

        /// <summary>
        /// Gets or sets the name of the Youtube channel to be updated.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the creator of the Youtube channel to be updated.
        /// </summary>
        public string Youtuber { get; set; }

        /// <summary>
        /// Gets or sets the country of the Youtube channel to be updated.
        /// </summary>
        public string Country { get; set; }
    }
}
