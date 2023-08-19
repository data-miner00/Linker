namespace Linker.Core.Models.Dtos
{
    using Linker.Core.Models;

    /// <summary>
    /// The database object model for a intermediary article state without Tags and parent attributes.
    /// </summary>
    public sealed class PartialArticle
    {
        /// <summary>
        /// Gets or sets the foreign key that refers to the Link parent.
        /// </summary>
        public string LinkId { get; set; }

        /// <summary>
        /// Gets or sets the title of the article.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author of the article.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the year published of the article.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the article is reserved for read later.
        /// </summary>
        public bool WatchLater { get; set; }

        /// <summary>
        /// Gets or sets the domain of the article.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the grammar prowess of the article.
        /// </summary>
        public Grammar Grammar { get; set; }
    }
}
