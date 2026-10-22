using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Internals;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;

/// <summary>
/// Deterministic hash of an <see cref="IIntersection"/>: the set of its children, hashed
/// order-insensitively.
/// </summary>
public sealed record IntersectionHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        98,
        201,
        240,
        131,
        229,
        232,
        229,
        91,
        131,
        88,
        117,
        200,
        166,
        161,
        27,
        53,
    ];

    private readonly IIntersection _intersection;

    public IntersectionHash(IIntersection intersection)
    {
        _intersection = intersection;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix.Concat(
                new UnorderedHash(_intersection.Children.Select(x => new RewriteHash(x)))
            )
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
