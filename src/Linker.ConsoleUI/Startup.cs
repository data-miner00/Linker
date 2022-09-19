namespace Linker.ConsoleUI
{
    using System;
    using EnsureThat;
    using Linker.ConsoleUI.UI;

    /// <summary>
    /// The actual class that provides startup code for the appplication.
    /// </summary>
    internal sealed class Startup : IStartup
    {
        private readonly IMenu menu;
        private readonly IRouter router;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="menu">The <see cref="IMenu"/> instance.</param>
        /// <param name="router">The <see cref="IRouter"/> instance.</param>
        public Startup(IMenu menu, IRouter router)
        {
            this.menu = EnsureArg.IsNotNull(menu);
            this.router = EnsureArg.IsNotNull(router);
        }

        /// <inheritdoc/>
        public void Run()
        {
            string input;

            do
            {
                this.menu.DisplayBanner();
                this.menu.PageHeader("Link Management System", "2.0.0");
                this.menu.GenerateMenu(new[]
                {
                    "Website",
                    "Articles",
                    "Youtube",
                    "Ad Hoc",
                    "Exit",
                });

                Console.Write("Your selection: ");
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        this.router.Website();
                        break;
                    case "2":
                        this.router.Article();
                        break;
                    case "3":
                        this.router.Youtube();
                        break;
                    case "4":
                        this.router.AdHoc();
                        break;
                    case "5":
                        Console.WriteLine("Program has been terminated");
                        return;
                    default:
                        Console.WriteLine("The input is invalid, please provide the correct input.");
                        break;
                }

                Console.Clear();
            }
            while (true);
        }
    }
}
