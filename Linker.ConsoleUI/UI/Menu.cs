namespace Linker.ConsoleUI.UI
{
    using System;
    using System.Collections.Generic;
    using Linker.ConsoleUI.Extensions;

    public sealed class Menu : IMenu
    {
        public int Width { get; set; } = 61;

        public void DisplayBanner()
        {
            Console.WriteLine();
            Console.WriteLine("   █████        ███             █████                        ");
            Console.WriteLine("  ░░███        ░░░             ░░███                         ");
            Console.WriteLine("   ░███        ████  ████████   ░███ █████  ██████  ████████ ");
            Console.WriteLine("   ░███       ░░███ ░░███░░███  ░███░░███  ███░░███░░███░░███");
            Console.WriteLine("   ░███        ░███  ░███ ░███  ░██████░  ░███████  ░███ ░░░ ");
            Console.WriteLine("   ░███      █ ░███  ░███ ░███  ░███░░███ ░███░░░   ░███     ");
            Console.WriteLine("   ███████████ █████ ████ █████ ████ █████░░██████  █████    ");
            Console.WriteLine("  ░░░░░░░░░░░ ░░░░░ ░░░░ ░░░░░ ░░░░ ░░░░░  ░░░░░░  ░░░░░     ");
            Console.WriteLine();
        }

        public void PageHeader(string title, string version)
        {
            Console.WriteLine(string.Empty.PadLeft(this.Width, '='));
            Console.WriteLine("|{0}|", string.Empty.PadLeft(this.Width - 2));
            Console.WriteLine("|{0}|", title.PadSides(this.Width - 2));
            Console.WriteLine("|{0}|", version.PadSides(this.Width - 2));
            Console.WriteLine("|{0}|", string.Empty.PadLeft(this.Width - 2));
            Console.WriteLine(string.Empty.PadLeft(this.Width, '='));
        }

        public void GenerateMenu(IEnumerable<string> items)
        {
            foreach (var (item, index) in items.WithIndex())
            {
                Console.WriteLine(string.Empty.PadLeft(this.Width, '='));
                Console.WriteLine("|{0}|", $"{index + 1} - {item}".PadSides(this.Width - 2));
            }

            Console.WriteLine(string.Empty.PadLeft(this.Width, '='));
        }

        public void PageFooter(string copyright)
        {
            // wip
        }
    }
}
