using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;

/// <summary>
/// Deterministic hash of an <see cref="IRelationFrom"/>: its relation and its tupleset.
/// </summary>
public sealed record RelationFromHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        170,
        74,
        229,
        34,
        155,
        70,
        128,
        141,
        7,
        44,
        0,
        35,
        17,
        86,
        130,
        121,
    ];

    private readonly IRelationFrom _relationFrom;

    public RelationFromHash(IRelationFrom relationFrom)
    {
        _relationFrom = relationFrom;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new DeterminedHash(_relationFrom.Relation))
                .Concat(new DeterminedHash(_relationFrom.Tupleset))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
