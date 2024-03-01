namespace Linker.Core.CsvModels
{
    using CsvHelper.Configuration;

    /// <summary>
    /// The class map to map CSV to <see cref="CsvArticle"/>.
    /// </summary>
    public sealed class ArticleClassMap : ClassMap<CsvArticle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleClassMap"/> class.
        /// </summary>
        public ArticleClassMap()
        {
            this.Map(m => m.Id).Name("Id");
            this.Map(m => m.Title).Name("Title");
            this.Map(m => m.Author).Name("Author");
            this.Map(m => m.Year).Name("Year");
            this.Map(m => m.Url).Name("Url");
            this.Map(m => m.Category).Name("Category");
            this.Map(m => m.WatchLater).Name("WatchLater");
            this.Map(m => m.Domain).Name("Domain");
            this.Map(m => m.Description).Name("Description");
            this.Map(m => m.Tags).Name("Tags");
            this.Map(m => m.Language).Name("Language");
            this.Map(m => m.Grammar).Name("Grammar");
            this.Map(m => m.LastVisitAt).Name("LastVisitAt");
            this.Map(m => m.CreatedAt).Name("CreatedAt");
            this.Map(m => m.ModifiedAt).Name("ModifiedAt");
        }
    }
}
