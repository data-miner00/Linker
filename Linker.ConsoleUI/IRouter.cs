using System;
using System.Collections.Generic;
using System.Text;

namespace Linker.ConsoleUI
{
    public interface IRouter
    {
        public void Website();

        public void Article();

        public void Youtube();

        public void AdHoc();
    }
}
