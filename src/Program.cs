using System.Text.RegularExpressions;
static bool MatchPattern(string inputLine, string pattern)
{
    if (pattern[0] == '[' && pattern[^1] == ']') return inputLine.AsSpan().ContainsAny(pattern.AsSpan(1, pattern.Length - 2));
    else if (pattern == @"\w") return Regex.IsMatch(inputLine, @"\w");
    else if (pattern == @"\d") return Regex.IsMatch(inputLine, @"\d");   
    else if (pattern.Length == 1) return inputLine.Contains(pattern);
    else throw new ArgumentException($"Unhandled pattern: {pattern}");
    
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