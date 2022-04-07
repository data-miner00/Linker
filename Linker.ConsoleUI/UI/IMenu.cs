namespace Linker.ConsoleUI.UI
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IMenu
    {
        public void DisplayBanner();

        public void PageHeader(string title, string version);

        public void GenerateMenu(IEnumerable<string> items);

        public void PageFooter(string copyright);
    }
}
