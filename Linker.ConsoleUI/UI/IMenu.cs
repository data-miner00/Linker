namespace Linker.ConsoleUI.UI
{
    using System.Collections.Generic;

    /// <summary>
    /// The contract on how the actual Menu will be implemented.
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// The function that renders the banner text for the application.
        /// </summary>
        public void DisplayBanner();

        /// <summary>
        /// The function that renders the title of the application and the version.
        /// </summary>
        /// <param name="title">The title of the application.</param>
        /// <param name="version">The version of the application.</param>
        public void PageHeader(string title, string version);

        /// <summary>
        /// The function that renders the menu dynamically based on data provided.
        /// </summary>
        /// <param name="items">The list of item to be rendered as menu.</param>
        public void GenerateMenu(IEnumerable<string> items);

        /// <summary>
        /// A function that renders the footer of the application.
        /// </summary>
        public void PageFooter();
    }
}
