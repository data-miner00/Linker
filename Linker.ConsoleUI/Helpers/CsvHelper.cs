namespace Linker.ConsoleUI.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using global::CsvHelper;
    using global::CsvHelper.Configuration;
    using Linker.Core.Models;
    using static System.Globalization.CultureInfo;

    public static class CsvHelper
    {
        public static List<T> Load<V, T, U>(string path, Func<V, T> mapper)
            where U : ClassMap<V>
            where T : Link
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

        public static int Save<T, V>(string path, Func<T, V> mapper, List<T> items)
            where T : Link
        {
            try
            {
                using var streamWriter = new StreamWriter(path);
                using var csvWriter = new CsvWriter(streamWriter, InvariantCulture);

                var csvLinks = items.ConvertAll(
                    new Converter<T, V>(mapper));

                csvWriter.WriteRecords(csvLinks);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return -1;
            }
        }
    }
}
