namespace Linker.Cli;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Integrations;
using System.Diagnostics;
using System.Runtime.CompilerServices;

internal static class Program
{
    static async Task Main(string[] args)
    {
        var connStr = "Data Source=D:\\db.sqlite;";

        using var dbContext = new AppDbContext(connStr);
        var repo = new LinkRepository(dbContext);
        var visitRepo = new VisitRepository(dbContext);
        var listRepo = new UrlListRepository(dbContext);

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

            case CommandType.VisitLink:
                {
                    if (command.CommandArguments is VisitLinkCommandArguments vlca)
                    {
                        Link? linkToVisit;

                        if (vlca.LinkId is not null)
                        {
                            linkToVisit = await repo.GetByIdAsync(vlca.LinkId.Value);
                        }
                        else if (vlca.Random)
                        {
                            var links = (await repo.GetAllAsync()).ToArray();

                            var random = new Random();

                            var randomIndex = random.Next(links.Length);

                            linkToVisit = links[randomIndex];
                        }
                        else
                        {
                            throw new InvalidOperationException("Bad condition");
                        }

                        var startInfo = new ProcessStartInfo
                        {
                            FileName = linkToVisit.Url,
                            UseShellExecute = true,
                        };

                        Process.Start(startInfo);

                        var visit = new Visit
                        {
                            LinkId = linkToVisit.Id,
                            CreatedAt = DateTime.Now,
                        };

                        await visitRepo.AddAsync(visit);
                    }
                }

                break;
            case CommandType.CreateList:
                {
                    if (command.CommandArguments is CreateListCommandArguments cla)
                    {
                        var list = new UrlList
                        {
                            Name = cla.Name,
                            Description = cla.Description,
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now,
                        };

                        await listRepo.AddAsync(list);
                    }
                }

                break;

            case CommandType.ShowLists:
                {
                    if (command.CommandArguments is ShowListsCommandArguments slca2)
                    {
                        var lists = await listRepo.GetAllAsync();

                        foreach (var (index, link) in lists
                            .SkipOrAll(slca2.Skip)
                            .TakeOrAll(slca2.Top)
                            .Select((link, index) => (index, link)))
                        {
                            Console.WriteLine($"{index + 1}. {link.Name} - {link.Description}");
                        }
                    }
                }

                break;

            case CommandType.UpdateList:
                {
                    if (command.CommandArguments is UpdateListCommandArguments ulca)
                    {
                        var originalList = await listRepo.GetByIdAsync(ulca.Id);

                        if (ulca.Name is not null)
                        {
                            originalList.Name = ulca.Name;
                        }

                        if (ulca.Description is not null)
                        {
                            originalList.Description = ulca.Description;
                        }

                        await listRepo.UpdateAsync(originalList);
                    }
                }

                break;

            case CommandType.DeleteList:
                {
                    if (command.CommandArguments is DeleteListCommandArguments dlca)
                    {
                        var listToDelete = await listRepo.GetByIdAsync(dlca.Id);

                        if (!dlca.ConfirmDelete)
                        {
                            Console.Write($"Confirm delete {listToDelete.Name}? [y/N]: ");
                            var response = Console.ReadLine();

                            if (response is null || !response.StartsWith("y", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Delete aborted.");
                                return;
                            }
                        }

                        await listRepo.RemoveAsync(dlca.Id);
                    }
                }

                break;

            case CommandType.AddLinkIntoList:
                {
                    if (command.CommandArguments is AddLinkIntoListCommandArguments alilca)
                    {
                        var list = await listRepo.GetByIdAsync(alilca.ListId);
                        var link = await repo.GetByIdAsync(alilca.LinkId);

                        list.Links.Add(link);
                        await dbContext.SaveChangesAsync();
                    }
                }

                break;

            case CommandType.RemoveLinkFromList:
                {
                    if (command.CommandArguments is RemoveLinkFromListCommandArguments rlflca)
                    {
                        var list = await listRepo.GetByIdAsync(rlflca.ListId);
                        var link = await repo.GetByIdAsync(rlflca.LinkId);

                        var isSuccess = list.Links.Remove(link);
                        await dbContext.SaveChangesAsync();
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
            link list show
            link list create 'my list' --description hello
            link list update 1 --name 'my 2 list' --description hello
            link list add 1 1
            link list remove 1 1 --confirm
            link list delete 1 --confirm
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
