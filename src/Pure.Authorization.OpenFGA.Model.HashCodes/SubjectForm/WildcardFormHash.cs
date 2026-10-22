using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.SubjectForm;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;

/// <summary>
/// Deterministic hash of an <see cref="IWildcardForm"/>: its type.
/// </summary>
public sealed record WildcardFormHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        229,
        119,
        237,
        185,
        76,
        129,
        124,
        153,
        123,
        175,
        151,
        148,
        102,
        67,
        239,
        225,
    ];

    private readonly IWildcardForm _wildcardForm;

    public WildcardFormHash(IWildcardForm wildcardForm)
    {
        _wildcardForm = wildcardForm;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix.Concat(new DeterminedHash(_wildcardForm.Type))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
