namespace Linker.ConsoleUI
{
    using System;
    using Linker.ConsoleUI.UI;
    using Linker.Core.Controllers;

    /// <summary>
    /// The actual router that handles the routing between link managers.
    /// </summary>
    public sealed class Router : IRouter
    {
        private readonly ILinkController linkController;
        private readonly IMenu menu;

        /// <summary>
        /// Initializes a new instance of the <see cref="Router"/> class.
        /// </summary>
        /// <param name="linkController">The <see cref="ILinkController"/> object.</param>
        /// <param name="menu">The <see cref="IMenu"/> object.</param>
        public Router(ILinkController linkController, IMenu menu)
        {
            this.linkController = linkController;
            this.menu = menu;
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
                        this.linkController.DisplayAllLinks();
                        break;
                    case "2":
                        this.linkController.DisplaySingleLink();
                        break;
                    case "3":
                        this.linkController.InsertLink();
                        break;
                    case "4":
                        this.linkController.UpdateLink();
                        break;
                    case "5":
                        this.linkController.RemoveLink();
                        break;
                    case "6":
                        this.linkController.Save();
                        return;
                    case "7":
                        this.linkController.Save();
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
            // wip
        }

        /// <inheritdoc/>
        public void Youtube()
        {
            // wip
        }

        /// <inheritdoc/>
        public void AdHoc()
        {
            // wip
        }
    }
}
