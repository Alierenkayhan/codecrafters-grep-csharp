using System;

if (args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd();

int patternIndex = 0;
bool matched = false;


bool startsWithAnchor = pattern[0] == '^';
if (startsWithAnchor)
{
    patternIndex++; 
}


if (startsWithAnchor && pattern.Length - 1 > inputLine.Length)
{
    Console.WriteLine("Not matched");
    Environment.Exit(1);
}

for (int inputIndex = 0; inputIndex < inputLine.Length; inputIndex++)
{

    if (startsWithAnchor && inputIndex > 0)
    {
        break;
    }

    if (patternIndex < pattern.Length && pattern[patternIndex] == '\\')
    {
        matched = IsMatch(pattern[patternIndex + 1], inputLine[inputIndex]);
        if (matched)
        {
            patternIndex += 2;
        }
    }
    else if (patternIndex < pattern.Length && pattern[patternIndex] == '[')
    {
        matched = IsMatch(pattern[patternIndex], inputLine[inputIndex]);
        if (matched)
        {
            patternIndex = pattern.IndexOf(']') + 1;
        }
    }
    else if (patternIndex < pattern.Length)
    {
        matched = inputLine[inputIndex] == pattern[patternIndex];
        if (matched)
        {
            patternIndex++;
        }
        else if (startsWithAnchor)
        {
            break;
        }
    }

    if (patternIndex == pattern.Length)
        break;
}

if (patternIndex != pattern.Length)
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
            return reg == val;
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
        comparisonChars = pattern.Substring(2, pattern.IndexOf("]") - 2);
        foreach (char c in comparisonChars)
        {
            if (inputLine.Contains(c))
            {
                return false;
            }
        }
        return true;
    }
    else
    {
        comparisonChars = pattern.Substring(1, pattern.IndexOf("]") - 1);
        foreach (char c in comparisonChars)
        {
            if (inputLine.Contains(c))
            {
                return true;
            }
        }
        return false;
    }
}
