using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.SubjectForm;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;

/// <summary>
/// Deterministic hash of an <see cref="ISubjectSetForm"/>: its type and its relation.
/// </summary>
public sealed record SubjectSetFormHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        246,
        251,
        109,
        104,
        6,
        0,
        112,
        235,
        254,
        253,
        123,
        35,
        245,
        221,
        9,
        132,
    ];

    private readonly ISubjectSetForm _subjectSetForm;

    public SubjectSetFormHash(ISubjectSetForm subjectSetForm)
    {
        _subjectSetForm = subjectSetForm;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new DeterminedHash(_subjectSetForm.Type))
                .Concat(new DeterminedHash(_subjectSetForm.Relation))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
