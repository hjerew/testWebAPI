using System.Globalization;
using System.Text.Json.Serialization;
namespace testWebAPI.Models
{
    public class CountryItem
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("isDone")]
        public bool IsDone { get; set; }
    }
}
