using System;
using System.Text.RegularExpressions;

if (args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd().Trim();

int patternIndex = 0;
bool matched = false;

for (int inputIndex = 0; inputIndex < inputLine.Length && patternIndex < pattern.Length; inputIndex++)
{
    char currentPatternChar = pattern[patternIndex];

    if (currentPatternChar == '\\')
    {
        matched = IsMatch(pattern[patternIndex + 1], inputLine[inputIndex]);
        if (matched) patternIndex += 2;
    }
    else if (currentPatternChar == '[')
    {
        matched = HandleCharacterGroup(inputLine[inputIndex], pattern, ref patternIndex);
    }
    else
    {
        matched = inputLine[inputIndex] == currentPatternChar;
        if (matched) patternIndex++;
    }

    if (!matched)
        break;
}

try
{
    Regex regex = new Regex(pattern);
    if (regex.IsMatch(inputLine))
    {
        Console.WriteLine("Matched");
        Environment.Exit(0);
    }
    else
    {
        Console.WriteLine("Not matched");
        Environment.Exit(1);
    }
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid pattern: {ex.Message}");
    Environment.Exit(2);
}

bool IsMatch(char reg, char val)
{
    return reg switch
    {
        'd' => char.IsDigit(val),
        'w' => char.IsLetterOrDigit(val),
        _ => false
    };
}

bool HandleCharacterGroup(char inputChar, string pattern, ref int patternIndex)
{
    int endBracketIndex = pattern.IndexOf(']', patternIndex);
    if (endBracketIndex == -1)
    {
        throw new ArgumentException($"Unmatched '[' in pattern: {pattern}");
    }

    bool isNegativeGroup = pattern[patternIndex + 1] == '^';
    int startIndex = isNegativeGroup ? patternIndex + 2 : patternIndex + 1;
    string characterGroup = pattern.Substring(startIndex, endBracketIndex - startIndex);

    patternIndex = endBracketIndex + 1;

    bool isMatch = characterGroup.Contains(inputChar);
    return isNegativeGroup ? !isMatch : isMatch;
}
