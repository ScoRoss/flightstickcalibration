using Newtonsoft.Json;

public class DeviceInstance
{
    [JsonProperty("@instance")]
    // Gets or sets the instance for the DeviceInstance.
    public string Instance { get; set; }
}