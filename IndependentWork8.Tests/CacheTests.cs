using System;
using Xunit;

public class CacheTests
{
    [Fact]
    public void Add_ShouldAddElement()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "Apple");

        Assert.True(cache.ContainsKey(1));
        Assert.Equal("Apple", cache.Get(1));
    }

    [Fact]
    public void Get_ShouldReturnValue()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "Apple");

        Assert.Equal("Apple", cache.Get(1));
    }

    [Fact]
    public void Add_ShouldRemoveOldestElementWhenFull()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "Apple");
        cache.Add(2, "Banana");
        cache.Add(3, "Orange");
        cache.Add(4, "Grape");

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.True(cache.ContainsKey(4));
    }

    [Fact]
    public void Get_ShouldThrowKeyNotFoundException()
    {
        var cache = new Cache<int, string>(3);

        Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
    }

    [Fact]
    public void ContainsKey_ShouldReturnTrueIfKeyExists()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "Apple");

        Assert.True(cache.ContainsKey(1));
        Assert.False(cache.ContainsKey(2));
    }

    [Fact]
    public void Count_ShouldReturnCorrectNumberOfElements()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "Apple");
        cache.Add(2, "Banana");

        Assert.Equal(2, cache.Count);

        cache.Add(3, "Orange");
        cache.Add(4, "Grape");

        Assert.Equal(3, cache.Count);
    }

    [Fact]
    public void Add_ShouldUpdateExistingKeyWithoutIncreasingCount()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "Apple");
        cache.Add(1, "Orange");

        Assert.Equal("Orange", cache.Get(1));
        Assert.Equal(1, cache.Count);
    }

    [Fact]
    public void Constructor_ShouldThrowExceptionForInvalidCapacity()
    {
        Assert.Throws<ArgumentException>(() => new Cache<int, string>(0));
    }
}
