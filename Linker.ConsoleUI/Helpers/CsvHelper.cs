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
        /// <typeparam name="TCsvModel">The csv model for <typeparamref name="TModel"/>.</typeparam>
        /// <typeparam name="TModel">The model of objects to work with.</typeparam>
        /// <typeparam name="TClassMap">The classmap object for the conversion.</typeparam>
        /// <param name="path">The path to save the csv data.</param>
        /// <param name="mapper">The mapper that constructs <typeparamref name="TCsvModel"/> to <typeparamref name="TModel"/>.</param>
        /// <returns>The serialized list of <typeparamref name="TModel"/>.</returns>
        public static List<TModel> Load<TCsvModel, TModel, TClassMap>(string path, Func<TCsvModel, TModel> mapper)
            where TClassMap : ClassMap<TCsvModel>
            where TModel : Link
        {
            List<TModel> items;

            if (!File.Exists(path))
            {
                items = new List<TModel>();
                Console.WriteLine($"File to {path} not found");
            }
            else
            {
                using var streamReader = new StreamReader(path);
                using var csvReader = new CsvReader(streamReader, InvariantCulture);

                csvReader.Context.RegisterClassMap<TClassMap>();
                items = csvReader.GetRecords<TCsvModel>().ToList().ConvertAll(
                    new Converter<TCsvModel, TModel>(mapper));
            }

            return items;
        }

        /// <summary>
        /// Save the data to csv file.
        /// </summary>
        /// <typeparam name="TModel">The model of items to be saved.</typeparam>
        /// <typeparam name="TCsvModel">The csv model for <typeparamref name="TModel"/>.</typeparam>
        /// <param name="path">The path to save the data to.</param>
        /// <param name="mapper">The mapper that constructs <typeparamref name="TModel"/> to <typeparamref name="TCsvModel"/>.</param>
        /// <param name="items">The list of <typeparamref name="TModel"/> items.</param>
        /// <returns>0 for success and -1 for error.</returns>
        public static int Save<TModel, TCsvModel>(string path, Func<TModel, TCsvModel> mapper, List<TModel> items)
            where TModel : Link
        {
            try
            {
                using var streamWriter = new StreamWriter(path);
                using var csvWriter = new CsvWriter(streamWriter, InvariantCulture);

                var csvLinks = items.ConvertAll(
                    new Converter<TModel, TCsvModel>(mapper));

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
