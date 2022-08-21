namespace Linker.Core.Repositories
{
    using Linker.Core.Models;

    /// <summary>
    /// The contract that a csv based repository must adhere.
    /// </summary>
    /// <typeparam name="TModel">The object model itself.</typeparam>
    /// <typeparam name="TCsvModel">The csv counterpart of the model.</typeparam>
    public interface IInMemoryCsvRepository<TModel, TCsvModel> : IRepository<TModel>
        where TModel : Link
    {
        /// <summary>
        /// Converts the csv model to its model.
        /// </summary>
        /// <param name="csvModel">The model that represents the object when first read from csv.</param>
        /// <returns>The converted model that is used within the program.</returns>
        public TModel CsvModelToModel(TCsvModel csvModel);

        /// <summary>
        /// Converts model to its csv model.
        /// </summary>
        /// <param name="model">The model that represents the object that is used within the program.</param>
        /// <returns>The converted model that is used to save to the csv file.</returns>
        public TCsvModel ModelToCsvModel(TModel model);
    }
}
