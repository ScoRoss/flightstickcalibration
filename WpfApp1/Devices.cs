using Newtonsoft.Json;

public class Devices
{
    [JsonProperty("keyboard")]
    // Gets or sets the keyboard device instance for the Devices.
    public DeviceInstance Keyboard { get; set; }

    [JsonProperty("mouse")]
    // Gets or sets the mouse device instance for the Devices.
    public DeviceInstance Mouse { get; set; }

    [JsonProperty("joystick")]
    // Gets or sets the list of joystick device instances for the Devices.
    public List<DeviceInstance> Joysticks { get; set; }
}