using System.Collections;
using Pure.Authorization.OpenFGA.Model.Abstractions.Model;
using Pure.Authorization.OpenFGA.Model.HashCodes.Internals;
using Pure.HashCodes;
using Pure.HashCodes.Abstractions;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Model;

/// <summary>
/// Deterministic hash of an <see cref="ITypeDefinition"/>: its name and the set of its
/// relations, hashed order-insensitively.
/// </summary>
public sealed record TypeDefinitionHash : IDeterminedHash
{
    private static readonly byte[] TypePrefix =
    [
        149,
        120,
        138,
        220,
        207,
        54,
        224,
        224,
        91,
        196,
        227,
        106,
        16,
        250,
        215,
        182,
    ];

    private readonly ITypeDefinition _typeDefinition;

    public TypeDefinitionHash(ITypeDefinition typeDefinition)
    {
        _typeDefinition = typeDefinition;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return new DeterminedHash(
            TypePrefix
                .Concat(new DeterminedHash(_typeDefinition.Name))
                .Concat(
                    new UnorderedHash(
                        _typeDefinition.Relations.Select(x => new RelationDefinitionHash(
                            x
                        ))
                    )
                )
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
