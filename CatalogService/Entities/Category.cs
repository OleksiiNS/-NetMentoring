using Newtonsoft.Json;

namespace CatalogService.Entities
{
    public class Category
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Items")]
        public List<Item> Items { get; set; }
    }
}
