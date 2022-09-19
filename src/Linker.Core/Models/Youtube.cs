namespace Linker.Core.Models
{
    /// <summary>
    /// The model for a Youtube channel's link.
    /// </summary>
    public class Youtube : Link
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
