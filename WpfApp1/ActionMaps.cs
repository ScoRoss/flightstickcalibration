using Newtonsoft.Json;
using System.Collections.Generic;

public class ActionMaps
{
    [JsonProperty("@version")]
    // Gets or sets the version property for the ActionMaps.
    public string Version { get; set; }

    [JsonProperty("@optionsVersion")]
    // Gets or sets the options version property for the ActionMaps.
    public string OptionsVersion { get; set; }

    [JsonProperty("@rebindVersion")]
    // Gets or sets the rebind version property for the ActionMaps.
    public string RebindVersion { get; set; }

    [JsonProperty("@profileName")]
    // Gets or sets the profile name property for the ActionMaps.
    public string ProfileName { get; set; }

    [JsonProperty("CustomisationUIHeader")]
    // Gets or sets the customisation UI header property for the ActionMaps.
    public CustomisationUIHeader CustomisationUIHeader { get; set; }

    [JsonProperty("options")]
    // Gets or sets the list of options for the ActionMaps.
    public List<Option> Options { get; set; }

    [JsonProperty("modifiers")]
    // Gets or sets the modifiers property for the ActionMaps.
    public object Modifiers { get; set; }

    [JsonProperty("actionmap")]
    // Gets or sets the list of action maps for the ActionMaps.
    public List<ActionMap> ActionMapList { get; set; } 
}

public class Option
{
}