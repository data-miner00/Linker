namespace Linker.Core.V2.Models
{
    /// <summary>
    /// The object that links a link to a tag.
    /// </summary>
    public class LinkTagPair
    {
        /// <summary>
        /// Gets or sets the link Id.
        /// </summary>
        public string LinkId { get; set; }

        /// <summary>
        /// Gets or sets the tag Id.
        /// </summary>
        public string TagId { get; set; }
    }
}
