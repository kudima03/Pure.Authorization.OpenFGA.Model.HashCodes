using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Subject;
using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Subject;

/// <summary>
/// Deterministic hash of an <see cref="ISubjectSet"/>: its object and its relation.
/// </summary>
public sealed record SubjectSetHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        128,
        202,
        156,
        201,
        228,
        107,
        177,
        88,
        136,
        132,
        202,
        19,
        153,
        241,
        68,
        238,
    ];

    private readonly ISubjectSet _subjectSet;

    public SubjectSetHash(ISubjectSet subjectSet)
    {
        _subjectSet = subjectSet;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new ObjectHash(_subjectSet.Object))
                .Concat(new DeterminedHash(_subjectSet.Relation))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
