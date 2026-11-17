using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class TextProcessor
{
    private static readonly HashSet<string> StopWords = new HashSet<string>
    {
        "a", "an", "the", "is", "are", "was", "were",
        "in", "on", "at", "to", "for", "of", "and", "or"
    };

    public static IEnumerable<string> Tokenize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Enumerable.Empty<string>();

        return Regex.Matches(text.ToLowerInvariant(), @"[\p{L}\p{N}+#]+")
            .Select(match => match.Value);
    }

    public static IEnumerable<string> RemoveStopWords(
        IEnumerable<string> words)
    {
        return words.Where(word => !StopWords.Contains(word));
    }
}
