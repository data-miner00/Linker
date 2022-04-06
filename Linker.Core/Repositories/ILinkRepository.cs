namespace Linker.Core.Repositories
{
    using System.Collections.Generic;
    using Linker.Core.Models;

    public interface ILinkRepository
    {
        public IEnumerable<Link> GetAll();

        public Link GetById(string id);

        public void Update(Link link);

        public void Add(Link link);

        public void Remove(string id);

        public int Commit();
    }
}
