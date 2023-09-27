namespace Linker.Core.ApiModels
{
    /// <summary>
    /// The update tag request object.
    /// </summary>
    public sealed class UpdateTagRequest
    {
        /// <summary>
        /// Gets or sets the new name of the targeted tag to change to.
        /// </summary>
        public string NewName { get; set; }
    }
}
