using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Object;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Object;

/// <summary>
/// Deterministic hash of an <see cref="IObject"/>: its type and its identifier.
/// </summary>
public sealed record ObjectHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        25,
        136,
        254,
        52,
        229,
        233,
        215,
        162,
        74,
        224,
        124,
        59,
        3,
        240,
        16,
        63,
    ];

    private readonly IObject _obj;

    public ObjectHash(IObject obj)
    {
        _obj = obj;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new DeterminedHash(_obj.Type))
                .Concat(new DeterminedHash(_obj.Id))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
