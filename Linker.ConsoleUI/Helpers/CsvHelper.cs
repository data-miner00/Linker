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

    /// <summary>
    /// The customized helper class for csv related endeavour.
    /// </summary>
    public static class CsvHelper
    {
        /// <summary>
        /// Loads the data from csv and serialize it into usable objects.
        /// </summary>
        /// <typeparam name="V">The csv model for <typeparamref name="T"/>.</typeparam>
        /// <typeparam name="T">The model of objects to work with.</typeparam>
        /// <typeparam name="U">The classmap object for the conversion.</typeparam>
        /// <param name="path">The path to save the csv data.</param>
        /// <param name="mapper">The mapper that constructs <typeparamref name="V"/> to <typeparamref name="T"/>.</param>
        /// <returns>The serialized list of <typeparamref name="T"/>.</returns>
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

        /// <summary>
        /// Save the data to csv file.
        /// </summary>
        /// <typeparam name="T">The model of items to be saved.</typeparam>
        /// <typeparam name="V">The csv model for <typeparamref name="T"/>.</typeparam>
        /// <param name="path">The path to save the data to.</param>
        /// <param name="mapper">The mapper that constructs <typeparamref name="T"/> to <typeparamref name="V"/>.</param>
        /// <param name="items">The list of <typeparamref name="T"/> items.</param>
        /// <returns>0 for success and -1 for error.</returns>
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
