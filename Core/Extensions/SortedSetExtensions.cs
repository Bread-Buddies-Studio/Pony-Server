using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions;

public static class SortedSetExtensions
{
    public static SortedSet<T> UpdateElement<T>(this SortedSet<T> set, T element)
    {
        // Update //
        if (set.Remove(element))
            set.Add(element);
        // Return new Set //
        return set;
    }

    public static void Update<T>(this SortedSet<T> set)
    {
        Span<T> cache = [.. set];
        set.Clear();

        foreach (var element in cache)
            set.Add(element);
    }

    public static void Update<T>(this SortedSet<T> set, Func<T, bool> predicate)
    {
        Span<T> cache = [.. set.Where(predicate)];
        set.Clear();

        foreach (var element in cache)
            set.Add(element);

        Debug.WriteLine(cache.Length);
    }
}