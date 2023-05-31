using System.Text.Json.Serialization;

namespace CatalogService.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
