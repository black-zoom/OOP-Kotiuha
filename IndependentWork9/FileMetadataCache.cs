using System;
using System.Collections.Generic;

public class FileMetadataCache
{
    private readonly Dictionary<string, FileMetadata> _cache =
        new(StringComparer.OrdinalIgnoreCase);

    public void Add(FileMetadata metadata)
    {
        _cache[metadata.FileName] = metadata;
    }

    public FileMetadata? Get(string fileName)
    {
        return _cache.TryGetValue(fileName, out var metadata)
            ? metadata
            : null;
    }

    public void Remove(string fileName)
    {
        _cache.Remove(fileName);
    }

    public bool Contains(string fileName)
    {
        return _cache.ContainsKey(fileName);
    }
}
