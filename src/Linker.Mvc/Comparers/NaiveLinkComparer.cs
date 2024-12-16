namespace Linker.Mvc.Comparers;

using Linker.Core.V2.Models;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The naive implementation to compare equality with <see cref="Link"/>.
/// </summary>
public sealed class NaiveLinkComparer : IEqualityComparer<Link>
{
    /// <inheritdoc/>
    public bool Equals(Link? x, Link? y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.Id.Equals(y.Id, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] Link obj)
    {
        // Check whether the object is null
        if (Object.ReferenceEquals(obj, null))
        {
            return 0;
        }

        // Get hash code for the Name field if it is not null.
        int hashProductName = obj.Name == null ? 0 : obj.Name.GetHashCode();

        // Get hash code for the Code field.
        int hashProductCode = obj.Id.GetHashCode();

        // Calculate the hash code for the obj.
        return hashProductName ^ hashProductCode;
    }
}
