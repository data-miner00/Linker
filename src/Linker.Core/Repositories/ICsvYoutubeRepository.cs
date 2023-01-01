namespace Linker.Core.Repositories
{
    using Linker.Core.CsvModels;
    using Linker.Core.Models;

    /// <summary>
    /// The interface for CsvYoutubeRepository.
    /// </summary>
    public interface ICsvYoutubeRepository : IInMemoryCsvRepository<Youtube, CsvYoutube>
    {
    }
}
