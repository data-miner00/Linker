namespace Linker.ConsoleUI.Controllers
{
    using System;
    using System.Linq;
    using EnsureThat;
    using Linker.ConsoleUI.Extensions;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    public abstract class BaseController<T> : IController<T>
        where T : Link
    {
        protected readonly IRepository<T> repository;

        protected BaseController(IRepository<T> repository)
        {
            this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
        }

        public abstract void DisplayAllItems();

        public abstract void DisplayItemDetails();

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

        public virtual void SaveChanges()
        {
            this.repository.Commit();
        }

        protected void DisplayEnum<T>(string name)
        {
            Console.WriteLine($"List of available {name}");
            var items = Enum.GetValues(typeof(T)).Cast<T>();

            foreach (var (item, index) in items.WithIndex())
            {
                Console.WriteLine("{0} {1}", (index + ".").PadRight(3), item);
            }
        }

        protected abstract T GetItemFromInput();

        protected string PromptForInput(params object[] prompt)
        {
            var consoleWrite = typeof(Console)
                .GetMethods()
                .Where(x => x.Name == "Write" && x.IsStatic)
                .FirstOrDefault();

            consoleWrite.Invoke(null, prompt);

            var input = Console.ReadLine();
            return input;
        }
    }
}
