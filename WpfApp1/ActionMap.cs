using Newtonsoft.Json;
using System.Collections.Generic;

public class ActionMap
{
    [JsonProperty("@name")]
    // Gets or sets the name property for the ActionMap.
    public string Name { get; set; }

    [JsonProperty("action")]
    // Gets or sets the list of actions for the ActionMap.
    public List<Action> Actions { get; set; }
}

public class Action
{
    [JsonProperty("@name")]
    // Gets or sets the name property for the Action.
    public string Name { get; set; }

    [JsonProperty("rebind")]
    // Gets or sets the rebind property for the Action.
    public Rebind Rebind { get; set; }
}

public class Rebind
{
    [JsonProperty("@input")]
    // Gets or sets the input property for the Rebind.
    public string Input { get; set; }

    [JsonProperty("@activationMode")]
    // Gets or sets the activation mode property for the Rebind.
    public string ActivationMode { get; set; }
}