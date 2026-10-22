using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.SubjectForm;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.SubjectForm;

/// <summary>
/// Deterministic hash of an <see cref="IConditionalForm"/>: the conditioned form and the
/// condition name.
/// </summary>
public sealed record ConditionalFormHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        33,
        254,
        23,
        91,
        80,
        84,
        50,
        139,
        201,
        85,
        77,
        70,
        153,
        41,
        217,
        93,
    ];

    private readonly IConditionalForm _conditionalForm;

    public ConditionalFormHash(IConditionalForm conditionalForm)
    {
        _conditionalForm = conditionalForm;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new SubjectFormHash(_conditionalForm.Form))
                .Concat(new DeterminedHash(_conditionalForm.Condition))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
