namespace Linker.ConsoleUI
{
    using System;
    using EnsureThat;
    using Linker.ConsoleUI.UI;
    using Linker.Core.Controllers;

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
            while (true)
            {
                Console.Clear();

                this.menu.DisplayBanner();
                this.menu.PageHeader("Link Management System", "Website");
                this.menu.GenerateMenu(new[]
                {
                    "View all websites",
                    "View website by ID",
                    "Insert a website",
                    "Update a website",
                    "Remove a website",
                    "Return",
                    "Exit",
                });

                Console.Write("Your selection: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        this.linkController.DisplayAllItems();
                        break;
                    case "2":
                        this.linkController.DisplayItemDetails();
                        break;
                    case "3":
                        this.linkController.InsertItem();
                        break;
                    case "4":
                        this.linkController.UpdateItem();
                        break;
                    case "5":
                        this.linkController.RemoveItem();
                        break;
                    case "6":
                        this.linkController.SaveChanges();
                        return;
                    case "7":
                        this.linkController.SaveChanges();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("The input is invalid, please provide the correct input.");
                        break;
                }
            }
        }

        /// <inheritdoc/>
        public void Article()
        {
            while (true)
            {
                Console.Clear();

                this.menu.DisplayBanner();
                this.menu.PageHeader("Link Management System", "Article");
                this.menu.GenerateMenu(new[]
                {
                    "View all articles",
                    "View article by ID",
                    "Insert an article",
                    "Update an article",
                    "Remove an article",
                    "Return",
                    "Exit",
                });

                Console.Write("Your selection: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        this.articleController.DisplayAllItems();
                        break;
                    case "2":
                        this.articleController.DisplayItemDetails();
                        break;
                    case "3":
                        this.articleController.InsertItem();
                        break;
                    case "4":
                        this.articleController.UpdateItem();
                        break;
                    case "5":
                        this.articleController.RemoveItem();
                        break;
                    case "6":
                        this.articleController.SaveChanges();
                        return;
                    case "7":
                        this.articleController.SaveChanges();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("The input is invalid, please provide the correct input.");
                        break;
                }
            }
        }

        /// <inheritdoc/>
        public void Youtube()
        {
            while (true)
            {
                Console.Clear();

                this.menu.DisplayBanner();
                this.menu.PageHeader("Link Management System", "Youtube");
                this.menu.GenerateMenu(new[]
                {
                    "View all channels",
                    "View channel by ID",
                    "Insert a channel",
                    "Update a channel",
                    "Remove a channel",
                    "Return",
                    "Exit",
                });

                Console.Write("Your selection: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        this.youtubeController.DisplayAllItems();
                        break;
                    case "2":
                        this.youtubeController.DisplayItemDetails();
                        break;
                    case "3":
                        this.youtubeController.InsertItem();
                        break;
                    case "4":
                        this.youtubeController.UpdateItem();
                        break;
                    case "5":
                        this.youtubeController.RemoveItem();
                        break;
                    case "6":
                        this.youtubeController.SaveChanges();
                        return;
                    case "7":
                        this.youtubeController.SaveChanges();
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
