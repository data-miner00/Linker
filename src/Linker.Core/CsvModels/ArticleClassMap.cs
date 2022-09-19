namespace Linker.Core.CsvModels
{
    using CsvHelper.Configuration;

    public sealed class ArticleClassMap : ClassMap<CsvArticle>
    {
        public ArticleClassMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.Title).Name("Title");
            Map(m => m.Author).Name("Author");
            Map(m => m.Year).Name("Year");
            Map(m => m.Url).Name("Url");
            Map(m => m.Category).Name("Category");
            Map(m => m.WatchLater).Name("WatchLater");
            Map(m => m.Domain).Name("Domain");
            Map(m => m.Description).Name("Description");
            Map(m => m.Tags).Name("Tags");
            Map(m => m.Language).Name("Language");
            Map(m => m.Grammar).Name("Grammar");
            Map(m => m.LastVisitAt).Name("LastVisitAt");
            Map(m => m.CreatedAt).Name("CreatedAt");
            Map(m => m.ModifiedAt).Name("ModifiedAt");
        }
    }
}
