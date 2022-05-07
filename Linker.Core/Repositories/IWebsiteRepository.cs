namespace Linker.Core.Repositories
{
    using System.Collections.Generic;
    using Linker.Core.Models;

    public interface IWebsiteRepository
    {
        public IEnumerable<Website> GetAll();

        public Website GetById(string id);

        public void Update(Website link);

        public void Add(Website link);

        public void Remove(string id);

        public int Commit();
    }
}
