using Newtonsoft.Json;

public class Category
{
    [JsonProperty("@label")]
    // Gets or sets the label for the Category.
    public string Label { get; set; }
}