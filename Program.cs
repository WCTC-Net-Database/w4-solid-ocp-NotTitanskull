using W4_assignment_template.Interfaces;
using W4_assignment_template.Models;
using W4_assignment_template.Services;

namespace W4_assignment_template;

class Program
{
    static IFileHandler fileHandler = new CsvFileHandler();
    static List<Character> characters = new();


    static void Main()
    {
        // (Stretch Goal): 
        // Allow the user to choose the file format (CSV or JSON) at runtime.
        // You can add a menu option to switch between CsvFileHandler and JsonFileHandler,
        // and update filePath accordingly (e.g., "input.csv" or "input.json").
        // This enables dynamic switching of file formats using the IFileHandler interface.
        
        characters = fileHandler.ReadCharacters();

        while (true)
        {
            PrintMenu();
            var userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    DisplayCharacters();
                    break;
                case "2":
                    FindCharacter();
                    break;
                case "3":
                    AddCharacter();
                    break;
                case "4":
                    LevelUpCharacter();
                    break;
                case "5":
                    ChangeFileType();
                    break;
                case "0":
                    fileHandler.WriteCharacters(characters);
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void PrintMenu()
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1.) Display Characters");
        Console.WriteLine("2.) Find Character");
        Console.WriteLine("3.) Add Character");
        Console.WriteLine("4.) Level Up Character");
        Console.WriteLine("5.) Change File Format (CSV/JSON)");
        Console.WriteLine("0.) Exit");
        Console.Write("> ");
    }

    static void DisplayCharacters()
    {
        Console.WriteLine("-----------");
        foreach (var character in characters)
        {
            Console.WriteLine($"Name: {character.Name}");
            Console.WriteLine($"Class: {character.Class}");
            Console.WriteLine($"Level: {character.Level}");
            Console.WriteLine($"HP: {character.HP}");
            Console.WriteLine("Equipment: " + (character.Equipment is { Count: > 0 }
                ? string.Join(", ", character.Equipment)
                : "(none)"));
            Console.WriteLine("-----------");
        }
    }
    
    static void FindCharacter()
    {
        Console.Write("Enter the name of the character to find: ");
        string nameToFind = Console.ReadLine();
        
        var character = characters.Find(c => c.Name.Equals(nameToFind, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(character != null
            ? $"Name: {character.Name}, Class: {character.Class}, Level: {character.Level}, HP: {character.HP}, Equipment: {string.Join(", ", character.Equipment)}"
            : "Character not found.");
    }


    static void AddCharacter()
    {
        // TODO: Implement logic to add a new character
        Console.Write("Enter Name: ");
        string charName = Console.ReadLine();
        
        Console.Write("Enter Class: ");
        string charClass = Console.ReadLine();
        
        int charLevel;
        while (true)
        {
            Console.Write("Enter Level: ");
            string inputLevel = Console.ReadLine();

            if (!int.TryParse(inputLevel, out charLevel))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }
                
            if (charLevel < 1)
            {
                Console.WriteLine("Level must be 1 or higher.");
                continue;
            }
            
            break; 
        }
        
        int charHitPoints;
        while (true)
        {
            Console.Write("Enter HP: ");
            string inputHP = Console.ReadLine();

            if (!int.TryParse(inputHP, out charHitPoints))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            if (charHitPoints < 1)
            {
                Console.WriteLine("HP must be 1 or higher.");
                continue;
            }

            break;
        }
        
        Console.WriteLine("Enter equipment items separately (Leave blank to finish): ");
        var charEquipment = new List<string>();
        while (true)
        {
            Console.Write("Enter equipment item: ");
            string equipmentInput = Console.ReadLine().Trim();
            
            if (equipmentInput.Length > 0)
            {
                charEquipment.Add(equipmentInput);
            }
            else if (string.IsNullOrWhiteSpace(equipmentInput))
            {
                break;
            }
        }
        
        var newCharacter = new Character(charName, charClass, charLevel, charHitPoints, charEquipment);
        characters.Add(newCharacter);
        
        fileHandler.WriteCharacters(characters);
        
        Console.WriteLine($"\nCharacter added: {charName}\n");
    }

    static void LevelUpCharacter()
    {
        foreach (var character in characters)
        {
            Console.WriteLine($"- {character.Name} (Level {character.Level})");
        }
        
        Console.Write("Enter the name of the character to level up: ");
        string charName = Console.ReadLine();

        var charOutput = characters.Find(Character => Character.Name.Equals(charName, StringComparison.OrdinalIgnoreCase));
        if (charOutput != null)
        {
            // TODO: Implement logic to level up the character
            charOutput.Level++;
            Console.WriteLine($"Character {charOutput.Name} leveled up to level {charOutput.Level}!");
        }
        else
        {
            Console.WriteLine("Character not found.");
        }
    }
    
    
    static void ChangeFileType()
    {
        Console.Write("Enter file type (CSV/JSON): ");
        string fileType = Console.ReadLine().Trim().ToLower();
        
        if (fileType == "csv")
        {
            fileHandler = new CsvFileHandler();
        }
        else if (fileType == "json")
        {
            fileHandler = new JsonFileHandler();
        }
        else
        {
            Console.WriteLine("Unsupported format.");
            return;
        }
        
        characters = fileHandler.ReadCharacters();
        
        var selectedType = fileHandler is CsvFileHandler ? "CSV" :
            fileHandler is JsonFileHandler ? "JSON" : "Unknown";

        Console.WriteLine($"File type changed to {selectedType}. Characters reloaded.");

    }
}