namespace Linker.Core.V2.Models
{
    /// <summary>
    /// The enum for all possible values for Aesthetics.
    /// </summary>
    public enum Aesthetics
    {
        /// <summary>
        /// The default value when the field is unset.
        /// </summary>
        None,

        /// <summary>
        /// Very poorly designed. Horrible UX and eyesore to watch.
        /// </summary>
        Poor,

        /// <summary>
        /// Not diabolical nor impressive. UX is acceptable.
        /// </summary>
        Normal,

        /// <summary>
        /// Clean and minimalistic. UX is quite smooth.
        /// </summary>
        Clean,

        /// <summary>
        /// Lovely UI and UX, most modern website should be in this category.
        /// </summary>
        Decent,

        /// <summary>
        /// Astounding design. Have a WOW effect when first exposed.
        /// </summary>
        Impressive,
    }
}
