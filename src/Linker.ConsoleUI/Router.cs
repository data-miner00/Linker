namespace Linker.ConsoleUI
{
    using System;
    using EnsureThat;
    using Linker.ConsoleUI.UI;
    using Linker.Core.Controllers.ConsoleUI;
    using Linker.Core.Models;

    /// <summary>
    /// The actual router that handles the routing between link managers.
    /// </summary>
    internal sealed class Router : IRouter
    {
        private readonly IWebsiteController linkController;
        private readonly IArticleController articleController;
        private readonly IYoutubeController youtubeController;
        private readonly IMenu menu;

        /// <summary>
        /// Initializes a new instance of the <see cref="Router"/> class.
        /// </summary>
        /// <param name="linkController">The <see cref="IWebsiteController"/> object.</param>
        /// <param name="articleController">The <see cref="IArticleController"/> object.</param>
        /// <param name="youtubeController">The <see cref="IYoutubeController"/> object.</param>
        /// <param name="menu">The <see cref="IMenu"/> object.</param>
        public Router(
            IWebsiteController linkController,
            IArticleController articleController,
            IYoutubeController youtubeController,
            IMenu menu)
        {
            this.linkController = EnsureArg.IsNotNull(linkController, nameof(linkController));
            this.articleController = EnsureArg.IsNotNull(articleController, nameof(articleController));
            this.youtubeController = EnsureArg.IsNotNull(youtubeController, nameof(youtubeController));
            this.menu = EnsureArg.IsNotNull(menu, nameof(menu));
        }

        /// <inheritdoc/>
        public void Website()
        {
            this.Display(this.linkController, "website");
        }

        /// <inheritdoc/>
        public void Article()
        {
            this.Display(this.articleController, "article");
        }

        /// <inheritdoc/>
        public void Youtube()
        {
            this.Display(this.youtubeController, "youtube");
        }

        private void Display<T>(IController<T> controller, string name)
            where T : Link
        {
            while (true)
            {
                Console.Clear();

                this.menu.DisplayBanner();
                this.menu.PageHeader("Link Management System", name);
                this.menu.GenerateMenu(new[]
                {
                    $"View all {name}s",
                    $"View {name} by ID",
                    $"Insert a {name}",
                    $"Update a {name}",
                    $"Remove a {name}",
                    "Return",
                    "Exit",
                });

                Console.Write("Your selection: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        controller.DisplayAllItems();
                        break;
                    case "2":
                        controller.DisplayItemDetails();
                        break;
                    case "3":
                        controller.InsertItem();
                        break;
                    case "4":
                        controller.UpdateItem();
                        break;
                    case "5":
                        controller.RemoveItem();
                        break;
                    case "6":
                        controller.SaveChanges();
                        return;
                    case "7":
                        controller.SaveChanges();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("The input is invalid, please provide the correct input.");
                        break;
                }
            }
        }

        /// <inheritdoc/>
        public void AdHoc()
        {
            // wip
        }
    }
}
