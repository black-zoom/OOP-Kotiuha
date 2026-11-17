using System;
using System.Collections.Generic;

public class TagManager
{
    private readonly Dictionary<string, HashSet<string>> _articleTags =
        new(StringComparer.OrdinalIgnoreCase);

    public void AddTag(string articleId, string tag)
    {
        if (!_articleTags.ContainsKey(articleId))
            _articleTags[articleId] = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        _articleTags[articleId].Add(tag);
    }

    public void RemoveTag(string articleId, string tag)
    {
        if (_articleTags.TryGetValue(articleId, out var tags))
            tags.Remove(tag);
    }

    public HashSet<string> GetAllUniqueTags()
    {
        var result = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var tags in _articleTags.Values)
            result.UnionWith(tags);

        return result;
    }

    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        var result = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase);

        if (_articleTags.TryGetValue(articleId1, out var tags1) &&
            _articleTags.TryGetValue(articleId2, out var tags2))
        {
            result.UnionWith(tags1);
            result.IntersectWith(tags2);
        }

        return result;
    }
}
