namespace Linker.Cli;

using Linker.Cli.Commands;
using Linker.Cli.Integrations;

internal class Program
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
            link delete
            link visit
            link search nixos
            link list create 'my list' --description hello
            link list update 1 --name 'my 2 list' --description hello
            link list add 1 --linkId 1
            link list remove 1 --linkId 1
            link list delete 1
            link export --format csv,json,xml --destination home.csv
        ";
    }
}
