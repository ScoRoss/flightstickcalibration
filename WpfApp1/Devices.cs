using Newtonsoft.Json;

public class Devices
{
    [JsonProperty("keyboard")]
    public DeviceInstance Keyboard { get; set; }

    [JsonProperty("mouse")]
    public DeviceInstance Mouse { get; set; }

    [JsonProperty("joystick")]
    public List<DeviceInstance> Joysticks { get; set; }
}