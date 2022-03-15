using Linker.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linker.Core.Repositories
{
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
