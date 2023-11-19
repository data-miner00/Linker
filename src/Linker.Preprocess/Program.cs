using System.Data.SQLite;
using System.Text.RegularExpressions;
using Linker.Preprocess;
using Linker.Preprocess.Models;
using Microsoft.Extensions.Logging;

Console.WriteLine("Link Preprocessor\n");

var connectionString = ReadConnectionString();
using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = factory.CreateLogger("Program");
var connection = new SQLiteConnection(connectionString);
var repository = new StorageRepository(connection, logger);

while (true)
{
    Console.WriteLine("Insert a record\n");
    Console.Write("Url                    : ");
    var url = Console.ReadLine();

    if (url == "-1" || url == null)
    {
        break;
    }

    Console.Write("Tags (seperated by ','): ");
    var tags = Console.ReadLine();

    if (tags == "-1" || tags == null)
    {
        break;
    }

    repository.AddAsync(new Link(url, tags)).GetAwaiter().GetResult();
}

static string ReadConnectionString()
{
    try
    {
        var text = File.ReadAllText("./appsettings.json");
        var pattern = new Regex("\"ConnectionString\": \"(.*?)\"");
        var match = pattern.Match(text);

        return match.Groups[1].Value;
    }
    catch
    {
        return string.Empty;
    }
}
