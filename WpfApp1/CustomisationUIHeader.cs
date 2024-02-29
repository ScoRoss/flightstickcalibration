using Newtonsoft.Json;

public class CustomisationUIHeader
{
    [JsonProperty("@label")]
    // Gets or sets the label for the CustomisationUIHeader.
    public string Label { get; set; }

    [JsonProperty("@description")]
    // Gets or sets the description for the CustomisationUIHeader.
    public string Description { get; set; }

    [JsonProperty("@image")]
    // Gets or sets the image for the CustomisationUIHeader.
    public string Image { get; set; }

    [JsonProperty("devices")]
    // Gets or sets the devices for the CustomisationUIHeader.
    public Devices Devices { get; set; }

    [JsonProperty("categories")]
    // Gets or sets the categories for the CustomisationUIHeader.
    public Categories Categories { get; set; }
}