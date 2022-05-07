namespace Linker.Core.Repositories
{
    using System.Collections.Generic;

    public interface IRepository<T>
    {
        public void Add(T item);

        public IEnumerable<T> GetAll();

        public T GetById(string id);

        public void Remove(string id);

        public void Update(T item);

        int Commit();
    }
}
