namespace Linker.Core.CsvModels
{
    /// <summary>
    /// The class used for mapper for Youtube.
    /// </summary>
    public class CsvYoutube : CsvLink
    {
        /// <summary>
        /// Gets or sets the name of the Youtube Channel.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the Youtuber.
        /// </summary>
        public string Youtuber { get; set; }

        /// <summary>
        /// Gets or sets the country of the Youtube Channel.
        /// </summary>
        public string Country { get; set; }
    }
}
