using System.Text.Json;
using W4_assignment_template.Interfaces;
using W4_assignment_template.Models;

// NOTE: The Character class uses [JsonProperty] attributes to map C# property names
// to the lowercase JSON keys required by the assignment. This ensures correct
// serialization and deserialization when reading from or writing to JSON files.

namespace W4_assignment_template.Services;

public class JsonFileHandler : IFileHandler
{
    private string _filePath = "Data/input.json";
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };
    
    public List<Character> ReadCharacters()
    {
        // TODO: Implement JSON reading logic
        List<Character> characters;
        
        if (string.IsNullOrWhiteSpace(_filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(_filePath));
        }
        else if (!File.Exists(_filePath))
        {
            characters = new List<Character>();
        }
        else
        {
            string json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                characters = new List<Character>();
            }
            else
            {
                try
                {
                    characters = JsonSerializer.Deserialize<List<Character>>(json, Options) ?? new List<Character>();
                }
                catch (JsonException ex)
                {
                    throw new InvalidDataException($"Invalid JSON content in '{_filePath}'.", ex);
                }
            }
        }
        return characters;
    }

    public void WriteCharacters(List<Character> characters)
    {
        // TODO: Implement JSON writing logic
        if (string.IsNullOrWhiteSpace(_filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(_filePath));
        }

        if (characters is null)
        {
            throw new ArgumentNullException(nameof(characters), "Characters list cannot be null.");
        }

        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(characters, Options);
        File.WriteAllText(_filePath, json);
    }
}
