using CatalogService.Entities;

namespace CatalogService.Repository
{
    public interface ICategoryRepository
    {
        public IEnumerable<Category> GetCategories();
        public IEnumerable<Item> GetItems(int categoryId, int page);
        public Category AddCategory(string name);
        public Category UpdateCategory(int id, string name);
        public void DeleteCategory(int id);
        public Item AddItem(int categoryId, string itemName);
        public Item UpdateItem(int categoryId, int id, string name);
        public void DeleteItem(int categoryId, int itemId);
    }
}
