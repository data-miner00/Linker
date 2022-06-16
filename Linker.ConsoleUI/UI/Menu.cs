namespace Linker.ConsoleUI.UI
{
    using System;
    using System.Collections.Generic;
    using Linker.Common.Extensions;
    using Linker.ConsoleUI.Extensions;

    /// <summary>
    /// The actual class that implements <see cref="IMenu"/>.
    /// </summary>
    public sealed class Menu : IMenu
    {
        /// <summary>
        /// Gets or sets the width of the console that can display.
        /// </summary>
        public int Width { get; set; } = 61;

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void PageHeader(string title, string version)
        {
            Console.WriteLine(string.Empty.PadLeft(this.Width, '='));
            Console.WriteLine("|{0}|", string.Empty.PadLeft(this.Width - 2));
            Console.WriteLine("|{0}|", title.PadSides(this.Width - 2));
            Console.WriteLine("|{0}|", version.PadSides(this.Width - 2));
            Console.WriteLine("|{0}|", string.Empty.PadLeft(this.Width - 2));
            Console.WriteLine(string.Empty.PadLeft(this.Width, '='));
        }

        /// <inheritdoc/>
        public void GenerateMenu(IEnumerable<string> items)
        {
            foreach (var (item, index) in items.WithIndex())
            {
                Console.WriteLine(string.Empty.PadLeft(this.Width, '='));
                Console.WriteLine("|{0}|", $"{index + 1} - {item}".PadSides(this.Width - 2));
            }

            Console.WriteLine(string.Empty.PadLeft(this.Width, '='));
        }

        /// <inheritdoc/>
        public void PageFooter()
        {
            // wip
        }
    }
}
