namespace Linker.ConsoleUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;
    using Linker.ConsoleUI.Extensions;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    public sealed class WebsiteController : IWebsiteController
    {
        private readonly IWebsiteRepository websiteRepository;

        public WebsiteController(IWebsiteRepository websiteRepository)
        {
            this.websiteRepository = EnsureArg.IsNotNull(websiteRepository, nameof(websiteRepository));
        }

        public void DisplayAllItems()
        {
            Console.Clear();
            Console.WriteLine("List of collected links.");

            RenderLinks(this.websiteRepository.GetAll());

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

        public void DisplayItemDetails()
        {
            Console.Clear();
            var id = PromptForInput("Enter the ID of the link: ", "");

            try
            {
                var link = this.websiteRepository.GetById(id);

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

        public void UpdateItem()
        {
            Console.Clear();

            this.DisplayAllItems();

            var id = PromptForInput("Please select the ID of the link to be updated: ", "");

            Console.WriteLine("\nInsert the details that needed to be changed. Hit Enter to skip.");

            var link = GetLinkFromInput();
            link.Id = id;

            try
            {
                this.websiteRepository.Update(link);
                Console.WriteLine("Successfully updated link!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed update link! Error: {0}", ex.Message);
            }

            _ = PromptForInput("\nPress ENTER to return to main menu...", "");
        }

        public void InsertItem()
        {
            Console.Clear();

            var newLink = GetLinkFromInput();
            var now = DateTime.Now;
            newLink.CreatedAt = now;
            newLink.ModifiedAt = now;

            this.websiteRepository.Add(newLink);

            Console.WriteLine("Successfully added new link!");
            _ = PromptForInput("\nPress ENTER to return to main menu...", "");
        }

        public static Website GetLinkFromInput()
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

        public void RemoveItem()
        {
            Console.Clear();

            this.DisplayAllItems();

            var id = PromptForInput("Select an ID to remove: ", "");

            try
            {
                this.websiteRepository.Remove(id);
                Console.WriteLine("Successfully removed link");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to remove link! Error: {0}", ex.Message);
            }

            _ = PromptForInput("\nPress ENTER to return to main menu...", "");
        }

        private static void DisplayEnum<T>(string name)
        {
            Console.WriteLine($"List of available {name}");
            var items = Enum.GetValues(typeof(T)).Cast<T>();

            foreach (var (item, index) in items.WithIndex())
            {
                Console.WriteLine("{0} {1}", (index + ".").PadRight(3), item);
            }
        }

        private static string PromptForInput(params object[] prompt)
        {
            var consoleWrite = typeof(Console)
                .GetMethods()
                .Where(x => x.Name == "Write" && x.IsStatic)
                .FirstOrDefault();

            consoleWrite.Invoke(null, prompt);

            var input = Console.ReadLine();
            return input;
        }

        public void SaveChanges()
        {
            this.websiteRepository.Commit();
        }
    }
}
