using Newtonsoft.Json;
using System.Collections.Generic;

public class ActionMaps
{
    [JsonProperty("@version")]
    public string Version { get; set; }

    [JsonProperty("@optionsVersion")]
    public string OptionsVersion { get; set; }

    [JsonProperty("@rebindVersion")]
    public string RebindVersion { get; set; }

    [JsonProperty("@profileName")]
    public string ProfileName { get; set; }

    [JsonProperty("CustomisationUIHeader")]
    public CustomisationUIHeader CustomisationUIHeader { get; set; }

    [JsonProperty("options")]
    public List<Option> Options { get; set; }

    [JsonProperty("modifiers")]
    public object Modifiers { get; set; }

    [JsonProperty("actionmap")]
    public List<ActionMap> ActionMapList { get; set; } // Change the property name to avoid conflict
}

public class Option
{
}