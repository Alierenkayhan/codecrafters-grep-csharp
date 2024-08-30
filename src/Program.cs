using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;
using System.IO;
using System;

if (args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}


string pattern = args[1];
string inputLine = Console.In.ReadToEnd();

int patternIndex = 0;
var matched = false;

if (pattern[0] == '^')
{
    patternIndex++;
}


for (int inputIndex = 0; inputIndex < inputLine.Length; inputIndex++)
{
    if (patternIndex == 0 && pattern[0] == '^' && inputIndex != 0)
        break;
 
    if (pattern[patternIndex] == '\\')
    {
        matched = IsMatch(pattern[patternIndex + 1], inputLine[inputIndex]);
        if (matched)
            patternIndex += 2;
    }
    else if (pattern[patternIndex] == '[')
    {
        matched = IsMatch(pattern[patternIndex], inputLine[inputIndex]);
        if (matched)
            patternIndex = pattern.IndexOf(']') + 1;
    }
    else
    {
        matched = inputLine[inputIndex] == pattern[patternIndex];
        if (matched)
            patternIndex++;
    }
    if (patternIndex == pattern.Length)
        break;
}
if (!matched || patternIndex < pattern.Length)
{
    Console.WriteLine("Not matched");
    Environment.Exit(1);
}
Console.WriteLine(matched);
Environment.Exit(0);

bool IsMatch(char reg, char val)
{
    switch (reg)
    {
        case 'd':
            return char.IsDigit(val);
        case 'w':
            return char.IsLetterOrDigit(val);
        case '[':
            return HandleCharacterGroup(inputLine, pattern);
        default:
            return false;
    }
}
static bool HandleCharacterGroup(string inputLine, string pattern)
{
    string comparisonChars;
    if (!pattern.Contains("]"))
    {
        throw new ArgumentException($"Unhandled pattern: {pattern}");
    }
    if (pattern[pattern.IndexOf("[") + 1] == '^')
    {
        comparisonChars = pattern.Substring(2, pattern.Length - 2);
        foreach (char c in comparisonChars)
        {
            if (inputLine.Contains(c))
            {
                Console.WriteLine($"{inputLine} contains {c}");
                return false;
            }
        }
        return true;
    }
    else
    {
        comparisonChars = pattern.Substring(1, pattern.Length - 2);
        foreach (char c in comparisonChars)
        {
            if (inputLine.Contains(c))
            {
                Console.WriteLine($"{inputLine} contains {c}");
                return true;
            }
        }
        return false;
    }
}