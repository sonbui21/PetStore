namespace Catalog.API.Extensions;

public static class StringExtensions
{
    public static string TruncateAtWord(this string input, int maxLength = 60)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
            return input;

        int lastSpace = input.LastIndexOf(' ', maxLength);

        if (lastSpace <= 0)
            return string.Concat(input.AsSpan(0, maxLength), "...");

        return string.Concat(input.AsSpan(0, lastSpace), "...");
    }
}
