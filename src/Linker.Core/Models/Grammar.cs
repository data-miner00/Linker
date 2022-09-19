namespace Linker.Core.Models
{
    /// <summary>
    /// The states or variants available for Grammar.
    /// </summary>
    public enum Grammar
    {
        /// <summary>
        /// Default value.
        /// </summary>
        Unknown,

        /// <summary>
        /// Poorly written. Grammar doesn't make sense or erraneous.
        /// </summary>
        Poor,

        /// <summary>
        /// Not fluent and have a few errors. Otherwise are comprehensible.
        /// </summary>
        Average,

        /// <summary>
        /// Written with fluency. Less mistakes or non-existent, with good vocab.
        /// </summary>
        Good,

        /// <summary>
        /// A masterpiece on its own. Mingled with bombastic vocabs seamlessly.
        /// Usually written by journalist.
        /// </summary>
        Impressive,
    }
}
