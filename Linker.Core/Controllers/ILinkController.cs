using System;
using System.Collections.Generic;
using System.Text;

namespace Linker.Core.Controllers
{
    public interface ILinkController
    {
        public void DisplayAllLinks();

        public void DisplaySingleLink();

        public void InsertLink();

        public void UpdateLink();

        public void RemoveLink();

        public void Save();
    }
}
