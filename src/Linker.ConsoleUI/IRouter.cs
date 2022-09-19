namespace Linker.ConsoleUI
{
    /// <summary>
    /// The interface for routing functionality. Defines the available routes to navigate.
    /// </summary>
    internal interface IRouter
    {
        /// <summary>
        /// The route to general website link manager.
        /// </summary>
        public void Website();

        /// <summary>
        /// The route to article link manager.
        /// </summary>
        public void Article();

        /// <summary>
        /// The route to Youtube channel link manager.
        /// </summary>
        public void Youtube();

        /// <summary>
        /// The route to uncategorized link manager.
        /// </summary>
        public void AdHoc();
    }
}
