using System.Text.Json.Serialization;

namespace W4_assignment_template.Models;

public class Character
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("class")]
    public string Class { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("hp")]
    public int HP { get; set; }

    [JsonPropertyName("equipment")]
    public List<string> Equipment { get; set; }
}
