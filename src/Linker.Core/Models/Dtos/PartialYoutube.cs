namespace Linker.Core.Models.Dtos
{
    /// <summary>
    /// The database transfer object model for a Youtube channel without Tags and parent attributes.
    /// </summary>
    public sealed class PartialYoutube
    {
        /// <summary>
        /// Gets or sets the foreign key that refers to the Link parent.
        /// </summary>
        public string LinkId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Youtube channel.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the owner of the Youtube channel.
        /// </summary>
        public string Youtuber { get; set; }

        /// <summary>
        /// Gets or sets the country of origin.
        /// </summary>
        public string Country { get; set; }
    }
}
