namespace Linker.Core.ApiModels
{
    using System.ComponentModel.DataAnnotations;
    using Linker.Core.Models;

    /// <summary>
    /// The request object for updating an article.
    /// </summary>
    public sealed class UpdateArticleRequest
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
        /// Gets or sets the author of the article.
        /// </summary>
        [Required]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the language of the article.
        /// </summary>
        [Required]
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets the aesthetics of the article.
        /// </summary>
        [Required]
        public Aesthetics Aesthetics { get; set; }

        /// <summary>
        /// Gets or sets the grammar of the article.
        /// </summary>
        [Required]
        public Grammar Grammar { get; set; }

        /// <summary>
        /// Gets or sets the year that the article was published.
        /// </summary>
        [Range(1970, 2100)]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the article has been put into watch later list.
        /// </summary>
        [Required]
        public bool WatchLater { get; set; }
    }
}
