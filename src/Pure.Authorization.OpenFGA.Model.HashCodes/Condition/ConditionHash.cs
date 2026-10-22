using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Condition;
using Pure.Authorization.OpenFGA.Model.HashCodes.Internals;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Condition;

/// <summary>
/// Deterministic hash of an <see cref="ICondition"/>: its name, its expression and the
/// set of its parameters, hashed order-insensitively.
/// </summary>
public sealed record ConditionHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        132,
        233,
        26,
        57,
        162,
        185,
        110,
        213,
        99,
        27,
        235,
        36,
        236,
        109,
        187,
        6,
    ];

    private readonly ICondition _condition;

    public ConditionHash(ICondition condition)
    {
        _condition = condition;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new DeterminedHash(_condition.Name))
                .Concat(new DeterminedHash(_condition.Expression))
                .Concat(
                    new UnorderedHash(
                        _condition.Parameters.Select(x => new ConditionParameterHash(x))
                    )
                )
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
