using System.Text.RegularExpressions;

static bool MatchPattern(string inputLine, string pattern)
{
    int inputIndex = 0;
    int patternIndex = 0;

    while (patternIndex < pattern.Length && inputIndex < inputLine.Length)
    {
        if (pattern[patternIndex] == '\\')
        {
            patternIndex++;

            if (patternIndex >= pattern.Length) 
                throw new ArgumentException("Invalid pattern: Trailing backslash");

            if (pattern[patternIndex] == 'd')
            {
                if (!char.IsDigit(inputLine[inputIndex]))   
                    return false;

                patternIndex++;
                inputIndex++;
            }
            else if (pattern[patternIndex] == 'w')
            {
                if (!char.IsLetterOrDigit(inputLine[inputIndex])) 
                    return false;

                patternIndex++;
                inputIndex++;
            }
            else
            {
                throw new ArgumentException($"Unhandled escape sequence: \\{pattern[patternIndex]}");
            }
        }
        else if (pattern[patternIndex] == '[')
        {
            int closeBracketIndex = pattern.IndexOf(']', patternIndex);
            if (closeBracketIndex == -1)
                throw new ArgumentException("Invalid pattern: Missing closing bracket");

            string charClass = pattern.Substring(patternIndex + 1, closeBracketIndex - patternIndex - 1);
            bool negate = charClass.StartsWith("^");
            if (negate) charClass = charClass.Substring(1);

            if (negate)
            {
                if (charClass.Contains(inputLine[inputIndex]))
                    return false;
            }
            else
            {
                if (!charClass.Contains(inputLine[inputIndex]))
                    return false;
            }

            patternIndex = closeBracketIndex + 1;
            inputIndex++;
        }
        else
        {
            if (pattern[patternIndex] != inputLine[inputIndex])
            {
                inputIndex++;
                continue;
            }

            patternIndex++;
            inputIndex++;
        }

        while (inputIndex < inputLine.Length && patternIndex < pattern.Length && inputLine[inputIndex] != pattern[patternIndex])
        {
            inputIndex++;
        }
    }
    return patternIndex == pattern.Length;
}

if (args.Length < 2 || args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd();

if (MatchPattern(inputLine, pattern)) 
    Environment.Exit(0);
else
    Environment.Exit(1);
