namespace Linker.Core.Repositories
{
    /// <summary>
    /// The contract where repository is commitable.
    /// </summary>
    public interface ITransactionalRepository
    {
        /// <summary>
        /// Commit the changes to the database.
        /// </summary>
        /// <returns>0 for success and -1 for failed.</returns>
        public int Commit();
    }
}
