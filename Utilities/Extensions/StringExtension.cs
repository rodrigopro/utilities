using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Utilities.Extensions;

public static partial class StringExtension
{
    public static bool IsEmail(this string value)
    {
        try
        {
            _ = new MailAddress(value);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public static string FormatEmail(this string value) => value.ToLower().Replace(" ", "");

    public static string RemoveDoubleSpacing(this string value)
    {
        var components = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return string.Join(' ', components);
    }

    public static string ToSlug(this string value)
    {
        var components = LettersAndDigitsOnlyRegex().Replace(value, " ").Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return string.Join('-', components).ToLower();
    }

    public static string DigitsOnly(this string value) => DigitsOnlyRegex().Replace(value, " ");

    public static string LettersOnly(this string value) => LettersOnlyRegex().Replace(value, " ");

    public static string LettersAndDigitsOnly(this string value) => LettersAndDigitsOnlyRegex().Replace(value, " ");

    public static string ToFullName(this string value, FullNameFormat fullNameFormat = FullNameFormat.AsItComes)
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
