using Newtonsoft.Json;

namespace WpfApp1
{
    public class Category
    {
        [JsonProperty("Label")]
        // Gets or sets the label for the Category.
        public string Label { get; set; }
    }
}