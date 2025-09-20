using W4_assignment_template.Interfaces;
using W4_assignment_template.Models;

namespace W4_assignment_template.Services;

public class CsvFileHandler : IFileHandler
{
    private const string FilePath = "Data/input.csv";
    public List<Character> ReadCharacters()
    {
        // TODO: Implement CSV reading logic
        if (string.IsNullOrWhiteSpace(FilePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(FilePath));

        if (!File.Exists(FilePath))
            return new List<Character>();

        var records = File.ReadAllLines(FilePath);
        var characters = new List<Character>();

        if (records.Length <= 0) return characters;

        // Skip header
        for (int i = 1; i < records.Length; i++)
        {
            var record = records[i];
            if (string.IsNullOrWhiteSpace(record)) continue;

            var fields = SplitCsv(record);
            if (fields.Count < 5)
            {
                continue;
            }

            var name = fields[0];
            var @class = fields[1];
            if (!int.TryParse(fields[2], out var level)) level = 1;
            if (!int.TryParse(fields[3], out var hp)) hp = 0;


            var equipment = new List<string>();
            if (!string.IsNullOrWhiteSpace(fields[4]))
            {
                equipment = fields[4]
                    .Split('|', StringSplitOptions.RemoveEmptyEntries)
                    .Select(e => e.Trim())
                    .ToList();
            }

            characters.Add(new Character(name, @class, level, hp, equipment));
        }
        
        return characters;
        
        static List<string> SplitCsv(string? input)
        {
            var result = new List<string>();
            if (input is null)
            {
                return result;
            }

            var current = new System.Text.StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        // Escaped quote ("")
                        if (i + 1 < input.Length && input[i + 1] == '"')
                        {
                            current.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
                else
                {
                    if (c == ',')
                    {
                        result.Add(current.ToString());
                        current.Clear();
                    }
                    else if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
            }

            result.Add(current.ToString());
            return result;
        }
    }

    public void WriteCharacters(List<Character>? characters)
    {
        // TODO: Implement CSV writing logic
        if (string.IsNullOrWhiteSpace(FilePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(FilePath));
        if (characters is null || characters.Count == 0)
            return;

        var lastChar = characters.Last();
        var equipmentJoined = string.Join('|', lastChar.Equipment ?? Enumerable.Empty<string>());
        var newLine = $"{CsvEscape(lastChar.Name)},{CsvEscape(lastChar.Class)},{lastChar.Level},{lastChar.HP},{CsvEscape(equipmentJoined)}";

        if (!File.Exists(FilePath))
        {
            var header = "Name,Class,Level,HP,Equipment";
            File.WriteAllLines(FilePath, [header, newLine]);
        }
        else
        {
            File.AppendAllText(FilePath, newLine + Environment.NewLine);
        }
    }

    // Parses a CSV line handling quotes and commas within quotes
    private static List<string> ParseCsvLine(string line)
    {
        var result = new List<string>();
        if (line == null) return result;

        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (inQuotes)
            {
                if (c == '"')
                {
                    // Check for escaped quote ("")
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++; // skip next quote
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    current.Append(c);
                }
            }
            else
            {
                if (c == ',')
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else if (c == '"')
                {
                    inQuotes = true;
                }
                else
                {
                    current.Append(c);
                }
            }
        }

        result.Add(current.ToString());
        return result;
    }

    // Escapes a CSV field: wraps in quotes if it contains comma or quote; doubles any quotes
    private static string CsvEscape(string? field)
    {
        field ??= string.Empty;
        bool mustQuote = field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r');
        if (mustQuote)
        {
            var escaped = field.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
        return field;
    }
}