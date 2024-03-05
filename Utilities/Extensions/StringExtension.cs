using System.Globalization;
using System.Text.RegularExpressions;

namespace Utilities.Extensions;

static partial class StringExtension
{
    static string RemoveDoubleSpacing(this string value)
    {
        var components = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return string.Join(' ', components);
    }

    static string ToSlug(this string value)
    {
        var components = LettersAndDigitsOnlyRegex().Replace(value, " ").Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return string.Join('-', components).ToLower();
    }

    static string DigitsOnlyRegex(this string value) => DigitsOnlyRegex().Replace(value, " ");

    static string LettersOnly(this string value) => LettersOnlyRegex().Replace(value, " ");

    static string LettersAndDigitsOnly(this string value) => LettersAndDigitsOnlyRegex().Replace(value, " ");

    static string ToFullName(this string value, FullNameFormat fullNameFormat = FullNameFormat.AsItComes)
    {
        value = value.RemoveDoubleSpacing();

        return fullNameFormat switch
        {
            FullNameFormat.CamelCase => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value),
            FullNameFormat.UpperCase => value.ToUpper(),
            FullNameFormat.LowerCase => value.ToLower(),
            _ => value,
        };
    }

    public enum FullNameFormat
    {
        AsItComes = 0,
        CamelCase = 1,
        UpperCase = 2,
        LowerCase = 3
    }

    [GeneratedRegex(@"[^0-9]")]
    private static partial Regex DigitsOnlyRegex();

    [GeneratedRegex("[^a-zA-Z]")]
    private static partial Regex LettersOnlyRegex();

    [GeneratedRegex("[^a-zA-Z0-9]")]
    private static partial Regex LettersAndDigitsOnlyRegex();
}
