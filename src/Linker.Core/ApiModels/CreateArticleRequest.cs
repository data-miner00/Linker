namespace Linker.Core.ApiModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Linker.Core.Models;

    /// <summary>
    /// The request object for creating an article.
    /// </summary>
    public sealed class CreateArticleRequest
    {
        /// <summary>
        /// Gets or sets the title of the article.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the url of the article.
        /// </summary>
        [Required]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the category of the article.
        /// </summary>
        [Required]
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the description of the article.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tags of the article.
        /// </summary>
        [Required]
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets author of the article.
        /// </summary>
        [Required]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets language of the article.
        /// </summary>
        [Required]
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets aesthetics of the article.
        /// </summary>
        [Required]
        public Aesthetics Aesthetics { get; set; }

        /// <summary>
        /// Gets or sets grammar of the article.
        /// </summary>
        [Required]
        public Grammar Grammar { get; set; }

        /// <summary>
        /// Gets or sets year published of the article.
        /// </summary>
        [Range(1970, 2100)]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the article is marked as watch later.
        /// </summary>
        [Required]
        public bool WatchLater { get; set; }
    }
}
