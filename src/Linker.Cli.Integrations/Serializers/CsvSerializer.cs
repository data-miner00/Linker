namespace Linker.Cli.Integrations.Serializers;

using Linker.Cli.Core.Serializers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CsvHelper;

/// <summary>
/// The CSV serializer for <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The object type.</typeparam>
public sealed class CsvSerializer<T> : ISerializer<T>
{
    /// <inheritdoc/>
    public string Serialize(IEnumerable<T> items)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, leaveOpen: true))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(items);
            }

            memoryStream.Position = 0;

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: {0}", ex.Message);
            return string.Empty;
        }
    }

    /// <inheritdoc/>
    public string Serialize(T item)
    {
        throw new NotImplementedException();
    }
}
