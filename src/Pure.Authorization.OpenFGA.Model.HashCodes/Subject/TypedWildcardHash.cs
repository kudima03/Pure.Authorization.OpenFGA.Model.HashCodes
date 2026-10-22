using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Subject;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Subject;

/// <summary>
/// Deterministic hash of an <see cref="ITypedWildcard"/>: its type.
/// </summary>
public sealed record TypedWildcardHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        253,
        90,
        141,
        68,
        232,
        162,
        135,
        101,
        221,
        125,
        244,
        231,
        198,
        222,
        83,
        98,
    ];

    private readonly ITypedWildcard _typedWildcard;

    public TypedWildcardHash(ITypedWildcard typedWildcard)
    {
        _typedWildcard = typedWildcard;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix.Concat(new DeterminedHash(_typedWildcard.Type))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
