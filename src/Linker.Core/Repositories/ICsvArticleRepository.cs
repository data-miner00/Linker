namespace Linker.Core.Repositories
{
    using Linker.Core.CsvModels;
    using Linker.Core.Models;

    /// <summary>
    /// The interface for CsvArticleRepository.
    /// </summary>
    public interface ICsvArticleRepository : IInMemoryCsvRepository<Article, CsvArticle>
    {
    }
}
