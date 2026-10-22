using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.SubjectForm;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;

/// <summary>
/// Deterministic hash of any <see cref="ISubjectForm"/>. It dispatches to the hash of the
/// concrete variant and yields exactly the same bytes, so
/// <c>new SubjectFormHash(typeForm)</c> equals <c>new TypeFormHash(typeForm)</c>.
/// Enumerating it throws <see cref="ArgumentException"/> for an implementation outside
/// the closed set of variants.
/// </summary>
public sealed record SubjectFormHash : IDeterminedHash
{
    private readonly ISubjectForm _form;

    public SubjectFormHash(ISubjectForm form)
    {
        _form = form;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        IDeterminedHash hash = _form switch
        {
            ITypeForm typeForm => new TypeFormHash(typeForm),
            ISubjectSetForm subjectSetForm => new SubjectSetFormHash(subjectSetForm),
            IWildcardForm wildcardForm => new WildcardFormHash(wildcardForm),
            IConditionalForm conditionalForm => new ConditionalFormHash(conditionalForm),
            _ => throw new ArgumentException(
                $"{_form.GetType()} is not a known ISubjectForm variant."
            ),
        };
        return hash.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
