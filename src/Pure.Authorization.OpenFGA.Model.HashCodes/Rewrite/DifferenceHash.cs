using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;

/// <summary>
/// Deterministic hash of an <see cref="IDifference"/>: its base, then its subtracted
/// part. The order matters: <c>a but not b</c> and <c>b but not a</c> hash differently.
/// </summary>
public sealed record DifferenceHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        73,
        241,
        9,
        101,
        103,
        121,
        180,
        113,
        48,
        6,
        50,
        80,
        81,
        45,
        137,
        52,
    ];

    private readonly IDifference _difference;

    public DifferenceHash(IDifference difference)
    {
        _difference = difference;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new RewriteHash(_difference.Base))
                .Concat(new RewriteHash(_difference.Subtract))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
