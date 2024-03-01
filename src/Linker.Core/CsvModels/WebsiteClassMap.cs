namespace Linker.Core.CsvModels
{
    using CsvHelper.Configuration;

    /// <summary>
    /// The class map to map CSV to <see cref="CsvWebsite"/>.
    /// </summary>
    public sealed class WebsiteClassMap : ClassMap<CsvWebsite>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteClassMap"/> class.
        /// </summary>
        public WebsiteClassMap()
        {
            this.Map(m => m.Id).Name("Id");
            this.Map(m => m.Name).Name("Name");
            this.Map(m => m.Url).Name("Url");
            this.Map(m => m.Category).Name("Category");
            this.Map(m => m.Aesthetics).Name("Aesthetics");
            this.Map(m => m.Domain).Name("Domain");
            this.Map(m => m.IsSubdomain).Name("IsSubdomain");
            this.Map(m => m.Description).Name("Description");
            this.Map(m => m.Tags).Name("Tags");
            this.Map(m => m.CreatedAt).Name("CreatedAt");
            this.Map(m => m.ModifiedAt).Name("ModifiedAt");
            this.Map(m => m.LastVisitAt).Name("LastVisitAt");
            this.Map(m => m.MainLanguage).Name("MainLanguage");
            this.Map(m => m.IsMultilingual).Name("IsMultilingual");
        }
    }
}
