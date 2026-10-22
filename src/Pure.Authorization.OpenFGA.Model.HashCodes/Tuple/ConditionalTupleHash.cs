using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Tuple;
using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.Authorization.OpenFGA.Model.HashCodes.Subject;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tuple;

/// <summary>
/// Deterministic hash of an <see cref="IConditionalTuple"/>: its subject, relation and
/// object, and its condition.
/// </summary>
public sealed record ConditionalTupleHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        222,
        62,
        103,
        201,
        69,
        237,
        101,
        33,
        15,
        47,
        168,
        222,
        171,
        235,
        132,
        39,
    ];

    private readonly IConditionalTuple _tuple;

    public ConditionalTupleHash(IConditionalTuple tuple)
    {
        _tuple = tuple;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new SubjectHash(_tuple.Subject))
                .Concat(new DeterminedHash(_tuple.Relation))
                .Concat(new ObjectHash(_tuple.Object))
                .Concat(new RelationshipConditionHash(_tuple.Condition))
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
