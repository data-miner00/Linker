namespace Linker.ConsoleUI
{
    using System;
    using Linker.ConsoleUI.UI;
    using Linker.Core.Controllers;

    public sealed class Router : IRouter
    {
        private readonly ILinkController linkController;
        private readonly IMenu menu;

        public Router(ILinkController linkController, IMenu menu)
        {
            this.linkController = linkController;
            this.menu = menu;
        }

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

        public void Article()
        {

        }

        public void Youtube()
        {

        }

        public void AdHoc()
        {

        }
    }
}
