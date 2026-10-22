using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Model;
using Pure.Authorization.OpenFGA.Model.HashCodes.Condition;
using Pure.Authorization.OpenFGA.Model.HashCodes.Internals;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Model;

/// <summary>
/// Deterministic hash of an <see cref="IAuthorizationModel"/>: its schema version, the
/// set of its type definitions and the set of its conditions. Types and conditions are
/// hashed order-insensitively.
/// </summary>
public sealed record AuthorizationModelHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        241,
        251,
        246,
        170,
        118,
        57,
        209,
        66,
        188,
        78,
        77,
        6,
        253,
        40,
        133,
        135,
    ];

    private readonly IAuthorizationModel _model;

    public AuthorizationModelHash(IAuthorizationModel model)
    {
        _model = model;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new DeterminedHash(_model.SchemaVersion))
                .Concat(
                    new UnorderedHash(_model.Types.Select(x => new TypeDefinitionHash(x)))
                )
                .Concat(
                    new UnorderedHash(_model.Conditions.Select(x => new ConditionHash(x)))
                )
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
