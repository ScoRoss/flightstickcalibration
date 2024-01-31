using Newtonsoft.Json;

public class Category
{
    [JsonProperty("@label")]
    public string Label { get; set; }

    // Add other properties if needed
}