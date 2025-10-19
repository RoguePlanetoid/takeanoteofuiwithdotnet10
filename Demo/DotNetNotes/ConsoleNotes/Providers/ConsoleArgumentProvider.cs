namespace ConsoleNotes.Providers;

/// <summary>
/// Console Argument Provider
/// </summary>
/// <param name="args">Command Line Arguments</param>
internal class ConsoleArgumentProvider(string[] args)
{
    private const char dash = '-';
    private const char equals = '=';
    private const string present = "true";
    private const string double_dash = "--";

    /// <summary>
    /// Parses command line arguments into dictionary.
    /// </summary>
    /// <param name="args">Command Line Arguments</param>
    /// <returns>Arguments</returns>
    private static Dictionary<string, string> Parse(string[] args)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if(args == null)
            return dict;
        var tokens = args.ToList();
        for (int i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (!token.StartsWith(dash))
                continue;
            var body = token.StartsWith(double_dash, StringComparison.Ordinal)
                ? token[2..]
                : token[1..];
            if (string.IsNullOrWhiteSpace(body))
                continue;
            var parts = body.Split(equals, 2);
            var name = parts[0];
            string value;
            if (parts.Length == 2)
                value = parts[1];
            else if (i + 1 < tokens.Count && !tokens[i + 1].StartsWith(dash))
                value = tokens[++i];
            else
                value = present;
            if (!string.IsNullOrWhiteSpace(name))
                dict[name] = value;
        }
        return dict;
    }

    /// <summary>
    /// Arguments
    /// </summary>
    public Dictionary<string, string> Arguments => 
        Parse(args);
}
