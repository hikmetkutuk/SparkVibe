using System.Text;

namespace Shared.Helpers;

public static class StringExtensions
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

        return sb.ToString();
    }
}