using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Tuple;
using Pure.Authorization.OpenFGA.Model.HashCodes.Object;
using Pure.Authorization.OpenFGA.Model.HashCodes.Subject;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tuple;

/// <summary>
/// Deterministic hash of any <see cref="ITuple"/>. An <see cref="IConditionalTuple"/>
/// hashes with its condition, yielding exactly the bytes of
/// <see cref="ConditionalTupleHash"/>; a plain tuple hashes its subject, relation and
/// object only.
/// </summary>
public sealed record TupleHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        227,
        24,
        78,
        93,
        93,
        126,
        98,
        174,
        13,
        246,
        15,
        247,
        176,
        64,
        103,
        72,
    ];

    private readonly ITuple _tuple;

    public TupleHash(ITuple tuple)
    {
        _tuple = tuple;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        IDeterminedHash hash = _tuple switch
        {
            IConditionalTuple conditionalTuple => new ConditionalTupleHash(
                conditionalTuple
            ),
            _ => new DeterminedHash(
                TypePrefix
                    .Concat(new SubjectHash(_tuple.Subject))
                    .Concat(new DeterminedHash(_tuple.Relation))
                    .Concat(new ObjectHash(_tuple.Object))
            ),
        };
        return hash.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
