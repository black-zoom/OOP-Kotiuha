using System.Collections.Generic;
using System.Linq;

public class InvertedIndex
{
    private readonly Dictionary<string, HashSet<int>> _index;
    private readonly Dictionary<int, Document> _documents;

    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>();
        _documents = new Dictionary<int, Document>();
    }

    public void AddDocument(Document doc)
    {
        _documents[doc.Id] = doc;

        foreach (var word in TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(doc.Title + " " + doc.Content)).Distinct())
        {
            if (!_index.ContainsKey(word))
                _index[word] = new HashSet<int>();

            _index[word].Add(doc.Id);
        }
    }

    public List<Document> SearchSingleWord(string word)
    {
        string? normalized = TextProcessor.Tokenize(word).FirstOrDefault();

        if (normalized == null || !_index.TryGetValue(normalized, out var ids))
            return new List<Document>();

        return ids.Select(id => _documents[id])
            .OrderBy(doc => doc.Id)
            .ToList();
    }

    public List<Document> SearchMultipleWords(params string[] words)
    {
        var searchWords = words
            .SelectMany(TextProcessor.Tokenize)
            .Distinct()
            .ToList();

        if (searchWords.Count == 0)
            return new List<Document>();

        if (!_index.TryGetValue(searchWords[0], out var first))
            return new List<Document>();

        var result = new HashSet<int>(first);

        foreach (var word in searchWords.Skip(1))
        {
            if (!_index.TryGetValue(word, out var ids))
                return new List<Document>();

            result.IntersectWith(ids);
        }

        return result.Select(id => _documents[id])
            .OrderBy(doc => doc.Id)
            .ToList();
    }

    public List<Document> SearchAnyWord(params string[] words)
    {
        var searchWords = words
            .SelectMany(TextProcessor.Tokenize)
            .Distinct();

        var result = new HashSet<int>();

        foreach (var word in searchWords)
        {
            if (_index.TryGetValue(word, out var ids))
                result.UnionWith(ids);
        }

        return result.Select(id => _documents[id])
            .OrderBy(doc => doc.Id)
            .ToList();
    }

    public int DocumentCount => _documents.Count;

    public int UniqueWords => _index.Count;
}
