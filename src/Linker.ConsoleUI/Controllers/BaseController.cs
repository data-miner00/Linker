namespace Linker.ConsoleUI.Controllers
{
    using System;
    using System.Linq;
    using EnsureThat;
    using Linker.Common.Extensions;
    using Linker.Core.Controllers.ConsoleUI;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// The base class for <see cref="IController{T}"/> object.
    /// Contains a few common functions.
    /// </summary>
    /// <typeparam name="TModel">The <see cref="Link"/> children.</typeparam>
    /// <typeparam name="TCsvModel">The Csv counterpart of <see cref="Link"/> children.</typeparam>
    internal abstract class BaseController<TModel, TCsvModel> : IController<TModel>
        where TModel : Link
    {
        /// <summary>
        /// The repository that is connected to the access data layer of the <typeparamref name="TModel"/> type.
        /// </summary>
        protected readonly IInMemoryCsvRepository<TModel, TCsvModel> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{TModel, TCsvModel}"/> class.
        /// </summary>
        /// <param name="repository">The <see cref="IInMemoryCsvRepository{TModel, TCsvModel}"/> object.</param>
        protected BaseController(IInMemoryCsvRepository<TModel, TCsvModel> repository)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
        }

        /// <summary>
        /// A page to displays all <typeparamref name="TModel"/> items in a list.
        /// </summary>
        public abstract void DisplayAllItems();

        /// <summary>
        /// A page to displays details of an item of type <typeparamref name="TModel"/>.
        /// </summary>
        public abstract void DisplayItemDetails();

        /// <summary>
        /// A page to add a new item of type <typeparamref name="TModel"/> into the database.
        /// </summary>
        public virtual void InsertItem()
        {
            Console.Clear();

            var newItem = this.GetItemFromInput();
            var now = DateTime.Now;
            newItem.CreatedAt = now;
            newItem.ModifiedAt = now;

            this.repository.Add(newItem);

            Console.WriteLine("Successfully added new link!");
            _ = this.PromptForInput("\nPress ENTER to return to main menu...", string.Empty);
        }

        /// <summary>
        /// A page to delete an item of type <typeparamref name="TModel"/> from the database.
        /// </summary>
        public virtual void RemoveItem()
        {
            Console.Clear();

            this.DisplayAllItems();

            var id = this.PromptForInput("Select an ID to remove: ", string.Empty);

            try
            {
                this.repository.Remove(id);
                Console.WriteLine("Successfully removed link");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to remove link! Error: {0}", ex.Message);
            }

            _ = this.PromptForInput("\nPress ENTER to return to main menu...", string.Empty);
        }

        /// <summary>
        /// A page to update a single item in the list.
        /// </summary>
        public virtual void UpdateItem()
        {
            Console.Clear();

            this.DisplayAllItems();

            var id = this.PromptForInput("Please select the ID of the link to be updated: ", string.Empty);

            Console.WriteLine("\nInsert the details that needed to be changed. Hit Enter to skip.");

            var item = this.GetItemFromInput();
            item.Id = id;

            try
            {
                this.repository.Update(item);
                Console.WriteLine("Successfully updated link!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed update link! Error: {0}", ex.Message);
            }

            _ = this.PromptForInput("\nPress ENTER to return to main menu...", string.Empty);
        }

        /// <summary>
        /// Save all the changes made.
        /// </summary>
        public virtual void SaveChanges()
        {
            this.repository.Commit();
        }

        /// <summary>
        /// A helper method to display all possible values of an enum.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="Enum"/> type.</typeparam>
        /// <param name="name">The name of the Enum.</param>
        protected void DisplayEnum<TEnum>(string name)
        {
            Console.WriteLine($"List of available {name}");
            var items = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            foreach (var (item, index) in items.WithIndex())
            {
                Console.WriteLine("{0} {1}", (index + ".").PadRight(3), item);
            }
        }

        /// <summary>
        /// Retrieves the input from the user customly.
        /// </summary>
        /// <returns>The built <see cref="Link"/> instance.</returns>
        protected abstract TModel GetItemFromInput();

        /// <summary>
        /// A helper method to display prompt and receive input as string.
        /// </summary>
        /// <param name="prompt">The text to be displayed.</param>
        /// <returns>The input obtained.</returns>
        protected string PromptForInput(params object[] prompt)
        {
            var consoleMethods = typeof(Console).GetMethods();
            var consoleWrite = Array.Find(consoleMethods, x => x.Name == "Write" && x.IsStatic);

            consoleWrite.Invoke(null, prompt);

            var input = Console.ReadLine();
            return input;
        }
    }
}
