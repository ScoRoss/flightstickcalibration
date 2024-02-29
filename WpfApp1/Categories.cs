using Newtonsoft.Json;
using System.Collections.Generic;

public class Categories
{
    [JsonProperty("category")]
    // Gets or sets the list of categories for the Categories.
    public List<Category> CategoryList { get; set; }
}