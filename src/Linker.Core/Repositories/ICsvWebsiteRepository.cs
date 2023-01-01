namespace Linker.Core.Repositories
{
    using Linker.Core.CsvModels;
    using Linker.Core.Models;

    /// <summary>
    /// The Csv implementation of website repository.
    /// </summary>
    public interface ICsvWebsiteRepository : IInMemoryCsvRepository<Website, CsvWebsite>
    {
    }
}
