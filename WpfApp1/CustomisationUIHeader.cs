using Newtonsoft.Json;

public class CustomisationUIHeader
{
    [JsonProperty("@label")]
    public string Label { get; set; }

    [JsonProperty("@description")]
    public string Description { get; set; }

    [JsonProperty("@image")]
    public string Image { get; set; }

    [JsonProperty("devices")]
    public Devices Devices { get; set; }

    [JsonProperty("categories")]
    public Categories Categories { get; set; }
}
