using System.Text.Json.Serialization;

namespace W4_assignment_template.Models;

public class Character(string name, string @class, int level, int hp, List<string> equipment)
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = name;

    [JsonPropertyName("class")]
    public string Class { get; set; } = @class;

    [JsonPropertyName("level")]
    public int Level { get; set; } = level;

    [JsonPropertyName("hp")]
    public int HP { get; set; } = hp;

    [JsonPropertyName("equipment")]
    public List<string> Equipment { get; set; } = equipment ?? new List<string>();
}
