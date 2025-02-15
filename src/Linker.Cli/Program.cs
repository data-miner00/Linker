namespace Linker.Cli;

using Linker.Cli.Commands;
using Linker.Cli.Integrations;

internal static class Program
{
    static async Task Main(string[] args)
    {
        var connStr = "Data Source=D:\\db.sqlite;";

        using var dbContext = new AppDbContext(connStr);
        var repo = new LinkRepository(dbContext);

        var command = ArgumentParser.Parse(args);

        switch (command.CommandType)
        {
            case CommandType.AddLink:
                if (command.CommandArguments is AddLinkCommandArguments alca)
                {
                    await repo.AddAsync(alca.ToLink());
                }

                break;
            case CommandType.ShowLinks:
                if (command.CommandArguments is ShowLinksCommandArguments slca)
                {
                    var links = await repo.GetAllAsync();

                    foreach (var (index, link) in links
                        .SkipOrAll(slca.Skip)
                        .TakeOrAll(slca.Top)
                        .Select((link, index) => (index, link)))
                    {
                        Console.WriteLine($"{index + 1}. {link.Url} - {link.Name}");
                    }
                }

                break;
            case CommandType.UpdateLink:
                {
                    if (command.CommandArguments is UpdateLinkCommandArguments ulca)
                    {
                        var original = await repo.GetByIdAsync(ulca.Id);

                        if (ulca.Url is not null)
                        {
                            original.Url = ulca.Url;
                        }

                        if (ulca.Name is not null)
                        {
                            original.Name = ulca.Name;
                        }

                        if (ulca.Description is not null)
                        {
                            original.Description = ulca.Description;
                        }

                        if (ulca.WatchLater)
                        {
                            original.WatchLater = true;
                        }

                        if (ulca.NoWatchLater)
                        {
                            original.WatchLater = false;
                        }

                        if (ulca.Tags is not null)
                        {
                            original.Tags = ulca.Tags;
                        }

                        if (ulca.ClearTags)
                        {
                            original.Tags = null;
                        }

                        if (ulca.AddTags.Count > 0)
                        {
                            var combined = string.Join(',', ulca.AddTags);
                            if (original.Tags is not null)
                            {
                                original.Tags = string.Join(',', original.Tags, combined);
                            }
                            else
                            {
                                original.Tags = combined;
                            }
                        }

                        if (ulca.RemoveTags.Count > 0 && original.Tags is not null) // prob write a warning here
                        {
                            string[] tempSplitted = original.Tags.Split(',');
                            var filteredTags = tempSplitted.Where(tag => !ulca.RemoveTags.Contains(tag)).ToArray();
                            original.Tags = string.Join(',', filteredTags);
                        }

                        if (ulca.Language is not null)
                        {
                            original.Language = ulca.Language;
                        }

                        original.ModifiedAt = DateTime.Now;

                        await repo.UpdateAsync(original);
                    }
                }

                break;

            case CommandType.DeleteLink:
                {
                    if (command.CommandArguments is DeleteLinkCommandArguments dlca)
                    {
                        if (!dlca.ConfirmDelete)
                        {
                            var linkToDelete = await repo.GetByIdAsync(dlca.Id);

                            Console.Write($"Confirm delete {linkToDelete.Url}? [y/N]: ");
                            var response = Console.ReadLine();

                            if (response is null || !response.StartsWith("y", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Delete aborted.");
                                return;
                            }
                        }

                        await repo.RemoveAsync(dlca.Id);
                    }
                }

                break;
        }

        // color cli
        // animation cli
        var prompt = @"
            hello world banner
            link --help
            link add https://www.what.com --name blah --description blah --watch-later --tags tag1,tag2,tag3 --lang en
            link show --top 10 --skip 20
            link update 1 --name blah --url https://updated.what.com --description blah --clear-tags --add-tag tag1 --remove-tag tag2 --lang kr
            link delete 1
            link visit 1
            link search nixos
            link list create 'my list' --description hello
            link list update 1 --name 'my 2 list' --description hello
            link list add 1 --linkId 1
            link list remove 1 --linkId 1
            link list delete 1
            link export --format csv,json,xml --destination home.csv
        ";
    }

    static IEnumerable<T> SkipOrAll<T>(this IEnumerable<T> src, int? skip)
    {
        return skip.HasValue ? src.Skip(skip.Value) : src;
    }

    static IEnumerable<T> TakeOrAll<T>(this IEnumerable<T> src, int? take)
    {
        return take.HasValue ? src.Take(take.Value) : src;
    }
}
