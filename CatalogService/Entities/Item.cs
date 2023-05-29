using Newtonsoft.Json;

namespace CatalogService.Entities
{
    public class Item
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
    }
}
