// Category.cs
using Newtonsoft.Json;

namespace WpfApp1
{
    public class Category
    {
        [JsonProperty("Label")]
        public string Label { get; set; }

        // Add other properties if needed
    }
}