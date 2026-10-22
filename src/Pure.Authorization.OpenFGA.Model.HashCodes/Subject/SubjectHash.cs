using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Object;
using Pure.Authorization.OpenFGA.Model.Abstractions.Subject;
using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Subject;

/// <summary>
/// Deterministic hash of any <see cref="ISubject"/>. It dispatches to the hash of the
/// concrete variant and yields exactly the same bytes, so <c>new SubjectHash(obj)</c>
/// equals <c>new ObjectHash(obj)</c>. Enumerating it throws
/// <see cref="ArgumentException"/> for an implementation outside the closed set of
/// variants.
/// </summary>
public sealed record SubjectHash : IDeterminedHash
{
    private readonly ISubject _subject;

    public SubjectHash(ISubject subject)
    {
        _subject = subject;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        IDeterminedHash hash = _subject switch
        {
            IObject obj => new ObjectHash(obj),
            ISubjectSet subjectSet => new SubjectSetHash(subjectSet),
            ITypedWildcard typedWildcard => new TypedWildcardHash(typedWildcard),
            _ => throw new ArgumentException(
                $"{_subject.GetType()} is not a known ISubject variant."
            ),
        };
        return hash.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
