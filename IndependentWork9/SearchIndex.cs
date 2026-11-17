using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class SearchIndex
{
    private readonly Dictionary<string, HashSet<int>> _invertedIndex =
        new(StringComparer.OrdinalIgnoreCase);

    public void AddDocument(int docId, string content)
    {
        var words = Regex.Matches(content.ToLowerInvariant(), @"[\p{L}\p{N}]+#?");

        foreach (Match match in words)
        {
            string word = match.Value;

            if (!_invertedIndex.ContainsKey(word))
                _invertedIndex[word] = new HashSet<int>();

            _invertedIndex[word].Add(docId);
        }
    }

    public HashSet<int> Search(string word)
    {
        return _invertedIndex.TryGetValue(word, out var documents)
            ? new HashSet<int>(documents)
            : new HashSet<int>();
    }

    public HashSet<int> SearchMultiple(params string[] words)
    {
        if (words.Length == 0)
            return new HashSet<int>();

        var result = Search(words[0]);

        foreach (var word in words.Skip(1))
            result.IntersectWith(Search(word));

        return result;
    }
}
