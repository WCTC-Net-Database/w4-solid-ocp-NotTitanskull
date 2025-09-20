using W4_assignment_template.Models;

namespace W4_assignment_template.Interfaces;

public interface IFileHandler
{
    List<Character> ReadCharacters();
    void WriteCharacters(List<Character> characters);
}