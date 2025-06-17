using System.Text;

namespace Shared.Helpers;

public static partial class StringExtensions
{
    public static string ToAsciiOnly(this string input)
    {
        var replacements = new Dictionary<char, char>()
        {
            ['ü'] = 'u',
            ['Ü'] = 'U',
            ['ö'] = 'o',
            ['Ö'] = 'O',
            ['ğ'] = 'g',
            ['Ğ'] = 'G',
            ['ş'] = 's',
            ['Ş'] = 'S',
            ['ı'] = 'i',
            ['İ'] = 'I',
            ['ç'] = 'c',
            ['Ç'] = 'C',
        };

        var sb = new StringBuilder();
        foreach (var ch in input)
        {
            sb.Append(replacements.GetValueOrDefault(ch, ch));
        }

        return MyRegex().Replace(sb.ToString(), "");
    }

    [System.Text.RegularExpressions.GeneratedRegex(@"[^a-zA-Z0-9]")]
    private static partial System.Text.RegularExpressions.Regex MyRegex();
}