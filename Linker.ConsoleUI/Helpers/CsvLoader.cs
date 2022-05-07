namespace Linker.ConsoleUI.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using CsvHelper.Configuration;
    using static System.Globalization.CultureInfo;

    public static class CsvLoader
    {
        public static List<T> Load<V, T, U>(string path, Func<V, T> mapper)
            where U : ClassMap<V>
        {
            List<T> items;

            if (!File.Exists(path))
            {
                items = new List<T>();
                Console.WriteLine($"File to {path} not found");
            }
            else
            {
                using var streamReader = new StreamReader(path);
                using var csvReader = new CsvReader(streamReader, InvariantCulture);

                csvReader.Context.RegisterClassMap<U>();
                items = csvReader.GetRecords<V>().ToList().ConvertAll(
                    new Converter<V, T>(mapper));
            }

            return items;
        }
    }
}
