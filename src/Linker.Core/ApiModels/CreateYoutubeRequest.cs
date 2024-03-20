namespace Linker.Core.ApiModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Linker.Core.Models;

    /// <summary>
    /// The request object for creating a Youtube record.
    /// </summary>
    public sealed class CreateYoutubeRequest
    {
        /// <summary>
        /// Gets or sets the url of the Youtube channel.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Url { get; set; } = null!;

        /// <summary>
        /// Gets or sets the category of the Youtube channel.
        /// </summary>
        [Required]
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the description of the Youtube channel.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the language of the Youtube channel.
        /// </summary>
        [Required]
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets the rating of the article.
        /// </summary>
        [Required]
        public Rating Rating { get; set; }

        /// <summary>
        /// Gets or sets the name of the Youtube channel.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the Youtube channel creator.
        /// </summary>
        public string Youtuber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the country that the Youtube channel based in.
        /// </summary>
        public string Country { get; set; } = null!;

        /// <summary>
        /// Gets or sets the tags that is related to the Youtube channel.
        /// </summary>
        [Required]
        public IEnumerable<string> Tags { get; set; } = null!;
    }
}
