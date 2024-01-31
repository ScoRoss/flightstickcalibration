using Newtonsoft.Json;
using System.Collections.Generic;

public class Categories
{
    [JsonProperty("category")]
    public List<Category> CategoryList { get; set; }
}