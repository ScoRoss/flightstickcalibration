using Newtonsoft.Json;
using System.Collections.Generic;

public class ActionMap
{
    [JsonProperty("@name")]
    public string Name { get; set; }

    [JsonProperty("action")]
    public List<Action> Actions { get; set; }
}

public class Action
{
    [JsonProperty("@name")]
    public string Name { get; set; }

    [JsonProperty("rebind")]
    public Rebind Rebind { get; set; }
}

public class Rebind
{
    [JsonProperty("@input")]
    public string Input { get; set; }

    [JsonProperty("@activationMode")]
    public string ActivationMode { get; set; }
}