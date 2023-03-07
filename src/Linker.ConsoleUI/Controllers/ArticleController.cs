﻿namespace Linker.ConsoleUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Linker.Common.Extensions;
    using Linker.ConsoleUI.Extensions;
    using Linker.Core.Controllers;
    using Linker.Core.CsvModels;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

    /// <summary>
    /// The controller for the <see cref="Article"/>.
    /// </summary>
    internal sealed class ArticleController : BaseController<Article, CsvArticle>, IArticleController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleController"/> class.
        /// </summary>
        /// <param name="articleRepository">The <see cref="IArticleRepository"/> object.</param>
        public ArticleController(ICsvArticleRepository articleRepository)
            : base(articleRepository)
        {
        }

        /// <inheritdoc/>
        public override void DisplayAllItems()
        {
            if (!Console.IsOutputRedirected)
            {
                Console.Clear();
            }

            Console.WriteLine("List of collected links.");

            RenderLinks(this.repository.GetAll());

            _ = this.PromptForInput("Press ENTER to return to main menu...", string.Empty);

            static void RenderLinks(IEnumerable<Article> links)
            {
                const string displayTemplate = "|| {0} || {1} || {2} || {3} || {4} || {5} ||";
                const int dividerLength = 163;
                const int indexPad = 6;
                const int idPad = 38;
                const int namePad = 20;
                const int urlPad = 30;
                const int tagsPad = 20;
                const int modifiedPad = 23;

                Console.WriteLine(string.Empty.PadRight(dividerLength, '='));
                Console.WriteLine(
                    displayTemplate,
                    "Index".PadRight(indexPad),
                    "Id".PadRight(idPad),
                    "Name".PadRight(namePad),
                    "Url".PadRight(urlPad),
                    "Tags".PadRight(tagsPad),
                    "Modified Date".PadRight(modifiedPad));
                Console.WriteLine(string.Empty.PadRight(dividerLength, '='));

                foreach (var (link, index) in links.WithIndex())
                {
                    Console.WriteLine(
                        displayTemplate,
                        (index + 1).ToString().PadRight(indexPad),
                        link.Id.PadRight(idPad),
                        link.Title.TruncateWithEllipsis(namePad - 3).PadRight(namePad),
                        link.Url.TruncateWithEllipsis(urlPad - 3).PadRight(urlPad),
                        string.Join(", ", link.Tags).TruncateWithEllipsis(tagsPad - 3).PadRight(tagsPad),
                        link.ModifiedAt.ToString().PadRight(modifiedPad));
                }

                Console.WriteLine(string.Empty.PadRight(dividerLength, '='));
            }
        }

        /// <inheritdoc/>
        public override void DisplayItemDetails()
        {
            if (!Console.IsOutputRedirected)
            {
                Console.Clear();
            }

            var id = this.PromptForInput("Enter the ID of the link: ", string.Empty);

            try
            {
                var link = this.repository.GetById(id);

                const int dividerLength = 100;
                const int labelPad = 15;
                const int contentPad = 83;

                Console.WriteLine("|{0}|", string.Empty.PadRight(dividerLength, '='));
                Console.WriteLine("|{0}|", string.Empty.PadRight(dividerLength, ' '));
                Console.WriteLine("|{0}|", link.Title.PadSides(dividerLength));
                Console.WriteLine("|{0}|", string.Empty.PadRight(dividerLength, ' '));
                Console.WriteLine("|{0}|", string.Empty.PadRight(dividerLength, '='));
                Console.WriteLine("|{0}: {1}|", "Id".PadRight(labelPad), link.Id.PadRight(contentPad));
                Console.WriteLine("|{0}|", string.Empty.PadRight(dividerLength, '-'));
                Console.WriteLine("|{0}: {1}|", "Url".PadRight(labelPad), link.Url.PadRight(contentPad));
                Console.WriteLine("|{0}|", string.Empty.PadRight(dividerLength, '-'));
                Console.WriteLine("|{0}: {1}| {2}: {3}|", "Language".PadRight(labelPad), link.Language.ToString().PadRight(30), "Grammar".PadRight(labelPad), link.Grammar.ToString().PadRight(34));
                Console.WriteLine("|{0}|", string.Empty.PadRight(dividerLength, '-'));
                Console.WriteLine("|{0}: {1}|", "Description".PadRight(labelPad), link.Description.TruncateWithEllipsis(contentPad - 3).PadRight(contentPad));
                Console.WriteLine("|{0}|", string.Empty.PadRight(dividerLength, '-'));
                Console.WriteLine("|{0}: {1}|", "Tags".PadRight(labelPad), string.Join(", ", link.Tags).PadRight(contentPad));
                Console.WriteLine("|{0}|", string.Empty.PadRight(dividerLength, '-'));
                Console.WriteLine("|{0}: {1}| {2}: {3}|", "Created".PadRight(labelPad), link.CreatedAt.ToString().PadRight(30), "Modified".PadRight(labelPad), link.ModifiedAt.ToString().PadRight(34));
                Console.WriteLine("|{0}|", string.Empty.PadRight(dividerLength, '='));

                Console.WriteLine("\nActions");
                Console.WriteLine("1. Visit link");
                Console.WriteLine("2. Get full URL");
                Console.WriteLine("3. Get full description");
                Console.WriteLine("4. Return\n");

                while (true)
                {
                    var input = this.PromptForInput("> ", string.Empty);
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
        protected override Article GetItemFromInput()
        {
            const string labelTemplate = "{0}: ";
            const int labelPad = 13;

            var title = this.PromptForInput(labelTemplate, "Title".PadRight(labelPad));
            var url = this.PromptForInput(labelTemplate, "Url".PadRight(labelPad));
            var author = this.PromptForInput(labelTemplate, "Author".PadRight(labelPad));
            var description = this.PromptForInput(labelTemplate, "Description".PadRight(labelPad));
            var year = Convert.ToInt32(this.PromptForInput(labelTemplate, "Year".PadRight(labelPad)));

            Console.WriteLine();
            this.DisplayEnum<Category>(nameof(Category));
            uint category, language, grammar;

            while (true)
            {
                try
                {
                    category = Convert.ToUInt32(this.PromptForInput(labelTemplate, "Category".PadRight(labelPad)));
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
            }

            Console.WriteLine();
            this.DisplayEnum<Language>(nameof(Language));

            while (true)
            {
                try
                {
                    language = Convert.ToUInt32(this.PromptForInput(labelTemplate, "Language".PadRight(labelPad)));
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
            }

            Console.WriteLine();
            this.DisplayEnum<Grammar>(nameof(Grammar));

            while (true)
            {
                try
                {
                    grammar = Convert.ToUInt32(this.PromptForInput(labelTemplate, "Grammar".PadRight(labelPad)));
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
            }

            var _tags = this.PromptForInput(labelTemplate, "Tags".PadRight(labelPad));
            var tags = _tags.Split(",").Select(tag => tag.Trim());

            return new Article
            {
                Title = title,
                Url = url,
                Author = author,
                Year = year,
                Category = (Category)category,
                Description = description,
                Tags = tags,
                Language = (Language)language,
                Grammar = (Grammar)grammar,
                WatchLater = false,
            };
        }
    }
}
