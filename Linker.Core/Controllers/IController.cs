namespace Linker.Core.Controllers
{
    public interface IController<T>
    {
        public void DisplayAllItems();

        public void DisplayItemDetails();

        public void InsertItem();

        public void UpdateItem();

        public void RemoveItem();

        public void SaveChanges();
    }
}
