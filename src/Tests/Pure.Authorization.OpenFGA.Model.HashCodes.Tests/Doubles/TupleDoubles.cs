using System.Text.Json.Nodes;
using Pure.Authorization.OpenFGA.Model.Abstractions.Object;
using Pure.Authorization.OpenFGA.Model.Abstractions.Subject;
using Pure.Authorization.OpenFGA.Model.Abstractions.Tuple;
using Pure.Primitives.Abstractions.String;

namespace Pure.Authorization.OpenFGA.Model.HashCodes.Tests.Doubles;

internal sealed record FakeTuple(ISubject Subject, IString Relation, IObject Object)
    : ITuple;

internal sealed record FakeConditionalTuple(
    ISubject Subject,
    IString Relation,
    IObject Object,
    IRelationshipCondition Condition
) : IConditionalTuple;

internal sealed record FakeRelationshipCondition(IString Name, JsonNode Context)
    : IRelationshipCondition;
