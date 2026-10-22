using Pure.Authorization.OpenFGA.Model.Abstractions.Condition;
using Pure.Authorization.OpenFGA.Model.Abstractions.Model;
using Pure.Authorization.OpenFGA.Model.Abstractions.Rewrite;
using Pure.Primitives.Abstractions.String;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

internal sealed record FakeAuthorizationModel(
    IString SchemaVersion,
    IEnumerable<ITypeDefinition> Types,
    IEnumerable<ICondition> Conditions
) : IAuthorizationModel;

internal sealed record FakeTypeDefinition(
    IString Name,
    IEnumerable<IRelationDefinition> Relations
) : ITypeDefinition;

internal sealed record FakeRelationDefinition(IString Name, IRewrite Rewrite)
    : IRelationDefinition;
