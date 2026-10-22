using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Internals;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;

/// <summary>
/// Deterministic hash of an <see cref="IUnion"/>: the set of its children, hashed
/// order-insensitively.
/// </summary>
public sealed record UnionHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        220,
        254,
        198,
        166,
        3,
        247,
        101,
        151,
        43,
        205,
        11,
        63,
        156,
        217,
        67,
        55,
    ];

    private readonly IUnion _union;

    public UnionHash(IUnion union)
    {
        _union = union;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix.Concat(
                new UnorderedHash(_union.Children.Select(x => new RewriteHash(x)))
            )
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
