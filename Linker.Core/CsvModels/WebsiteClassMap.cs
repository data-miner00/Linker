namespace Linker.Core.CsvModels
{
    using CsvHelper.Configuration;

    public sealed class WebsiteClassMap : ClassMap<CsvWebsite>
    {
        public WebsiteClassMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.Name).Name("Name");
            Map(m => m.Url).Name("Url");
            Map(m => m.Category).Name("Category");
            Map(m => m.Aesthetics).Name("Aesthetics");
            Map(m => m.Domain).Name("Domain");
            Map(m => m.IsSubdomain).Name("IsSubdomain");
            Map(m => m.Description).Name("Description");
            Map(m => m.Tags).Name("Tags");
            Map(m => m.CreatedAt).Name("CreatedAt");
            Map(m => m.ModifiedAt).Name("ModifiedAt");
            Map(m => m.LastVisitAt).Name("LastVisitAt");
            Map(m => m.MainLanguage).Name("MainLanguage");
            Map(m => m.IsMultilingual).Name("IsMultilingual");
        }
    }
}
