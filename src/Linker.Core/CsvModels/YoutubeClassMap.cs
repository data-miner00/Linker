namespace Linker.Core.CsvModels
{
    /// <summary>
    /// The mapper class for <see cref="CsvYoutube"/>.
    /// </summary>
    public class YoutubeClassMap : BaseClassMap<CsvYoutube>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YoutubeClassMap"/> class.
        /// </summary>
        public YoutubeClassMap()
        {
            this.Map(m => m.Name).Name("Name");
            this.Map(m => m.Youtuber).Name("Youtuber");
            this.Map(m => m.Country).Name("Country");
        }
    }
}
