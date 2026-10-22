using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.Authorization.OpenFGA.Model.HashCodes.Internals;
using Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Rewrite;

/// <summary>
/// Deterministic hash of an <see cref="IDirect"/>: the set of its subject forms, hashed
/// order-insensitively.
/// </summary>
public sealed record DirectHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        48,
        242,
        194,
        99,
        44,
        133,
        192,
        28,
        218,
        181,
        145,
        184,
        39,
        42,
        112,
        220,
    ];

    private readonly IDirect _direct;

    public DirectHash(IDirect direct)
    {
        _direct = direct;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix.Concat(
                new UnorderedHash(_direct.Forms.Select(x => new SubjectFormHash(x)))
            )
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
