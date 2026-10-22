using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Tuple;
using Pure.Authorization.OpenFGA.Model.HashCodes.Internals;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tuple;

/// <summary>
/// Deterministic hash of an <see cref="IRelationshipCondition"/>: its name and the
/// canonical form of its context. Semantically equal JSON contexts hash equally: object
/// properties are sorted by ordinal key, arrays keep their order, primitives use their
/// JSON text, and a JSON <c>null</c> value differs from an absent property.
/// </summary>
public sealed record RelationshipConditionHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        70,
        122,
        123,
        228,
        28,
        240,
        28,
        112,
        185,
        202,
        96,
        68,
        106,
        39,
        125,
        181,
    ];

    private readonly IRelationshipCondition _condition;

    public RelationshipConditionHash(IRelationshipCondition condition)
    {
        _condition = condition;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new DeterminedHash(_condition.Name))
                .Concat(new DeterminedHash(new CanonicalJson(_condition.Context)))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
