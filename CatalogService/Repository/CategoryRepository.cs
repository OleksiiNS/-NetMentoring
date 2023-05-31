using CatalogService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private const int PageSize = 3;
        private readonly CategoryContext _context;

        public CategoryRepository() { 
            _context = new CategoryContext();
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.Include(i => i.Items).ToList();
        }

        public IEnumerable<Item> GetItems(int categoryId, int page)
        {
            return _context.Categories.Where(r => r.Id == categoryId).SelectMany(s => s.Items).Skip((page-1)*PageSize).Take(page*PageSize).ToList();
        }

        public Category AddCategory(string name)
        {
            var newId = _context.Categories.Any() ? _context.Categories.Max(r => r.Id) + 1 : 1;
            var newCategory = new Category
            {
                Id = newId,
                Name = name,
                Items = new List<Item>()
            };
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return newCategory;
        }

        public Category UpdateCategory(int id, string name)
        {
            var category = _context.Categories.First(r => r.Id == id);
            category.Name = name;
            _context.SaveChanges();
            return category;
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.First(r => r.Id == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public Item AddItem(int categoryId, string itemName)
        {
            var newId = _context.Items.Any() ? _context.Items.Max(r => r.Id) + 1 : 1;
            var category = _context.Categories.First(r => r.Id == categoryId);
            var item = new Item { Id = newId, Name = itemName, Category = category };
            _context.Items.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Item UpdateItem(int categoryId, int id, string name)
        {
            var updatedItem = _context.Categories.Where(r => r.Id == categoryId).SelectMany(r => r.Items).First(r => r.Id == id);
            updatedItem.Name = name;
            _context.SaveChanges();
            return updatedItem;
        }

        public void DeleteItem(int categoryId, int itemId)
        {
            var category = _context.Categories.Include(i=>i.Items).First(r => r.Id == categoryId);
            var item = category.Items.First(r => r.Id == itemId);
            category.Items.Remove(item);
            _context.SaveChanges();
        }
    }
}
