namespace Linker.ConsoleUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Linker.Common.Extensions;
    using Linker.ConsoleUI.Extensions;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// The controller for the <see cref="Website"/>.
    /// </summary>
    public sealed class WebsiteController : BaseController<Website>, IWebsiteController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteController"/> class.
        /// </summary>
        /// <param name="websiteRepository">The <see cref="IWebsiteRepository"/> object.</param>
        public WebsiteController(IWebsiteRepository websiteRepository)
            : base(websiteRepository)
        {
        }

        /// <inheritdoc/>
        public override void DisplayAllItems()
        {
            Console.Clear();
            Console.WriteLine("List of collected links.");

            RenderLinks(this.repository.GetAll());

            _ = PromptForInput("Press ENTER to return to main menu...", "");

            static void RenderLinks(IEnumerable<Website> links)
            {
                const string displayTemplate = "|| {0} || {1} || {2} || {3} || {4} || {5} ||";
                const int dividerLength = 163;
                const int indexPad = 6;
                const int idPad = 38;
                const int namePad = 20;
                const int urlPad = 30;
                const int tagsPad = 20;
                const int modifiedPad = 23;

                Console.WriteLine("".PadRight(dividerLength, '='));
                Console.WriteLine(
                    displayTemplate,
                    "Index".PadRight(indexPad),
                    "Id".PadRight(idPad),
                    "Name".PadRight(namePad),
                    "Url".PadRight(urlPad),
                    "Tags".PadRight(tagsPad),
                    "Modified Date".PadRight(modifiedPad));
                Console.WriteLine("".PadRight(dividerLength, '='));

                foreach (var (link, index) in links.WithIndex())
                {
                    Console.WriteLine(
                        displayTemplate,
                        (index + 1).ToString().PadRight(indexPad),
                        link.Id.PadRight(idPad),
                        link.Name.TruncateWithEllipsis(namePad - 3).PadRight(namePad),
                        link.Url.TruncateWithEllipsis(urlPad - 3).PadRight(urlPad),
                        string.Join(", ", link.Tags).TruncateWithEllipsis(tagsPad - 3).PadRight(tagsPad),
                        link.ModifiedAt.ToString().PadRight(modifiedPad));
                }

                Console.WriteLine("".PadRight(dividerLength, '='));
            }
        }

        /// <inheritdoc/>
        public override void DisplayItemDetails()
        {
            Console.Clear();
            var id = PromptForInput("Enter the ID of the link: ", "");

            try
            {
                var link = this.repository.GetById(id);

                const int dividerLength = 100;
                const int labelPad = 15;
                const int contentPad = 83;

                Console.WriteLine("|{0}|", "".PadRight(dividerLength, '='));
                Console.WriteLine("|{0}|", "".PadRight(dividerLength, ' '));
                Console.WriteLine("|{0}|", link.Name.PadSides(dividerLength));
                Console.WriteLine("|{0}|", "".PadRight(dividerLength, ' '));
                Console.WriteLine("|{0}|", "".PadRight(dividerLength, '='));
                Console.WriteLine("|{0}: {1}|", "Id".PadRight(labelPad), link.Id.PadRight(contentPad));
                Console.WriteLine("|{0}|", "".PadRight(dividerLength, '-'));
                Console.WriteLine("|{0}: {1}|", "Url".PadRight(labelPad), link.Url.PadRight(contentPad));
                Console.WriteLine("|{0}|", "".PadRight(dividerLength, '-'));
                Console.WriteLine("|{0}: {1}| {2}: {3}|", "Language".PadRight(labelPad), link.Language.ToString().PadRight(30), "Aesthetics".PadRight(labelPad), link.Aesthetics.ToString().PadRight(34));
                Console.WriteLine("|{0}|", "".PadRight(dividerLength, '-'));
                Console.WriteLine("|{0}: {1}|", "Description".PadRight(labelPad), link.Description.TruncateWithEllipsis(contentPad - 3).PadRight(contentPad));
                Console.WriteLine("|{0}|", "".PadRight(dividerLength, '-'));
                Console.WriteLine("|{0}: {1}|", "Tags".PadRight(labelPad), string.Join(", ", link.Tags).PadRight(contentPad));
                Console.WriteLine("|{0}|", "".PadRight(dividerLength, '-'));
                Console.WriteLine("|{0}: {1}| {2}: {3}|", "Created".PadRight(labelPad), link.CreatedAt.ToString().PadRight(30), "Modified".PadRight(labelPad), link.ModifiedAt.ToString().PadRight(34));
                Console.WriteLine("|{0}|", "".PadRight(dividerLength, '='));

                Console.WriteLine("\nActions");
                Console.WriteLine("1. Visit link");
                Console.WriteLine("2. Get full URL");
                Console.WriteLine("3. Get full description");
                Console.WriteLine("4. Return\n");

                while (true)
                {
                    var input = PromptForInput("> ", "");
                    switch (input)
                    {
                        case "1":
                            System.Diagnostics.Process.Start("chrome.exe", link.Url);
                            break;
                        case "2":
                            Console.WriteLine("Full link: {0}", link.Url);
                            break;
                        case "3":
                            Console.WriteLine("Full description: {0}", link.Description);
                            break;
                        case "4":
                            return;
                        default:
                            Console.WriteLine("Please insert the correct selection!");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        /// <inheritdoc/>
        protected override Website GetItemFromInput()
        {
            const string labelTemplate = "{0}: ";
            const int labelPad = 13;

            var name = PromptForInput(labelTemplate, "Name".PadRight(labelPad));
            var url = PromptForInput(labelTemplate, "Url".PadRight(labelPad));
            var description = PromptForInput(labelTemplate, "Description".PadRight(labelPad));

            Console.WriteLine();
            DisplayEnum<Category>(nameof(Category));
            uint category, language, aesthetics;

            while (true)
            {
                try
                {
                    category = Convert.ToUInt32(PromptForInput(labelTemplate, "Category".PadRight(labelPad)));
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                    continue;
                }
            }

            Console.WriteLine();
            DisplayEnum<Language>(nameof(Language));

            while (true)
            {
                try
                {
                    language = Convert.ToUInt32(PromptForInput(labelTemplate, "Language".PadRight(labelPad)));
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                    continue;
                }
            }

            Console.WriteLine();
            DisplayEnum<Aesthetics>(nameof(Aesthetics));

            while (true)
            {
                try
                {
                    aesthetics = Convert.ToUInt32(PromptForInput(labelTemplate, "Aesthetics".PadRight(labelPad)));
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                    continue;
                }
            }

            var _tags = PromptForInput(labelTemplate, "Tags".PadRight(labelPad));
            var tags = _tags.Split(",").Select(tag => tag.Trim());

            return new Website
            {
                Name = name,
                Url = url,
                Category = (Category)category,
                Description = description,
                Tags = tags,
                Language = (Language)language,
                Aesthetics = (Aesthetics)aesthetics,
                IsSubdomain = false,
                IsMultilingual = false,
            };
        }
    }
}
