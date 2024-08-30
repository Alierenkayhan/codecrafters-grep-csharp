using System.Text.RegularExpressions;
static bool MatchPattern(string inputLine, string pattern)
{
    if (pattern.StartsWith("[^") && pattern.EndsWith("]"))
    {
        // Handle negated character class pattern like [^abc]
        var chars = pattern.Substring(2, pattern.Length - 3).ToCharArray();
        return !inputLine.Any(c => chars.Contains(c));
    }

    else if (pattern.StartsWith("[") && pattern.EndsWith("]"))
    {
        // Handle character class pattern like [abc]
        var chars = pattern.Substring(1, pattern.Length - 2).ToCharArray();
        return inputLine.Any(c => chars.Contains(c));
    }
    else if (pattern == @"\w")
    {
        // Handle word character pattern
        return inputLine.Any(char.IsLetterOrDigit);
    }
    else if (pattern == @"\d")
    {
        // Handle digit character pattern
        return inputLine.Any(char.IsDigit);
    }
    else if (pattern.Length == 1)
    {
        // Handle single character pattern
        return inputLine.Contains(pattern);
    }
    else
    {
        throw new ArgumentException($"Unhandled pattern: {pattern}");
    }

}

if (args[0] != "-E")
{
    Console.WriteLine("Expected first argument to be '-E'");
    Environment.Exit(2);
}

string pattern = args[1];
string inputLine = Console.In.ReadToEnd();

if (MatchPattern(inputLine, pattern))   Environment.Exit(0);
else  Environment.Exit(1);