using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;

/// <summary>
/// Deterministic hash of an <see cref="IComputedRelation"/>: its relation.
/// </summary>
public sealed record ComputedRelationHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        20,
        147,
        206,
        2,
        41,
        130,
        155,
        31,
        202,
        83,
        86,
        50,
        107,
        155,
        238,
        159,
    ];

    private readonly IComputedRelation _computedRelation;

    public ComputedRelationHash(IComputedRelation computedRelation)
    {
        _computedRelation = computedRelation;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix.Concat(new DeterminedHash(_computedRelation.Relation))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
