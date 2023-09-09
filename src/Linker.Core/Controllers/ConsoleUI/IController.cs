namespace Linker.Core.Controllers.ConsoleUI
{
    using Linker.Core.Models;

    /// <summary>
    /// The base interface for link controllers.
    /// </summary>
    /// <typeparam name="T">The object that extends <see cref="Link"/>.</typeparam>
    public interface IController<T>
        where T : Link
    {
        /// <summary>
        /// Navigates to display all items page.
        /// </summary>
        public void DisplayAllItems();

        /// <summary>
        /// Navigates to display item details page.
        /// </summary>
        public void DisplayItemDetails();

        /// <summary>
        /// Navigates to add new item page.
        /// </summary>
        public void InsertItem();

        /// <summary>
        /// Navigates to update item page.
        /// </summary>
        public void UpdateItem();

        /// <summary>
        /// Navigates to remove item page.
        /// </summary>
        public void RemoveItem();

        /// <summary>
        /// Save the changes made.
        /// </summary>
        public void SaveChanges();
    }
}
