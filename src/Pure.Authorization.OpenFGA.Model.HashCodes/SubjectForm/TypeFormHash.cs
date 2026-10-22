using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.SubjectForm;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;

/// <summary>
/// Deterministic hash of an <see cref="ITypeForm"/>: its type.
/// </summary>
public sealed record TypeFormHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        223,
        29,
        170,
        233,
        25,
        44,
        54,
        197,
        190,
        237,
        229,
        93,
        20,
        231,
        13,
        42,
    ];

    private readonly ITypeForm _typeForm;

    public TypeFormHash(ITypeForm typeForm)
    {
        _typeForm = typeForm;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix.Concat(new DeterminedHash(_typeForm.Type))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
