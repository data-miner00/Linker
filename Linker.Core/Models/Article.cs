namespace Linker.Core.Models
{
    /// <summary>
    /// The model for a website that is an article.
    /// </summary>
    public class Article : Link
    {
        /// <summary>
        /// Gets or sets the title of the article.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author of the article.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the year of the published article.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the article is listed as watch later.
        /// </summary>
        public bool WatchLater { get; set; }

        /// <summary>
        /// Gets or sets the domain of the article.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the grammatical level of the article.
        /// </summary>
        public Grammar Grammar { get; set; }
    }
}
