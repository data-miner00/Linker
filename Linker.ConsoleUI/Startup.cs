using System;
using EnsureThat;
using Linker.Core.Controllers;

namespace Linker.ConsoleUI
{
    internal sealed class Startup : IStartup
    {
        private readonly ILinkController linkController;

        public Startup(ILinkController linkController)
        {
            this.linkController = EnsureArg.IsNotNull(linkController, nameof(linkController));
        }

        public void Run()
        {
            string input;

            do
            {
                Console.WriteLine("Link Management Program\n");
                Console.WriteLine("1. View all links");
                Console.WriteLine("2. View by id");
                Console.WriteLine("3. Insert a link");
                Console.WriteLine("4. Update a link");
                Console.WriteLine("5. Remove a link");
                Console.WriteLine("6. Exit");


                Console.Write("> ");
                input = Console.ReadLine();

                switch(input)
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
                        break;
                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }

                Console.Clear();

            } while (input != "6");
        }
    }
}
